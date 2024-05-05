using MySql.Data.MySqlClient;

namespace ABM_inmobiliaria.Models{
    public class RepositorioAuditoria{
        public RepositorioAuditoria()
        {
        }

        readonly string ConnectionString = "Server=localhost;Database=Inmobiliaria;User=root;Password=;";

        public IList<Auditoria> GetAuditorias(){
            IList<Auditoria> auditorias = new List<Auditoria>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT Id, idUsuario, idEntidad , entidad, fechaAccion
                            FROM auditoria";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Auditoria auditoria = new Auditoria{
                                Id = reader.GetInt32("Id"),
                                IdUsuario = reader.GetInt32("idUsuario"),
                                IdEntidad = reader.GetInt32("idEntidad"),
                                Entidad = reader.GetBoolean("entidad"),
                                FechaAccion = reader.GetDateTime("fechaAccion")
                            };
                            auditorias.Add(auditoria);
                        }
                        connection.Close();
                    }
                }


            return auditorias;
            }
        }

        public void InsertarAuditoria(int idUsuario, int idEntidad, bool entidad, DateTime fechaAccion){
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO auditoria (idUsuario, idEntidad, entidad, fechaAccion)
                VALUES (@idUsuario, @idEntidad, @entidad, @fechaAccion)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    command.Parameters.AddWithValue("@idEntidad", idEntidad);
                    command.Parameters.AddWithValue("@entidad", entidad);
                    command.Parameters.AddWithValue("@fechaAccion", fechaAccion);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}