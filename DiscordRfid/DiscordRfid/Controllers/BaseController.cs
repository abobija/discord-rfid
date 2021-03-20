using Microsoft.Data.Sqlite;

namespace DiscordRfid.Controllers
{
    public abstract class BaseController
    {
        protected SqliteConnection Connection;

        public BaseController(SqliteConnection connection)
        {
            Connection = connection;
        }
    }
}
