using System.Data;
using Microsoft.Data.SqlClient;
using QuantityMeasurement.Model.DTOs;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Util;

namespace QuantityMeasurement.Repository.Database
{
    // used by the ConsoleApp when RepositoryType = "database"
    // uses raw SqlConnection/SqlCommand (UC16 style) instead of EF
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;
        private static int _activeConnections = 0;

        public QuantityMeasurementDatabaseRepository(AppConfig config)
        {
            _connectionString = config.ConnectionString;
            EnsureTableExists();
        }

        private SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            Interlocked.Increment(ref _activeConnections);
            return conn;
        }

        private void EnsureTableExists()
        {
            try
            {
                using var conn = OpenConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'quantity_measurements_ef')
                    CREATE TABLE quantity_measurements_ef (
                        Id               UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                        Operation        NVARCHAR(50)  NOT NULL,
                        Operand1Value    FLOAT,
                        Operand1Unit     NVARCHAR(MAX),
                        Operand1Category NVARCHAR(450),
                        Operand2Value    FLOAT,
                        Operand2Unit     NVARCHAR(MAX),
                        Operand2Category NVARCHAR(MAX),
                        ResultValue      FLOAT,
                        ResultUnit       NVARCHAR(MAX),
                        ResultCategory   NVARCHAR(MAX),
                        BoolResult       BIT,
                        ScalarResult     FLOAT,
                        HasError         BIT NOT NULL DEFAULT 0,
                        ErrorMessage     NVARCHAR(MAX),
                        Timestamp        DATETIME2 NOT NULL DEFAULT GETUTCDATE()
                    )";
                cmd.ExecuteNonQuery();
            }
            catch { /* table already exists or DB unavailable */ }
        }

        public void Save(QuantityResponseDTO response)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO quantity_measurements_ef
                    (Operation, Operand1Value, Operand1Unit, Operand1Category,
                     Operand2Value, Operand2Unit, Operand2Category,
                     ResultValue, ResultUnit, ResultCategory,
                     BoolResult, ScalarResult, HasError, ErrorMessage, Timestamp)
                VALUES
                    (@op, @v1, @u1, @c1, @v2, @u2, @c2,
                     @rv, @ru, @rc, @br, @sr, @err, @errmsg, @ts)";

            cmd.Parameters.AddWithValue("@op",     response.Operation);
            cmd.Parameters.AddWithValue("@v1",     (object?)response.Operand1?.Value    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@u1",     (object?)response.Operand1?.Unit     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@c1",     (object?)response.Operand1?.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@v2",     (object?)response.Operand2?.Value    ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@u2",     (object?)response.Operand2?.Unit     ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@c2",     (object?)response.Operand2?.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rv",     (object?)response.Result?.Value      ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ru",     (object?)response.Result?.Unit       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@rc",     (object?)response.Result?.Category   ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@br",     (object?)response.BoolResult         ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sr",     (object?)response.ScalarResult       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@err",    !response.Success);
            cmd.Parameters.AddWithValue("@errmsg", (object?)response.ErrorMessage       ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ts",     response.Timestamp);
            cmd.ExecuteNonQuery();
        }

        public IReadOnlyList<QuantityResponseDTO> GetAll()
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM quantity_measurements_ef ORDER BY Timestamp DESC";
            return ReadResults(cmd);
        }

        public IReadOnlyList<QuantityResponseDTO> GetRecent(int count)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT TOP {count} * FROM quantity_measurements_ef ORDER BY Timestamp DESC";
            return ReadResults(cmd);
        }

        public IReadOnlyList<QuantityResponseDTO> GetByOperation(string operation)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM quantity_measurements_ef WHERE LOWER(Operation) = LOWER(@op) ORDER BY Timestamp DESC";
            cmd.Parameters.AddWithValue("@op", operation);
            return ReadResults(cmd);
        }

        public IReadOnlyList<QuantityResponseDTO> GetByCategory(string category)
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM quantity_measurements_ef WHERE LOWER(Operand1Category) = LOWER(@cat) ORDER BY Timestamp DESC";
            cmd.Parameters.AddWithValue("@cat", category);
            return ReadResults(cmd);
        }

        public int GetTotalCount()
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM quantity_measurements_ef";
            return (int)cmd.ExecuteScalar()!;
        }

        public string GetPoolStatistics() =>
            $"SQL Server - active connections: {_activeConnections}, total records: {GetTotalCount()}";

        public void Clear()
        {
            using var conn = OpenConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM quantity_measurements_ef";
            cmd.ExecuteNonQuery();
        }

        public void ReleaseResources() { }

        private static IReadOnlyList<QuantityResponseDTO> ReadResults(SqlCommand cmd)
        {
            var list = new List<QuantityResponseDTO>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new QuantityResponseDTO
                {
                    Success   = !reader.GetBoolean(reader.GetOrdinal("HasError")),
                    Operation = reader.GetString(reader.GetOrdinal("Operation")),
                    Operand1  = reader.IsDBNull(reader.GetOrdinal("Operand1Value")) ? null
                        : new QuantityDTO(
                            reader.GetDouble(reader.GetOrdinal("Operand1Value")),
                            reader.IsDBNull(reader.GetOrdinal("Operand1Unit"))     ? "" : reader.GetString(reader.GetOrdinal("Operand1Unit")),
                            reader.IsDBNull(reader.GetOrdinal("Operand1Category")) ? "" : reader.GetString(reader.GetOrdinal("Operand1Category"))),
                    Operand2  = reader.IsDBNull(reader.GetOrdinal("Operand2Value")) ? null
                        : new QuantityDTO(
                            reader.GetDouble(reader.GetOrdinal("Operand2Value")),
                            reader.IsDBNull(reader.GetOrdinal("Operand2Unit"))     ? "" : reader.GetString(reader.GetOrdinal("Operand2Unit")),
                            reader.IsDBNull(reader.GetOrdinal("Operand2Category")) ? "" : reader.GetString(reader.GetOrdinal("Operand2Category"))),
                    Result    = reader.IsDBNull(reader.GetOrdinal("ResultValue")) ? null
                        : new QuantityDTO(
                            reader.GetDouble(reader.GetOrdinal("ResultValue")),
                            reader.IsDBNull(reader.GetOrdinal("ResultUnit"))     ? "" : reader.GetString(reader.GetOrdinal("ResultUnit")),
                            reader.IsDBNull(reader.GetOrdinal("ResultCategory")) ? "" : reader.GetString(reader.GetOrdinal("ResultCategory"))),
                    BoolResult   = reader.IsDBNull(reader.GetOrdinal("BoolResult"))   ? null : reader.GetBoolean(reader.GetOrdinal("BoolResult")),
                    ScalarResult = reader.IsDBNull(reader.GetOrdinal("ScalarResult")) ? null : reader.GetDouble(reader.GetOrdinal("ScalarResult")),
                    ErrorMessage = reader.IsDBNull(reader.GetOrdinal("ErrorMessage")) ? null : reader.GetString(reader.GetOrdinal("ErrorMessage")),
                    Timestamp    = reader.GetDateTime(reader.GetOrdinal("Timestamp"))
                });
            }
            return list.AsReadOnly();
        }
    }
}
