using System;
using RethinkDb.Driver;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace OnionArchitectureExample
{
    public class WatcherDataStore
    {
        public const string CrawlerTable = DataStoreConstants.CrawlerTable;
        public static RethinkDB R = RethinkDB.R;
        public static Db DB = R.Db(DataStoreConstants.DrivDefaultBucketName);

        public static string StatusWatchTable = DataStoreConstants.HealthStatusTable;

        public WatcherDataStore()
        {
            Connection = R.Connection()
                .Hostname("localhost")
                .Port(RethinkDBConstants.DefaultPort)
                .Timeout(60)
                .Connect();
        }

        public IConnection Connection { get; set; }

        //utfordring her er at 
        //for å ha ordentlig exception handlig dvs. ikke shut ned hele service pga koblingsproblemer med 
        // db, må vi implementere extra logikk
        // 
        public virtual Result<ServiceStatus> GetNumberOfChangedDocuments(string serviceName, int interval)
        {
            try
            {
                var result = DB.Table(CrawlerTable)
                        .Between(DateTime.UtcNow.AddMinutes(interval), R.Maxval()).OptArg("index", "updated")
                        .Filter(R.HashMap("type", serviceName))
                        .Count()
                        .RunAtom<int>(Connection);

                return Result.Ok(ServiceStatus.CreateServiceStatus(result));

            }
            catch (Exception ex)
            {
              return Result.Fail<ServiceStatus>(
                  ServiceStatus.CreateServiceStatus(StatusEnum.None),
                  $"Could not connect to RethinkDB. Failed with exception {ex}");

            }
        }

        public void UpdateStatusEntity(StatusEntity document)
        {
            var result = DB
                .Table(StatusWatchTable)
                .Insert(document).OptArg("conflict", "replace")
                .RunResult(Connection);

            if (result.Errors != 0)
            {
                throw new Exception($"Unable to save {document} to {StatusWatchTable}, {result}");
            }
        }


        public void CreateTableIfNotExists()
        {
            const string tableName = DataStoreConstants.HealthStatusTable;

            DB.TableList().Contains(tableName)
                .Do_(tableExist =>
                    R.Branch(tableExist,
                        new { tables_created = 0 },
                        DB.TableCreate(tableName)))
                .RunResult(Connection);
        }
    }

    //Fordel med å bruke result i dette tilfelle er for å kontrollere hvis det skjer noe med RethinkDb kobling
    // det er ikke nødvendig å kaste en exception, vi kan bare sette status til None som betyr i dette tilfelle at 
    // kan ikke koble til database
}