using System;

namespace OnionArchitectureExample
{
    public class WatcherService
    {
        private readonly Logging _logging;
        private readonly WatcherDataStore _watcherDataStore;
        private readonly WatcherManager _watcherManager;

        public WatcherService(
            WatcherDataStore watcherDataStore, 
            Logging logging,
            WatcherManager watcherManager)
        {
            _watcherDataStore = watcherDataStore;
            _logging = logging;
            _watcherManager = watcherManager;
        }

      public void CheckStatus(ConfigItem item)
        {
            //husk hvis metode kan gi feil skal det returneres Result<T>
            Result<ServiceStatus> serviceStatus = 
                _watcherDataStore.GetNumberOfChangedDocuments(item.ServiceName, item.Interval);

            var statusAction = _watcherManager.CheckStatus(item, item.ServiceName, serviceStatus);

            ApplyChanges(statusAction, item.Interval);
        }

        private void ApplyChanges(StatusAction action, int interval)
        {
            switch (action.Type)
            {
                case ActionType.Update:
                    _logging.WriteToLog(action.Result, action.ServiceName, interval);
                    _watcherDataStore.UpdateStatusEntity(
                        StatusEntity.Create(action.ServiceName, DateTime.UtcNow, action.Result.Value.Status,
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