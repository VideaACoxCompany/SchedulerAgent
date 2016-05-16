CREATE TABLE [wis].[BatchProcessingQueue]
(
    [BatchProcessingQueueId] INT IDENTITY NOT NULL PRIMARY KEY,
    [BatchName] NVARCHAR(256) NOT NULL,
    [BatchPath] NVARCHAR(max) NULL,
    [BatchProcessingStatusId] INT NOT NULL,
    [BatchProcessingTypeId] INT NOT NULL,
    [CreatedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
	[RetryCount] INT NOT NULL,
    [RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [BatchProcessingQueue_FK_BatchProcessingStatus_BatchProcessingStatusId] FOREIGN KEY ([BatchProcessingStatusId]) REFERENCES [wis].[BatchProcessingStatus]([BatchProcessingStatusId]),
    CONSTRAINT [FK_BatchProcessingType_BatchProcessingTypeId] FOREIGN KEY ([BatchProcessingTypeId]) REFERENCES [wis].[BatchProcessingType]([BatchProcessingTypeId])
)