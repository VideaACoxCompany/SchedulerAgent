CREATE TABLE [wis].[BatchProcessingStep]
(
    [BatchProcessingStepId] INT IDENTITY NOT NULL PRIMARY KEY,
    [StepName] NVARCHAR(256) NOT NULL,
    [BatchProcessingQueueId] INT NOT NULL,
	[BatchProcessingStatusId] INT NOT NULL,
    [StartDateTime] DATETIME NOT NULL DEFAULT getdate(),
    [EndDateTime] DATETIME,	
	[CompleteByTime] DATETIME NOT NULL,
	CONSTRAINT [BatchProcessingStep_FK_BatchProcessingStatus_BatchProcessingStatusId] FOREIGN KEY ([BatchProcessingStatusId]) REFERENCES [wis].[BatchProcessingStatus]([BatchProcessingStatusId]),
    CONSTRAINT [FK_BatchProcessingStep_BatchProcessingQueueId] FOREIGN KEY ([BatchProcessingQueueId]) REFERENCES [wis].[BatchProcessingQueue]([BatchProcessingQueueId])
)