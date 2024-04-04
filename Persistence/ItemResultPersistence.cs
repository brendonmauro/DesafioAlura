using Domain;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ItemResultPersistence : DbAccess
    {
        public void Insert(ItemResult itemResult)
        {

            string insertQuery = "INSERT INTO item_result (titulo, professor, carga_horaria, descricao, date) VALUES (@titulo, @professor, @carga_horaria, @descricao, @date);";
            using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, this._connection))
            {
                _connection.Open();
                command.Parameters.AddWithValue("@titulo", itemResult.Titulo);
                command.Parameters.AddWithValue("@professor", itemResult.Titulo);
                command.Parameters.AddWithValue("@carga_horaria", itemResult.CargaHoraria);
                command.Parameters.AddWithValue("@descricao", itemResult.Descricao);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
