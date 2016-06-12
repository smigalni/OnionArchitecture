namespace OnionArchitectureExample
{
    public class WatcherManager
    {
        public StatusAction CheckStatus(ConfigItem item, string serviceName, Result<ServiceStatus> serviceStatus)
        {
            var status = serviceStatus.Value.Status;
            //hvis status er none (fikke ikke kontakt med database) og det er første gang da må vi ha mere logikk
            if (item.FirstTime )
            {
                item.SetFirstTimeToFalse();
                return new StatusAction(serviceStatus, serviceName, ActionType.Update);
            }

            if (status == item.Status)
            {
                return new StatusAction(serviceStatus, serviceName, ActionType.DoNothing);
            }

            item.ChangeStatus(status);

            return new StatusAction(serviceStatus, serviceName, ActionType.Update);

        }
    }

    public struct StatusAction
    {

        public StatusAction(Result<ServiceStatus> result, string serviceName, ActionType type)
        {
            ServiceName = serviceName;
            Result = result;
            Type = type;
        }

        public ActionType Type { get; set; }
        public Result<ServiceStatus> Result { get; set; }
        public string ServiceName { get; set; }
    }

   

}