using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Tests.Services
{
    [TestClass]
    public class BatchProcessingQueueServiceTests
    {
        private Mock<IWarehouseIngestDataService<BatchProcessingQueue>> _mockBatchQueueDataService;

        private BatchProcessingQueueService _batchProcessingQueueService;

        [TestInitialize]
        public void Setting()
        {
            _mockBatchQueueDataService = new Mock<IWarehouseIngestDataService<BatchProcessingQueue>>();

            _batchProcessingQueueService = new BatchProcessingQueueService(_mockBatchQueueDataService.Object);
        }

        [TestMethod]
        [Description("Get total BatchProcessingQueue batch count")]
        public void Test_GetBatchCount()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.Count()).Returns(2);
            
            //act
            var count = _batchProcessingQueueService.GetBatchCount();
            
            //assert
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [Description("Test getting BatchProcessingQueue batches")]
        public void Test_GetBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetRetrievingBatches());


            //act
            var batch = _batchProcessingQueueService.GetBatches(i => i.BatchProcessingQueueId == 1).ToList();
            var batchRetrieving = _batchProcessingQueueService.GetBatches(i => i.BatchProcessingStatusId == (int) BatchProcessStatus.Retrieving).ToList();

            //assert
            Assert.AreEqual(1, batch.Count());
            Assert.AreEqual(2, batchRetrieving.Count());
        }

        [TestMethod]
        [Description("Test getting batches in 'Retrieved' status")]
        public void Test_GetRetrievedBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetRetrievedValidatedBatches());

            //act
            var batches = _batchProcessingQueueService.GetRetrievedBatches().ToList();

            //assert
            Assert.AreEqual(2, batches.Count());
        }

        [TestMethod]
        [Description("Test getting batches in 'validated' status")]
        public void Test_GetValidatedBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetRetrievedValidatedBatches());

            //act
            var batches = _batchProcessingQueueService.GetValidatedBatches().ToList();

            //assert
            Assert.AreEqual(2, batches.Count());
        }

        [TestMethod]
        [Description("Test getting batches in 'prestaged' status")]
        public void Test_GetPrestagedBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetStagingPurgingVaultingBatches());

            //act
            var stagingLimit1Batch = _batchProcessingQueueService.GetPrestagedBatches(1).ToList();
            var stagingLimit2Batch = _batchProcessingQueueService.GetPrestagedBatches(2).ToList();

            //assert
            Assert.AreEqual(0, stagingLimit1Batch.Count());
            Assert.AreEqual(1, stagingLimit2Batch.Count());
        }


        [TestMethod]
        [Description("Test getting batches in 'staged' status")]
        public void Test_GetStagedBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetStagedVaultedBatches());

            //act
            var batch = _batchProcessingQueueService.GetStagedBatch();

            //assert
            Assert.IsNotNull(batch);
            Assert.AreEqual(100, batch.BatchProcessingQueueId);
            Assert.AreEqual(8, batch.BatchProcessingStatusId);
        }

        [TestMethod]
        [Description("Test getting batches in 'vaulted' status")]
        public void Test_GetVaultedBatches()
        {
            //arrange
            _mockBatchQueueDataService.Setup(i => i.QueryReadonly()).Returns(GetStagedVaultedBatches());

            //act
            var batch = _batchProcessingQueueService.GetVaultedBatch(7);

            //assert
            Assert.IsNotNull(batch);
            Assert.AreEqual(103, batch.BatchProcessingQueueId);
            Assert.AreEqual((int) BatchProcessStatus.Vaulted, batch.BatchProcessingStatusId);
        }


        #region Mock Data

        private IQueryable<BatchProcessingQueue> GetRetrievingBatches()
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

        private IQueryable<BatchProcessingQueue> GetStagingPurgingVaultingBatches()
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
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 2,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieved,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 3,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Validating,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 4,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Validated,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 5,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaging,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 6,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 7,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Staging,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 8,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Staged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 1,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Vaulted,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 1,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Purged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                }
            }.AsQueryable();
        }

        private IQueryable<BatchProcessingQueue> GetStagedVaultedBatches()
        {
            return new List<BatchProcessingQueue>()
            {
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 100,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Staged,
                    BatchProcessingTypeId = 1,
                    RetryCount = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 101,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Staged,
                    BatchProcessingTypeId = 1,
                    RetryCount = 2,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 102,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Vaulted,
                    BatchProcessingTypeId = 1,
                    RetryCount = 0,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 1",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 103,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Vaulted,
                    BatchProcessingTypeId = 1,
                    RetryCount = 0,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow.AddDays(-10),
                }
            }.AsQueryable();
        }


        private IQueryable<BatchProcessingQueue> GetRetrievedValidatedBatches()
        {
            return new List<BatchProcessingQueue>()
            {
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 10",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 10,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieved,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 20",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 20,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieved,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 30",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 30,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Validated,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 40",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 40,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Validated,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 50",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 50,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 60",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 60,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },

            }.AsQueryable();
        }
        

        #endregion
    }
}
