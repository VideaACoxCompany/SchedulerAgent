using System;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;
using Videa.Warehouse.Ingest.Scheduler.Handlers;

namespace Videa.Warehouse.Ingest.Tests.Scheduler
{
    [TestClass]
    public class HandlerTests
    {
        private Mock<ILogger> _mockLogger;
        private Mock<IBatchProcessingService> _mockBatchProcessingService;

        [TestInitialize]
        public void Setting()
        {
            _mockLogger = new Mock<ILogger>();
            _mockBatchProcessingService = new Mock<IBatchProcessingService>();
        }


        //retrive
        [TestMethod]
        [Description("Test RetrieveEndHanlder. ")]
        public void Test_RetrieveEndHandler()
        {
            //arrange
            var retrieveEndHandler = new RetrieveEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                retrieveEndHandler.Consume(new RetrieveEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test RetrieveEndHanlder for handling exceptions. ")]
        public void Test_RetrieveEndHandler_Exception()
        {
            //arrange
            var retrieveEndHandler = new RetrieveEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                retrieveEndHandler.Consume(new RetrieveEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        //validate
        [TestMethod]
        [Description("Test ValidateEndHandler. ")]
        public void Test_ValidateEndHandler()
        {
            //arrange
            var validateEndHandler = new ValidateEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                validateEndHandler.Consume(new ValidateEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Description("Test ValidateEndHandler for handling exceptions. ")]
        public void Test_ValidateEndHandler_Exception()
        {
            //arrange
            var validateEndHandler = new ValidateEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                validateEndHandler.Consume(new ValidateEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        //prestage
        [TestMethod]
        [Description("Test PrestageEndHandler. ")]
        public void Test_PrestageEndHandler()
        {
            //arrange
            var prestageEndHandler = new PrestageEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                prestageEndHandler.Consume(new PrestageEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }


        }

        [TestMethod]
        [Description("Test PrestageEndHandler for handling exceptions. ")]
        public void Test_PrestageEndHandler_Exception()
        {
            //arrange
            var prestageEndHandler = new PrestageEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                prestageEndHandler.Consume(new PrestageEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }



        //stage
        [TestMethod]
        [Description("Test StageEndHandler. ")]
        public void Test_StageEndHandler()
        {
            //arrange
            var stageEndHandler = new StageEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                stageEndHandler.Consume(new StageEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }


        }

        [TestMethod]
        [Description("Test StageEndHandler for handling exceptions. ")]
        public void Test_StageEndHandler_Exception()
        {
            //arrange
            var stageEndHandler = new StageEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                stageEndHandler.Consume(new StageEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        //vault
        [TestMethod]
        [Description("Test VaultEndHandler. ")]
        public void Test_VaultEndHandler()
        {
            //arrange
            var vaultEndHandler = new VaultEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                vaultEndHandler.Consume(new VaultEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }


        }

        [TestMethod]
        [Description("Test VaultEndHandler for handling exceptions. ")]
        public void Test_VaultEndHandler_Exception()
        {
            //arrange
            var vaultEndHandler = new VaultEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                vaultEndHandler.Consume(new VaultEnd() {BatchId = 1, Status = new MessageStatus() {IsSuccess = true}});
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }

        //purge

        [TestMethod]
        [Description("Test PurgeEndHandler. ")]
        public void Test_PurgeEndHandler()
        {
            //arrange
            var purgeEndHandler = new PurgeEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Returns(BatchProcessingQueueSaveResult.Error);

            try
            {
                //act
                purgeEndHandler.Consume(new PurgeEnd() { BatchId = 1, Status = new MessageStatus() { IsSuccess = true } });
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }


        }

        [TestMethod]
        [Description("Test PurgeEndHandler for exceptions. ")]
        public void Test_PurgeEndHandler_Exception()
        {
            //arrange
            var purgeEndHandler = new PurgeEndHandler(_mockLogger.Object, _mockBatchProcessingService.Object);
            _mockBatchProcessingService.Setup(i => i.UpdateBatch(It.IsAny<int>(), It.IsAny<int>(), true, true)).Throws(new DbUpdateException());

            try
            {
                //act
                purgeEndHandler.Consume(new PurgeEnd() { BatchId = 1, Status = new MessageStatus() { IsSuccess = true } });
            }
            catch (Exception ex)
            {
                //assert
                Assert.Fail(ex.Message);
            }
        }
    }
}
