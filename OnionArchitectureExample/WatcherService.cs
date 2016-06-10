using System;

namespace OnionArchitectureExample
{
    public class WatcherService
    {
        private readonly Logging _logging;
        private readonly WatcherDataStore _watcherDataStore;
        private readonly WatcherManager _watcherManager;

        public WatcherService(WatcherDataStore watcherDataStore, Logging logging,
            WatcherManager watcherManager)
        {
            _watcherDataStore = watcherDataStore;
            _logging = logging;
            _watcherManager = watcherManager;
        }

        public StatusEnum GetServiceStatus(string serviceName, int interval)
        {
            var status = _watcherDataStore.GetStatus(serviceName, interval) > 0 ? StatusEnum.Good : StatusEnum.Bad;
            return status;
        }

        public void CheckStatus(ConfigItem item)
        {
            //var serviceStatus = GetServiceStatus(item.ServiceName, item.Interval);
            var serviceStatus = StatusEnum.Bad;
            var statusAction = _watcherManager.CheckStatus(item, serviceStatus);
            ApplyChanges(statusAction, item.Interval);
        }

        private void ApplyChanges(StatusAction action, int interval)
        {
            switch (action.Type)
            {
                case ActionType.Update:
                    _logging.WriteToLog(action.ServiceStatus, action.ServiceName, interval);
                    _watcherDataStore.UpdateStatusEntity(
                        StatusEntity.Create(action.ServiceName, DateTime.UtcNow, action.ServiceStatus,
                            action.ServiceName));
                    break;

                case ActionType.DoNothing:
                    break;

                default:
                    throw new InvalidOperationException("Action type is undefined");
            }
        }
    }

  
}