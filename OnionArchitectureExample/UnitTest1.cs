using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace OnionArchitectureExample
{
    [TestFixture]
    public class UnitTest1
    {
        private List<ConfigItem> SetUpConfigItem(bool firstTime)
        {
            var configs = new List<ConfigItem>
            {
                new ConfigItem(
                    Constants.TrainPlanService,
                    StatusEnum.None,
                    Constants.DefaultInterval,
                    firstTime),
                new ConfigItem(
                    Constants.TrainSignalService,
                    StatusEnum.None,
                    Constants.DefaultInterval,
                    firstTime
                    )
            };
            return configs;
        }


        //[Test]
        //public void Test_StatusBad()
        //{
        //    var mock = new Mock<WatcherDataStore>();
        //    //GetStaus has to be virtuall for this to work 
        //    mock.Setup(obj => obj.GetNumberOfChangedDocuments(It.IsAny<string>(), It.IsAny<int>())).Returns(5);

        //    var watcherService = new WatcherService(mock.Object, null, null);
        //    var result = watcherService.GetServiceStatus("adsf", 5);

        //    result.ShouldBe(StatusEnum.Good);
        //}

        //[Test]
        //public void Test_StatusGod()
        //{
        //    var mock = new Mock<WatcherDataStore>();
        //    mock.Setup(obj => obj.GetNumberOfChangedDocuments(It.IsAny<string>(), It.IsAny<int>())).Returns(0);

        //    var watcherService = new WatcherService(mock.Object, null, null);
        //    var result = watcherService.GetServiceStatus("adsf", 5);

        //    result.ShouldBe(StatusEnum.Bad);
        //}


        [Test]
        public void TestMethod_StatusEnum_FirstRun()
        {
            var items = SetUpConfigItem(true);
            var result = Result.Ok(ServiceStatus.CreateServiceStatus(5));
            var watcherManager = new WatcherManager();
            watcherManager.CheckStatus(items.First(), items.First().ServiceName, result);

            Assert.AreEqual(items.First().Status, StatusEnum.None);
            items.First().FirstTime.ShouldBe(false);
        }

        [Test]
        public void TestMethod_StatusEnum_SecondRun_StatusGood()
        {
            var items = SetUpConfigItem(false);
            var result = Result.Ok(ServiceStatus.CreateServiceStatus(5));
            var watcherManager = new WatcherManager();
            watcherManager.CheckStatus(items.First(), items.First().ServiceName, result);

            Assert.AreEqual(items.First().Status, StatusEnum.Good);
            items.First().FirstTime.ShouldBe(false);
        }

        [Test]
        public void TestMethod_StatusEnum_SecondRun_StatusBad()
        {
            var items = SetUpConfigItem(false);
            var result = Result.Ok(ServiceStatus.CreateServiceStatus(0));
            var watcherManager = new WatcherManager();
            watcherManager.CheckStatus(items.First(), items.First().ServiceName, result);

            items.First().Status.ShouldBe(StatusEnum.Bad);
            items.First().FirstTime.ShouldBe(false);
        }

        [Test]
        public void TestMethod_StatusEnum_SecondRun_StatusNone()
        {
            var items = SetUpConfigItem(false);
            var result = Result.Fail<ServiceStatus>(
                  ServiceStatus.CreateServiceStatus(StatusEnum.None),
                  "Could not connect to RethinkDB. Failed with exception");
            var watcherManager = new WatcherManager();
            watcherManager.CheckStatus(items.First(), items.First().ServiceName, result);

            items.First().Status.ShouldBe(StatusEnum.None);
            items.First().FirstTime.ShouldBe(false);
        }

        //[Test]
        //public void TestMethod1_StatusEnum_Bad()
        //{
        //    var items = SetUpConfigItem();

        //    var watcherManager = new WatcherManager();
        //    watcherManager.CheckStatus(items.First(), StatusEnum.Bad);

        //    Assert.AreEqual(items.First().Status, StatusEnum.Bad);
        //    Assert.AreNotEqual(items.First().Status, StatusEnum.None);
        //}

    }
}