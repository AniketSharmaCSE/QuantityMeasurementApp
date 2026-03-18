-- UC16 Schema: Quantity Measurement Application

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuantityMeasurementDb')
BEGIN
    CREATE DATABASE QuantityMeasurementDb;
END
GO

USE QuantityMeasurementDb;
GO

-- Main entity table: one row per measurement operation performed
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'quantity_measurement_entity'
)
BEGIN
    CREATE TABLE quantity_measurement_entity (
        Id                      INT IDENTITY(1,1)   NOT NULL CONSTRAINT PK_qme PRIMARY KEY,
        this_value              FLOAT               NOT NULL,
        this_unit               NVARCHAR(50)        NOT NULL,
        this_measurement_type   NVARCHAR(50)        NOT NULL,
        that_value              FLOAT               NULL,
        that_unit               NVARCHAR(50)        NULL,
        that_measurement_type   NVARCHAR(50)        NULL,
        operation               NVARCHAR(20)        NOT NULL,
        result_value            FLOAT               NULL,
        result_unit             NVARCHAR(50)        NULL,
        result_measurement_type NVARCHAR(50)        NULL,
        result_string           NVARCHAR(255)       NULL,
        is_error                BIT                 NOT NULL DEFAULT 0,
        error_message           NVARCHAR(500)       NULL,
        created_at              DATETIME2           NOT NULL DEFAULT GETDATE(),
        updated_at              DATETIME2           NOT NULL DEFAULT GETDATE()
    );

    CREATE INDEX idx_operation          ON quantity_measurement_entity (operation);
    CREATE INDEX idx_measurement_type   ON quantity_measurement_entity (this_measurement_type);
    CREATE INDEX idx_created_at         ON quantity_measurement_entity (created_at);

    PRINT 'Table quantity_measurement_entity created.';
END
ELSE
BEGIN
    PRINT 'Table quantity_measurement_entity already exists – skipped.';
END
GO

-- History/audit table: tracks every insert to the entity table for audit trail purposes.
-- entity_id references the entity that was created, operation_count tracks
-- how many times that entity has been accessed or updated.
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

    PRINT 'Table quantity_measurement_history created.';
END
ELSE
BEGIN
    PRINT 'Table quantity_measurement_history already exists – skipped.';
END
GO

-- SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
-- WHERE TABLE_NAME IN ('quantity_measurement_entity', 'quantity_measurement_history')
-- ORDER BY TABLE_NAME;
-- GO
