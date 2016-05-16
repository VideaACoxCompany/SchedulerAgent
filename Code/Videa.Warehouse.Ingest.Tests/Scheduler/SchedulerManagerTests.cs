using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Scheduler.Managers;
using Videa.Warehouse.Ingest.Scheduler.Services;

namespace Videa.Warehouse.Ingest.Tests.Scheduler
{
    [TestClass]
    public class SchedulerManagerTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IRetrieveStartService> _mockRetrieveStartService;
        private Mock<IValidateStartService> _mockValidateStartService;
        private Mock<IPrestageStartService> _mockPrestageStartService;
        private Mock<IStageStartService> _mockStageStartService;
        private Mock<IVaultStartService> _mockVaultStartService;
        private Mock<IPurgeStartService> _mockPurgeStartService;
        private Mock<IConfigManager> _mockConfigManager;
        private Mock<IBatchProcessingQueueService> _mockBatchQueueService;

        private SchedulerManager _schedulerManager;

        [TestInitialize]
        public void Setting()
        {
            _mockLogger = new Mock<ILogger>();
            _mockRetrieveStartService = new Mock<IRetrieveStartService>();
            _mockValidateStartService = new Mock<IValidateStartService>();
            _mockPrestageStartService = new Mock<IPrestageStartService>();
            _mockStageStartService = new Mock<IStageStartService>();
            _mockVaultStartService = new Mock<IVaultStartService>();
            _mockPurgeStartService = new Mock<IPurgeStartService>();
            _mockConfigManager = new Mock<IConfigManager>();

            _mockBatchQueueService = new Mock<IBatchProcessingQueueService>();

            _schedulerManager = new SchedulerManager(_mockLogger.Object,
                _mockRetrieveStartService.Object,
                _mockValidateStartService.Object,
                _mockPrestageStartService.Object,
                _mockStageStartService.Object,
                _mockVaultStartService.Object,
                _mockPurgeStartService.Object,
                _mockBatchQueueService.Object,
                _mockConfigManager.Object);
        }

        [TestMethod]
        [Description("Test SchedulerManger StartProcess() method. ")]
        public void Test_StartProcess()
        {
            //arrange
            
            _mockBatchQueueService.Setup(i => i.GetPrestagedBatches(1)).Returns(GetPrestagedBatches);
            _mockConfigManager.Setup(i => i.StagingLimit).Returns(1);
            try
            {
                //act
                _schedulerManager.StartProcess();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test SchedulerManger StartProcess() method handling exceptions. ")]
        public void Test_StartProcess_Exception()
        {
            //arrange
            _mockRetrieveStartService.Setup(i => i.Execute()).Throws(new Exception());

            try
            {
                //act
                _schedulerManager.StartProcess();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test SchedulerManger StartProcess() method when there are staged batches. ")]
        public void Test_StartProcess_GetStagedPath()
        {
            //arrange

            _mockBatchQueueService.Setup(i => i.GetStagedBatch()).Returns(GetStagedBatch);
            try
            {
                //act
                _schedulerManager.StartProcess();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        [Description("Test SchedulerManger StartProcess() method when there are vaulted batches. ")]
        public void Test_StartProcess_GetVaultedPath()
        {
            //arrange

            _mockBatchQueueService.Setup(i => i.GetVaultedBatch(It.IsAny<int>())).Returns(GetVaultedBatch);
            _mockConfigManager.Setup(i => i.PurgeLimitInDays).Returns(7);

            try
            {
                //act
                _schedulerManager.StartProcess();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }




        #region Mock Data


        private List<BatchProcessingQueue> GetPrestagedBatches()
        {
            return new List<BatchProcessingQueue>()
            {
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 30",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 30,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaged,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                }

            };
        }


        private BatchProcessingQueue GetStagedBatch()
        {
            return new BatchProcessingQueue()
            {
                BatchName = "Test Batch 30",
                BatchPath = @"C:\temp\VideaTN\TestBatch\",
                BatchProcessingQueueId = 30,
                BatchProcessingStatusId = (int) BatchProcessStatus.Staged,
                BatchProcessingTypeId = 1,
                CreatedDateUtc = DateTime.UtcNow,
                LastModifiedDateUtc = DateTime.UtcNow,
            };
        }

        private BatchProcessingQueue GetVaultedBatch()
        {
            return new List<BatchProcessingQueue>()
            {
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 30",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 30,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Vaulted,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                },
                new BatchProcessingQueue()
                {
                    BatchName = "Test Batch 30",
                    BatchPath = @"C:\temp\VideaTN\TestBatch\",
                    BatchProcessingQueueId = 40,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Vaulted,
                    BatchProcessingTypeId = 1,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastModifiedDateUtc = DateTime.UtcNow,
                }

            }.First();
        }
        #endregion
    }
}
