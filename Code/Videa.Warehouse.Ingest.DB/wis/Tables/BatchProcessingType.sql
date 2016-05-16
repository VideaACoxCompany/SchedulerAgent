CREATE TABLE [wis].[BatchProcessingType]
(
    [BatchProcessingTypeId] INT IDENTITY NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(256) NOT NULL,
    [CreatedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [RowVersion] ROWVERSION NOT NULL
)