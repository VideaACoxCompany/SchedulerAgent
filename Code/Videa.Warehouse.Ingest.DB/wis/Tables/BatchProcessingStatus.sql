CREATE TABLE [wis].[BatchProcessingStatus]
(
    [BatchProcessingStatusId] INT IDENTITY NOT NULL PRIMARY KEY,
    [StatusName] NVARCHAR(256) NOT NULL,
    [CreatedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [RowVersion] ROWVERSION NOT NULL
)