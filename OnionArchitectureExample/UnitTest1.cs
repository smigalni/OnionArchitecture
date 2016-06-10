using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace OnionArchitectureExample
{
    [TestFixture]
    public class UnitTest1
    {
        private List<ConfigItem> SetUpConfigItem()
        {
            var configs = new List<ConfigItem>
            {
                new ConfigItem(
                    Constants.TrainPlanService,
                    StatusEnum.None,
                    Constants.DefaultInterval),
                new ConfigItem(
                    Constants.TrainSignalService,
                    StatusEnum.None,
                    Constants.DefaultInterval
                    )
            };
            return configs;
        }

        [Test]
        public void Test_StatusBad()
        {
            var mock = new Mock<WatcherDataStore>();
            //GetStaus has to be virtuall for this to work 
            mock.Setup(obj => obj.GetStatus(It.IsAny<string>(), It.IsAny<int>())).Returns(5);

            var watcherService = new WatcherService(mock.Object, null, null);
            var result = watcherService.GetServiceStatus("adsf", 5);

            result.ShouldBe(StatusEnum.Good);
        }

        [Test]
        public void Test_StatusGod()
        {
            var mock = new Mock<WatcherDataStore>();
            mock.Setup(obj => obj.GetStatus(It.IsAny<string>(), It.IsAny<int>())).Returns(0);

            var watcherService = new WatcherService(mock.Object, null, null);
            var result = watcherService.GetServiceStatus("adsf", 5);

            result.ShouldBe(StatusEnum.Bad);
        }


        [Test]
        public void TestMethod_StatusEnum_Good()
        {
            var items = SetUpConfigItem();

            var watcherManager = new WatcherManager();
            var resultat = watcherManager.CheckStatus(items.First(), StatusEnum.Good);

            Assert.AreEqual(items.First().Status, StatusEnum.Good);
            Assert.AreNotEqual(items.First().Status, StatusEnum.None);
        }


        [Test]
        public void TestMethod1_StatusEnum_Bad()
        {
            var items = SetUpConfigItem();

            var watcherManager = new WatcherManager();
            var resultat = watcherManager.CheckStatus(items.First(), StatusEnum.Bad);

            Assert.AreEqual(items.First().Status, StatusEnum.Bad);
            Assert.AreNotEqual(items.First().Status, StatusEnum.None);
        }

    }
}