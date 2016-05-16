using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Tests.Services
{
    [TestClass]
    public class BatchProcessingServiceTests
    {
        private Mock<IWarehouseIngestDataService<BatchProcessingQueue>> _mockBatchQueueDataService;
        private Mock<ILogger> _mockLogger;

        private Mock<IBatchProcessingStepService> _mockBatchProcessingStepService;
        private Mock<IWarehouseIngestUoWService> _mockWarehouseIngestUnitOfWork;

        private BatchProcessingService _batchProcessingService;


        [TestInitialize]
        public void Setting()
        {
            _mockBatchQueueDataService = new Mock<IWarehouseIngestDataService<BatchProcessingQueue>>();
            _mockLogger = new Mock<ILogger>();

            _mockBatchProcessingStepService = new Mock<IBatchProcessingStepService>();
            _mockWarehouseIngestUnitOfWork = new Mock<IWarehouseIngestUoWService>();

            _batchProcessingService = new BatchProcessingService(_mockBatchQueueDataService.Object, _mockBatchProcessingStepService.Object,
                _mockWarehouseIngestUnitOfWork.Object, _mockLogger.Object);
        }
        

        [TestMethod]
        [Description("Test Update Batch status and also add or update batch step.")]
        public void Test_UpdateBatch()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetBatches);
            _mockBatchProcessingStepService.Setup(i => i.AddBatchStep(It.IsAny<int>(), It.IsAny<int>())).Returns(GetBatchProcessingStep());
            _mockWarehouseIngestUnitOfWork.Setup(i => i.Do(It.IsAny<Action>()));
            _mockWarehouseIngestUnitOfWork.Setup(i => i.Commit());

            //act
            var result = _batchProcessingService.UpdateBatch(1, 1);

            //assert
            Assert.AreEqual(BatchProcessingQueueSaveResult.Success, result);
        }

        [TestMethod]
        [Description("Test creating a batch and batch step. ")]
        public void Test_Create()
        {
            //arrange
            _mockBatchProcessingStepService.Setup(i => i.AddBatchStep(It.IsAny<int>(), It.IsAny<int>())).Returns(GetBatchProcessingStep());
            _mockBatchQueueDataService.Setup(i => i.Add(It.IsAny<BatchProcessingQueue>())).Returns(GetRetrievingBatch());
            _mockWarehouseIngestUnitOfWork.Setup(i => i.Do(It.IsAny<Action>()));
            _mockWarehouseIngestUnitOfWork.Setup(i => i.Commit());

            //act
            var batch = GetRetrievingBatch();
            var result = _batchProcessingService.Create(batch);

            //assert
            Assert.AreEqual(1, result.BatchProcessingQueueId);
        }

        #region Data
        
        private IQueryable<BatchProcessingQueue> GetBatches()
        {
            return new List<BatchProcessingQueue>()
            {
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 1,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                    BatchProcessingTypeId = 1,
                    RetryCount = 0,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 2,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                    BatchProcessingTypeId = 1,
                    RetryCount = 0,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
            }.AsQueryable();
        }
        
        private BatchProcessingQueue GetRetrievingBatch()
        {
            return new BatchProcessingQueue()
            {
                BatchName = "Test Batch 1",
                BatchPath = @"C:\temp\VideaTN\TestBatch\",
                BatchProcessingQueueId = 1,
                BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                BatchProcessingTypeId = 1,
                RetryCount = 0,
                CreatedDateUtc = DateTime.UtcNow,
                LastModifiedDateUtc = DateTime.UtcNow,
            };
        }

        private BatchProcessingStep GetBatchProcessingStep()
        {
            return new BatchProcessingStep()
            {
                BatchProcessingStepId = 1,
                BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                StepName = BatchProcessingStatus.Retrieving,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now.AddHours(1),
                BatchProcessingQueueId = 1,
                CompleteByTime = DateTime.Now.AddDays(2),
            };
        }
        
        #endregion
    }
}
