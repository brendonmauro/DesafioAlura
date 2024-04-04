using Npgsql;

namespace Persistence
{
    public class DbAccess
    {
        public NpgsqlConnection _connection;
        private readonly string _connectionString = "Host=localhost;Port=5432;Database=desafioalura;Username=postgres;Password=root;";

        public DbAccess()
        {
            _connection = new NpgsqlConnection(_connectionString);
        }
    }
}
