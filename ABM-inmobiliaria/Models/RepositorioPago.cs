using MySql.Data.MySqlClient;


namespace ABM_inmobiliaria.Models{
    public class RepositorioPago{
        public RepositorioPago()
        {
        }

        readonly string ConnectionString = "Server=localhost;Database=Inmobiliaria;User=root;Password=;";

        public IList<Pago> GetPagos()
        {
            IList<Pago> listaPagos = new List<Pago>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT p.Id, numeroPago, idContrato, p.idUsuario, fecha, importe, estado, detalle,
                                     u.nombre AS nombreUsuario, u.apellido AS apellidoUsuario
                            FROM pago p  
                            INNER JOIN usuario u ON p.idUsuario = u.id
                            WHERE estado = 1";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Pago pago = new Pago
                            {
                                Id = reader.GetInt32("Id"),
                                NumeroPago = reader.GetInt32("numeroPago"),
                                IdContrato =reader.GetInt32("idContrato"),
                                IdUsuario = reader.GetInt32("idUsuario"),
                                usuario = new Usuario{
                                    Nombre = reader.GetString("nombreUsuario"),
                                    Apellido  =reader.GetString("apellidoUsuario")
                                },
                                Fecha = reader.GetDateTime("fecha"),
                                Importe = reader.GetDouble("importe"),
                                Estado = reader.GetBoolean("estado"),
                                Detalles = reader.GetString("detalle")
                            };
                            listaPagos.Add(pago);
                        }
                        connection.Close();
                    }
                }
                return listaPagos;
            }
        }

        public void EliminarPago(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @$"UPDATE  pago SET estado = @Estado
                WHERE id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {   

                    command.Parameters.AddWithValue("@Estado", false);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        
    }
}
