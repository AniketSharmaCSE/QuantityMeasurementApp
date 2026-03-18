using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository.Exception;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Util;

namespace QuantityMeasurement.Repository.Database
{
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository, IDisposable
    {
        private readonly ConnectionPool _pool;
        private bool _disposed;

        // inserts into the main entity table and returns the generated Id
        private const string SQL_INSERT_ENTITY = @"
            INSERT INTO quantity_measurement_entity
                (this_value, this_unit, this_measurement_type,
                 that_value, that_unit, that_measurement_type,
                 operation,
                 result_value, result_unit, result_measurement_type,
                 result_string, is_error, error_message)
            OUTPUT INSERTED.Id
            VALUES
                (@ThisValue, @ThisUnit, @ThisType,
                 @ThatValue, @ThatUnit, @ThatType,
                 @Operation,
                 @ResultValue, @ResultUnit, @ResultType,
                 @ResultString, @IsError, @ErrorMessage)";

        // writes an audit row to the history table linking back to the entity
        private const string SQL_INSERT_HISTORY = @"
            INSERT INTO quantity_measurement_history (entity_id, operation_count)
            VALUES (@EntityId, 1)";

        private const string SQL_SELECT_ALL = @"
            SELECT e.Id, e.this_value, e.this_unit, e.this_measurement_type,
                   e.that_value, e.that_unit, e.that_measurement_type,
                   e.operation,
                   e.result_value, e.result_unit, e.result_measurement_type,
                   e.result_string, e.is_error, e.error_message, e.created_at
            FROM quantity_measurement_entity e
            ORDER BY e.Id ASC";

        private const string SQL_SELECT_BY_OPERATION = @"
            SELECT e.Id, e.this_value, e.this_unit, e.this_measurement_type,
                   e.that_value, e.that_unit, e.that_measurement_type,
                   e.operation,
                   e.result_value, e.result_unit, e.result_measurement_type,
                   e.result_string, e.is_error, e.error_message, e.created_at
            FROM quantity_measurement_entity e
            WHERE LOWER(e.operation) = LOWER(@Operation)
            ORDER BY e.Id ASC";

        // filters by the measurement type of the first operand (this_measurement_type)
        private const string SQL_SELECT_BY_CATEGORY = @"
            SELECT e.Id, e.this_value, e.this_unit, e.this_measurement_type,
                   e.that_value, e.that_unit, e.that_measurement_type,
                   e.operation,
                   e.result_value, e.result_unit, e.result_measurement_type,
                   e.result_string, e.is_error, e.error_message, e.created_at
            FROM quantity_measurement_entity e
            WHERE LOWER(e.this_measurement_type) = LOWER(@Category)
               OR LOWER(e.that_measurement_type) = LOWER(@Category)
            ORDER BY e.Id ASC";

        private const string SQL_COUNT = @"
            SELECT COUNT(*) FROM quantity_measurement_entity";

        // deletes history rows first (FK constraint), then entity rows
        private const string SQL_DELETE_HISTORY = @"
            DELETE FROM quantity_measurement_history";

        private const string SQL_DELETE_ENTITY = @"
            DELETE FROM quantity_measurement_entity";

        // creates both tables if they don't exist yet
        private const string SQL_CREATE_ENTITY_TABLE = @"
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'quantity_measurement_entity'
            )
            BEGIN
                CREATE TABLE quantity_measurement_entity (
                    Id                      INT IDENTITY(1,1)   NOT NULL CONSTRAINT PK_qme PRIMARY KEY,
                    this_value              FLOAT               NOT NULL DEFAULT 0,
                    this_unit               NVARCHAR(50)        NOT NULL DEFAULT '',
                    this_measurement_type   NVARCHAR(50)        NOT NULL DEFAULT '',
                    that_value              FLOAT               NULL,
                    that_unit               NVARCHAR(50)        NULL,
                    that_measurement_type   NVARCHAR(50)        NULL,
                    operation               NVARCHAR(20)        NOT NULL DEFAULT '',
                    result_value            FLOAT               NULL,
                    result_unit             NVARCHAR(50)        NULL,
                    result_measurement_type NVARCHAR(50)        NULL,
                    result_string           NVARCHAR(255)       NULL,
                    is_error                BIT                 NOT NULL DEFAULT 0,
                    error_message           NVARCHAR(500)       NULL,
                    created_at              DATETIME2           NOT NULL DEFAULT GETDATE(),
                    updated_at              DATETIME2           NOT NULL DEFAULT GETDATE()
                );
                CREATE INDEX idx_operation        ON quantity_measurement_entity (operation);
                CREATE INDEX idx_measurement_type ON quantity_measurement_entity (this_measurement_type);
                CREATE INDEX idx_created_at       ON quantity_measurement_entity (created_at);
            END";

        private const string SQL_CREATE_HISTORY_TABLE = @"
            IF NOT EXISTS (
                SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = 'quantity_measurement_history'
            )
            BEGIN
                CREATE TABLE quantity_measurement_history (
                    Id              INT IDENTITY(1,1)   NOT NULL CONSTRAINT PK_qmh PRIMARY KEY,
                    entity_id       INT                 NOT NULL,
                    operation_count INT                 NOT NULL DEFAULT 1,
                    created_at      DATETIME2           NOT NULL DEFAULT GETDATE(),
                    updated_at      DATETIME2           NOT NULL DEFAULT GETDATE(),
                    CONSTRAINT FK_history_entity FOREIGN KEY (entity_id)
                        REFERENCES quantity_measurement_entity(Id)
                );
            END";

        public QuantityMeasurementDatabaseRepository(AppConfig config)
        {
            _pool = new ConnectionPool(config);
            EnsureSchemaExists();
            Console.WriteLine("[DatabaseRepository] Initialized with SQL Server persistence.");
        }

        public void Save(QuantityResponseDTO response)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();

                // Step 1: insert into entity table, get back the new row's Id
                int entityId;
                using (var cmd = new SqlCommand(SQL_INSERT_ENTITY, conn))
                {
                    cmd.Parameters.AddWithValue("@ThisValue",    response.Operand1?.Value    ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThisUnit",     response.Operand1?.Unit     ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThisType",     response.Operand1?.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThatValue",    (object?)response.Operand2?.Value    ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThatUnit",     (object?)response.Operand2?.Unit     ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ThatType",     (object?)response.Operand2?.Category ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Operation",    response.Operation ?? "");
                    cmd.Parameters.AddWithValue("@ResultValue",  (object?)response.Result?.Value    ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ResultUnit",   (object?)response.Result?.Unit     ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ResultType",   (object?)response.Result?.Category ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ResultString", response.ToString() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsError",      response.Success ? 0 : 1);
                    cmd.Parameters.AddWithValue("@ErrorMessage", string.IsNullOrEmpty(response.ErrorMessage)
                                                                 ? (object)DBNull.Value
                                                                 : response.ErrorMessage);

                    // OUTPUT INSERTED.Id returns the new PK as a scalar
                    object? result = cmd.ExecuteScalar();
                    entityId = Convert.ToInt32(result);
                }

                // Step 2: write an audit row in the history table referencing the entity
                using (var histCmd = new SqlCommand(SQL_INSERT_HISTORY, conn))
                {
                    histCmd.Parameters.AddWithValue("@EntityId", entityId);
                    histCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Save", $"Failed to save measurement: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null) _pool.Release(conn);
            }
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            return ExecuteQuery(SQL_SELECT_ALL, cmd => { });
        }

        public void Clear()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();

                // history must be deleted first because of the FK constraint to entity
                using (var histCmd = new SqlCommand(SQL_DELETE_HISTORY, conn))
                    histCmd.ExecuteNonQuery();

                using (var entCmd = new SqlCommand(SQL_DELETE_ENTITY, conn))
                {
                    int rows = entCmd.ExecuteNonQuery();
                    Console.WriteLine($"[DatabaseRepository] Cleared {rows} record(s).");
                }
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Clear", $"Failed to clear measurements: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null) _pool.Release(conn);
            }
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            return ExecuteQuery(SQL_SELECT_BY_OPERATION, cmd =>
                cmd.Parameters.AddWithValue("@Operation", operation));
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            return ExecuteQuery(SQL_SELECT_BY_CATEGORY, cmd =>
                cmd.Parameters.AddWithValue("@Category", category));
        }

        public int GetTotalCount()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                using var cmd = new SqlCommand(SQL_COUNT, conn);
                object? result = cmd.ExecuteScalar();
                return result is int count ? count : Convert.ToInt32(result);
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("GetTotalCount", $"Failed to count measurements: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null) _pool.Release(conn);
            }
        }

        public string GetPoolStatistics()
        {
            return _pool.GetStatistics();
        }

        public void ReleaseResources()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _pool.Dispose();
            Console.WriteLine("[DatabaseRepository] Resources released.");
        }

        private IReadOnlyList<QuantityResponseDTO> ExecuteQuery(
            string sql,
            Action<SqlCommand> parameterBinder)
        {
            SqlConnection? conn = null;
            var results = new List<QuantityResponseDTO>();
            try
            {
                conn = _pool.Acquire();
                using var cmd = new SqlCommand(sql, conn);
                parameterBinder(cmd);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    results.Add(MapRow(reader));
            }
            catch (SqlException ex)
            {
                throw new DatabaseException("Query", $"Failed to retrieve measurements: {ex.Message}", ex);
            }
            finally
            {
                if (conn != null) _pool.Release(conn);
            }
            return results.AsReadOnly();
        }

        private static QuantityResponseDTO MapRow(SqlDataReader reader)
        {
            bool isError = reader.GetBoolean(reader.GetOrdinal("is_error"));
            string operation = reader.GetString(reader.GetOrdinal("operation"));

            var dto = new QuantityResponseDTO
            {
                Operation    = operation,
                Success      = !isError,
                ErrorMessage = reader.IsDBNull(reader.GetOrdinal("error_message"))
                               ? string.Empty
                               : reader.GetString(reader.GetOrdinal("error_message")),

                Operand1 = new QuantityDTO(
                    reader.GetDouble(reader.GetOrdinal("this_value")),
                    reader.GetString(reader.GetOrdinal("this_unit")),
                    reader.GetString(reader.GetOrdinal("this_measurement_type"))
                ),

                // that_value is NULL for Convert operations (single-input)
                Operand2 = reader.IsDBNull(reader.GetOrdinal("that_value"))
                           ? null
                           : new QuantityDTO(
                               reader.GetDouble(reader.GetOrdinal("that_value")),
                               reader.GetString(reader.GetOrdinal("that_unit")),
                               reader.GetString(reader.GetOrdinal("that_measurement_type"))
                             ),

                // result_value is NULL for error responses
                Result = reader.IsDBNull(reader.GetOrdinal("result_value"))
                         ? null
                         : new QuantityDTO(
                               reader.GetDouble(reader.GetOrdinal("result_value")),
                               reader.GetString(reader.GetOrdinal("result_unit")),
                               reader.GetString(reader.GetOrdinal("result_measurement_type"))
                           )
            };
            return dto;
        }

        private void EnsureSchemaExists()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();

                // entity table must be created before history because history has a FK to it
                using (var cmd = new SqlCommand(SQL_CREATE_ENTITY_TABLE, conn))
                    cmd.ExecuteNonQuery();

                using (var cmd = new SqlCommand(SQL_CREATE_HISTORY_TABLE, conn))
                    cmd.ExecuteNonQuery();

                Console.WriteLine("[DatabaseRepository] Schema verified/created.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"[DatabaseRepository] Schema check warning: {ex.Message}");
            }
            finally
            {
                if (conn != null) _pool.Release(conn);
            }
        }
    }
}
