using System;
using Autofac;
using OnionArchitectureExample;
using Serilog;

namespace OnionArchitectureExample
{
    public class Program
    {
        public static ILogger Log { get; set; }

        private static void Main(string[] args)
        {
            #region Logging
            Log = new Logging().Create(Constants.WatcherService);
            #endregion
            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Log.Error($"{Constants.WatcherService} is failed with exception {ex} ");
            }
           
        }

        public static void Run()
        {
            #region Autofac

            var builder = new ContainerBuilder();
            //AutofacContainerBuilder.RegisterTypes(builder);
            builder.RegisterType<Application>().AsSelf().SingleInstance();
            builder.RegisterType<WatcherService>().AsSelf();
            //builder.RegisterType<WatcherDataStore>().As<IWatcherDataStore>();
            builder.RegisterType<WatcherDataStore>().AsSelf();
            builder.RegisterType<Logging>().AsSelf();
            builder.RegisterType<WatcherManager>().AsSelf();


            var container = builder.Build();

            #endregion
           

            #region Start Watcher

            using (var lifetimeScope = container.BeginLifetimeScope())
            {
                var watcherManager = lifetimeScope.Resolve<Application>();

                watcherManager.StartWatcher();
            }

            #endregion
        }
    }
}