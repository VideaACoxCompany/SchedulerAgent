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
    public class BatchProcessingStepServiceTests
    {
        private Mock<IWarehouseIngestDataService<BatchProcessingStep>> _mockStepDataService;
        private BatchProcessingStepService _stepService;

        [TestInitialize]
        public void Setting()
        {
            _mockStepDataService = new Mock<IWarehouseIngestDataService<BatchProcessingStep>>();
            _stepService = new BatchProcessingStepService(_mockStepDataService.Object);
        }

        [TestMethod]
        [Description("Test adding a batch step. ")]
        public void Test_AddBatchStep()
        {
            //arrange
            _mockStepDataService.Setup(i => i.Add(It.IsAny<BatchProcessingStep>())).Returns(GetBatchProcessingStep());

            //act
            var result = _stepService.AddBatchStep(1, (int) BatchProcessStatus.Vaulting);

            //assert
            Assert.AreEqual(1, result.BatchProcessingQueueId);
            Assert.AreEqual(9, result.BatchProcessingStatusId);
        }

        [TestMethod]
        [Description("Test updating an existing step")]
        public void Test_UpdateStep()
        {
            //arrange
            _mockStepDataService.Setup(i => i.Query()).Returns(GetBatchProcessingSteps());

            //act
            var result = _stepService.UpdateStep(1);

            //assert
            Assert.IsNotNull(result.First().EndDateTime);
        }

        #region Mock Data

        private BatchProcessingStep GetBatchProcessingStep()
        {
            return new BatchProcessingStep()
            {
                BatchProcessingStepId = 1,
                BatchProcessingStatusId = (int)BatchProcessStatus.Retrieving,
                StepName = BatchProcessingStatus.Retrieving,
                StartDateTime = DateTime.Now,
                BatchProcessingQueueId = 1,
                CompleteByTime = DateTime.Now.AddDays(2),
            };
        }

        private IQueryable<BatchProcessingStep> GetBatchProcessingSteps()
        {
            return new List<BatchProcessingStep>()
            {
                new BatchProcessingStep()
                {
                    BatchProcessingStepId = 1,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                    StepName = BatchProcessingStatus.Retrieving,
                    StartDateTime = DateTime.Now,
                    BatchProcessingQueueId = 1,
                    CompleteByTime = DateTime.Now.AddDays(2),
                },
                new BatchProcessingStep()
                {
                    BatchProcessingStepId = 2,
                    BatchProcessingStatusId = (int) BatchProcessStatus.Prestaging,
                    StepName = BatchProcessingStatus.Retrieving,
                    StartDateTime = DateTime.Now,
                    BatchProcessingQueueId = 1,
                    CompleteByTime = DateTime.Now.AddDays(2),
                }
            }.AsQueryable();
        }
        #endregion
    }
}
