using System.Collections.Generic;
using System.Threading;

namespace OnionArchitectureExample
{
    public class Application
    {
        private readonly WatcherDataStore _watcherDataStore;
        private readonly WatcherService _watcherService;

        public Application(WatcherService watcherService,
            WatcherDataStore watcherDataStore)
        {
            _watcherService = watcherService;
            _watcherDataStore = watcherDataStore;
        }

        public void StartWatcher()
        {
            _watcherDataStore.CreateTableIfNotExists();

            var configs = new List<ConfigItem>
            {
                new ConfigItem(
                    Constants.TrainPlanService,
                    StatusEnum.None,
                    Constants.DefaultInterval,
                    true),
                new ConfigItem(
                    Constants.TrainSignalService,
                    StatusEnum.None,
                    Constants.DefaultInterval,
                    true)
            };

            while (true)
            {
                foreach (var item in configs)
                {
                    _watcherService.CheckStatus(item);
                }
                Thread.Sleep(Constants.MillisecondTimeout);
            }
        }
    }

    public class ConfigItem
    {
        public ConfigItem(string serviceName, StatusEnum status, int interval, bool firstTime)
        {
            ServiceName = serviceName;
            Status = status;
            Interval = interval;
            FirstTime = firstTime;
        }
        public string ServiceName { get; private set; }

        public StatusEnum Status { get; private set; }

        public int Interval { get; private set; }
        public bool FirstTime { get; private set; }

        public void ChangeStatus(StatusEnum serviceStatus)
        {
            Status = serviceStatus;
        }

        public void SetFirstTimeToFalse()
        {
            FirstTime = false;
        }
    }
}