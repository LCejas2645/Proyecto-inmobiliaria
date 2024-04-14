using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ABM_inmobiliaria.Models
{
    public class RepositorioTipo
    {

        readonly string ConnectionString = "Server=localhost;Database=Inmobiliaria;User=root;Password=;";
        public RepositorioTipo()
        {
        }

        public IList<Tipo> GetTipo()
        {
            var listaTipos = new List<Tipo>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT id, tipoInmueble FROM Tipo";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaTipos.Add(new Tipo
                            {
                                Id = reader.GetInt32("id"),
                                TipoInmueble = reader.GetString("tipoInmueble"),
                            });

                        }
                    }
                }
                connection.Close();
            }

            return listaTipos;

        }

        public void InsertarTipo(Tipo tipo)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO tipo (tipoInmueble)
                VALUES (@tipoInmueble)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tipoInmueble", tipo.TipoInmueble);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}