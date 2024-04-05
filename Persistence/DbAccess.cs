using Npgsql;

namespace Persistence
{
    /// <summary>
    /// Classe responsável por fazer a conexão com o banco de dados
    /// </summary>
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
