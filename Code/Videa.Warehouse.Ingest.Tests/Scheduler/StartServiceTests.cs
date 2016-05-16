using System;
using System.Collections.Generic;
using System.Linq;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;
using Videa.Warehouse.Ingest.Scheduler.Services;

namespace Videa.Warehouse.Ingest.Tests.Scheduler
{
    [TestClass]
    public class StartServiceTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IServiceBus> _mockServiceBus;
        private Mock<IBatchProcessingService> _mockBatchProcessingService;
        private Mock<IBatchProcessingQueueService> _mockBatchProcessingQueueService;

        [TestInitialize]
        public void Setting()
        {
            _mockLogger = new Mock<ILogger>();
            _mockServiceBus = new Mock<IServiceBus>();
            _mockBatchProcessingService = new Mock<IBatchProcessingService>();
            _mockBatchProcessingQueueService = new Mock<IBatchProcessingQueueService>();
        }


        #region retrieve

        [TestMethod]
        [Description("Test RetrieveStartService. ")]
        public void Test_RetrieveStartService()
        {
            //arrange
            var retrieveStartService = new RetrieveStartService(_mockLogger.Object, _mockServiceBus.Object, 
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);
            
            try
            {
                //act
                retrieveStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test RetrieveStartService when IsTestingMode is on. ")]
        public void Test_RetrieveStartService_TestingMode()
        {
            //arrange
            var retrieveStartService = new RetrieveStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            System.Configuration.ConfigurationManager.AppSettings["IsTestingMode"] = "true";
            
            try
            {
                //act
                retrieveStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [Description("Test RetrieveStartService for handling exceptions. ")]
        [TestMethod]
        public void Test_RetrieveStartService_Exception()
        {
            //arrange
            var retrieveStartService = new RetrieveStartService(_mockLogger.Object, _mockServiceBus.Object, 
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetValidatedBatches()).Returns(GetValidatedBatches);
            _mockServiceBus.Setup(i => i.Publish(It.IsAny<RetrieveStart>())).Throws(new Exception());
            try
            {
                //act
                retrieveStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region validate

        [Description("Test ValidateStartService. ")]
        [TestMethod]
        public void Test_ValidateStartService()
        {
            //arrange
            var validateStartService = new ValidateStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetRetrievedBatches()).Returns(GetRetrievedBatches);

            try
            {
                //act
                validateStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test ValidateStartService when there's no batch present. ")]
        public void Test_ValidateStartService_NoBatch()
        {
            //arrange
            var validateStartService = new ValidateStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            try
            {
                //act
                validateStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test ValidateStartService for handling exceptions. ")]
        public void Test_ValidateStartService_Exception()
        {
            //arrange
            var validateStartService = new ValidateStartService(_mockLogger.Object, _mockServiceBus.Object,
              _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetRetrievedBatches()).Returns(GetRetrievedBatches);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception());

            try
            {
                //act
                validateStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }
        #endregion

        #region Prestage

        [TestMethod]
        [Description("Test PrestageStartService. ")]
        public void Test_PrestageStartService()
        {
            //arrange
            var prestageStartService = new PrestageStartService(_mockLogger.Object, _mockServiceBus.Object, 
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetValidatedBatches()).Returns(GetValidatedBatches);

            try
            {
                //act
                prestageStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test PrestageStartService when there's no batch. ")]
        public void Test_PrestageStartService_NoBatch()
        {
            //arrange
            var prestageStartService = new PrestageStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(BatchProcessingQueueSaveResult.Success);

            try
            {
                //act
                prestageStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test PrestageStartService for handling exceptions. ")]
        public void Test_PrestageStartService_Exception()
        {
            //arrange
            var prestageStartService = new PrestageStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingQueueService.Object, _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetValidatedBatches()).Returns(GetValidatedBatches);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception());
            try
            {
                //act
                prestageStartService.Execute();
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }


        #endregion

        #region stage

        [TestMethod]
        [Description("Test StageStartService. ")]
        public void Test_StageStartService()
        {
            //arrange
            var stageStartService = new StageStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(BatchProcessingQueueSaveResult.Success);

            try
            {
                //act
                stageStartService.Execute(GetPrestagedBatches());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        [Description("Test StageStartService for handling exceptions. ")]
        public void Test_StageStartService_Exception()
        {
            //arrange
            var stageStartService = new StageStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetValidatedBatches()).Returns(GetValidatedBatches);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception());
            try
            {
                //act
                stageStartService.Execute(GetPrestagedBatches());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        //vault
        [TestMethod]
        [Description("Test VaultStartService. ")]
        public void Test_VaultStartService()
        {
            //arrange
            var vaultStartService = new VaultStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(BatchProcessingQueueSaveResult.Success);

            try
            {
                //act
                vaultStartService.Execute(GetStagedBatch());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        [Description("Test VaultStartService for handling exceptions. ")]
        public void Test_VaultStartService_Exception()
        {
            //arrange
            var vaultStartService = new VaultStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetValidatedBatches()).Returns(GetValidatedBatches);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception());
            try
            {
                //act
                vaultStartService.Execute(GetStagedBatch());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        #endregion


        #region purge

        [TestMethod]
        [Description("Test PurgeStartService. ")]
        public void Test_PurgeStartService()
        {
            //arrange
            var purgeStartService = new PurgeStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(BatchProcessingQueueSaveResult.Success);

            try
            {
                //act
                purgeStartService.Execute(GetVaultedBatch());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }


        [TestMethod]
        [Description("Test PurgeStartService for handling exceptions. ")]
        public void Test_PurgeStartService_Exception()
        {
            //arrange
            var purgeStartService = new PurgeStartService(_mockLogger.Object, _mockServiceBus.Object,
                _mockBatchProcessingService.Object);

            _mockBatchProcessingQueueService.Setup(i => i.GetVaultedBatch(7)).Returns(GetVaultedBatch);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Throws(new Exception());
            try
            {
                //act
                purgeStartService.Execute(GetVaultedBatch());
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        #endregion

        #region Mock Data

        private IQueryable<BatchProcessingQueue> GetRetrievedBatches()
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
                }

            }.AsQueryable();
        }

        private IQueryable<BatchProcessingQueue> GetValidatedBatches()
        {
            return new List<BatchProcessingQueue>()
            {
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
                }

            }.AsQueryable();
        }

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
