using System;

namespace OnionArchitectureExample
{
    public class ServiceStatus
    {
        private StatusEnum none;

        public ServiceStatus(int result)
        {
            NumberOfChangedDocuments = result;
            Status = GetStatus(NumberOfChangedDocuments);
        }

        public ServiceStatus(int numberOfChangedDocuments, StatusEnum status)
        {
            NumberOfChangedDocuments = numberOfChangedDocuments;
            Status = status;
        }

        public int NumberOfChangedDocuments { get; }

        public StatusEnum Status { get; private set; }

        private StatusEnum GetStatus(int numberOfChangedDocuments)
        {
            return numberOfChangedDocuments > 0 ? StatusEnum.Good : StatusEnum.Bad;
        }

        public static ServiceStatus CreateServiceStatus(int result)
        {
            return new ServiceStatus(result);
        }


        public static ServiceStatus CreateServiceStatus(StatusEnum status)
        {
            return new ServiceStatus(0, status);
        }
    }
}