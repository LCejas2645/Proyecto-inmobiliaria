using MySql.Data.MySqlClient;


namespace ABM_inmobiliaria.Models
{
    public class RepositorioPago
    {
        public RepositorioPago()
        {
        }

        readonly string ConnectionString = "Server=localhost;Database=Inmobiliaria;User=root;Password=;";


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
