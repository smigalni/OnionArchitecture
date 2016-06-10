using System;
using System.Collections.Generic;

namespace OnionArchitectureExample
{
    public class WatcherManager
    {
        public StatusAction CheckStatus(ConfigItem item, StatusEnum serviceStatus)
        {
            if (serviceStatus == item.Status)
            {
                return new StatusAction(item.ServiceName, serviceStatus, ActionType.DoNothing);
            }

            item.ChangeStatus(serviceStatus);

            return new StatusAction(item.ServiceName, serviceStatus, ActionType.Update);
        }
    }

    public struct StatusAction
    {
        public readonly string ServiceName;
        public readonly StatusEnum ServiceStatus;
        public readonly ActionType Type;

        public StatusAction(string serviceName, StatusEnum serviceStatus, ActionType type)
        {
            ServiceName = serviceName;
            ServiceStatus = serviceStatus;
            Type = type;
        }
    }

   

}