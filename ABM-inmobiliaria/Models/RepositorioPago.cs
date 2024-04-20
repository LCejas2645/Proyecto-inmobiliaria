
using MySql.Data.MySqlClient;


namespace ABM_inmobiliaria.Models
{
    public class RepositorioPago
    {
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
                                    u.nombre AS nombreUsuario, u.apellido AS apellidoUsuario,
                                    i.apellido AS apellido_inquilino, pr.apellido AS apellido_propietario
                            FROM pago p  
                            INNER JOIN usuario u ON p.idUsuario = u.id
                            INNER jOIN contrato c ON p.idContrato = c.id
                            INNER JOIN inquilino i ON c.idinquilino = i.id
                            INNER JOIN propietario pr ON c.idpropietario = pr.id
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
                                IdContrato = reader.GetInt32("idContrato"),
                                IdUsuario = reader.GetInt32("idUsuario"),
                                usuario = new Usuario
                                {
                                    Nombre = reader.GetString("nombreUsuario"),
                                    Apellido = reader.GetString("apellidoUsuario")
                                },
                                inquilino = new Inquilino{
                                    Apellido = reader.GetString("apellido_inquilino")
                                },
                                propietario = new Propietario{
                                    Apellido = reader.GetString("apellido_propietario")
                                },
                                Fecha = reader.GetDateTime("fecha"),
                                Importe = reader.GetDouble("importe"),
                                Estado = reader.GetBoolean("estado"),
                                Detalle = reader.GetString("detalle")
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

        public void InsertarPago(Pago pago)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"INSERT INTO pago (numeroPago, idContrato, idUsuario, fecha, importe, estado, detalle)
                VALUES (@NumeroPago, @IdContrato, @IdUsuario, @Fecha, @Importe, @Estado, @detalle)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    command.Parameters.AddWithValue("@IdContrato", pago.IdContrato);
                    command.Parameters.AddWithValue("@IdUsuario", pago.IdUsuario);
                    command.Parameters.AddWithValue("@Fecha", pago.Fecha);
                    command.Parameters.AddWithValue("@Importe", pago.Importe);
                    command.Parameters.AddWithValue("@Estado", true);
                    command.Parameters.AddWithValue("@Detalle", pago.Detalle);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public int ObtenerNumeroPago(int idContrato)
        {
            int numeroPago = 1;

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT MAX(NumeroPago) FROM pago WHERE IdContrato = @IdContrato";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader[0] != DBNull.Value)
                        {
                            numeroPago = Convert.ToInt32(reader[0]) + 1;
                        }
                    }
                }
            }
            return numeroPago;
        }


        public void ActualizarPago(Pago pago)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"UPDATE pago SET detalle = @Detalle WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Detalle", pago.Detalle);
                    command.Parameters.AddWithValue("@Id", pago.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }


}

