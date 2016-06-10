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

        public virtual int GetStatus(string serviceName, int interval)
        {
            var result = DB.Table(CrawlerTable)
                .Between(DateTime.UtcNow.AddMinutes(interval), R.Maxval()).OptArg("index", "updated")
                .Filter(R.HashMap("type", serviceName))
                .Count()
                .RunAtom<int>(Connection);

            return result;
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

}