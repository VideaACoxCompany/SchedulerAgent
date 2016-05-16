CREATE TABLE [wis].[BatchStatistic]
(
    [BatchStatisticId] INT IDENTITY NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [DataGroupSource] NVARCHAR(255) NULL,
    [DataGroupTarget] NVARCHAR(255) NULL,
    [DataPackageSource] NVARCHAR(255) NULL,
    [DataPackageTarget] NVARCHAR(255) NULL,
    [Value] INT NOT NULL,
    [BatchStatisticTypeId] INT NOT NULL,
    [BatchProcessingQueueId] INT NOT NULL,
    [CreatedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [LastModifiedDateUtc] DATETIME NOT NULL DEFAULT getutcdate(),
    [RowVersion] ROWVERSION NOT NULL,
    CONSTRAINT [FK_BatchStatistic_BatchStatisticTypeId] FOREIGN KEY ([BatchStatisticTypeId]) REFERENCES [wis].[BatchStatisticType]([BatchStatisticTypeId]),
    CONSTRAINT [FK_BatchStatistic_BatchProcessingQueueId] FOREIGN KEY ([BatchProcessingQueueId]) REFERENCES [wis].[BatchProcessingQueue]([BatchProcessingQueueId])
)