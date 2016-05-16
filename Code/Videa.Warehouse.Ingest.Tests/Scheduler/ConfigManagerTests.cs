
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Videa.Warehouse.Ingest.Scheduler.Managers;

namespace Videa.Warehouse.Ingest.Tests.Scheduler
{
    [TestClass]
    public class ConfigManagerTests
    {
        [TestMethod]
        [Description("Test PurgeLimitInDays default value. ")]
        public void Test_PurgeLimitInDays()
        {
            var configManager = new ConfigManager();
            var limit = configManager.PurgeLimitInDays;
            Assert.AreEqual(7, limit);
        }

        [TestMethod]
        [Description("Test StagingLimit default value. ")]
        public void Test_StagingLimit()
        {
            var configManager = new ConfigManager();
            var limit = configManager.StagingLimit;
            Assert.AreEqual(1, limit);
        }

        //set config

        [TestMethod]
        [Description("Test PurgeLimitInDays from config setting. ")]
        public void Test_PurgeLimitInDays_SetConfig()
        {
            ConfigurationManager.AppSettings["PurgeLimitInDays"] = "5";
            var configManager = new ConfigManager();
            var limit = configManager.PurgeLimitInDays;
            Assert.AreEqual(5, limit);
        }

        [TestMethod]
        [Description("Test StagingLimit from config setting. ")]
        public void Test_StagingLimit_SetConfig()
        {
            ConfigurationManager.AppSettings["StagingLimit"] = "2";
            var configManager = new ConfigManager();
            var limit = configManager.StagingLimit;
            Assert.AreEqual(2, limit);
        }
    }
}
