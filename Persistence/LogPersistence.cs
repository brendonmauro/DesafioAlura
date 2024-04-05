using Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    /// <summary>
    /// Classe resposnável por fazer a persistencia dos logs
    /// </summary>
    public class LogPersistence : DbAccess
    {

        public LogPersistence() : base() { }

        public void Insert(Log log)
        {
            try
            {
                string insertQuery = "INSERT INTO log (text, date) VALUES (@text, @date);";
                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, _connection))
                {
                    _connection.Open();
                    command.Parameters.AddWithValue("@text", log.Text.Substring(0, log.Text.Length < 995 ? log.Text.Length : 995));
                    command.Parameters.AddWithValue("@date", log.Date);
                    command.ExecuteNonQuery();
                    _connection.Close();
                }
            } catch(Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
