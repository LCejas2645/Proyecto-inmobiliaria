using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ABM_inmobiliaria.Models
{
    public class RepositorioContrato
    {
        public RepositorioContrato()
        {
        }

        readonly string ConnectionString = "Server=localhost;Database=Inmobiliaria;User=root;Password=;";

        public IList<Contrato> GetContratos()
        {
            IList<Contrato> listaContratos = new List<Contrato>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT c.Id, idInmueble, idInquilino, c.idPropietario, c.idUsuario, vigente, montoMensual, fechaInicio, fechaFin,
                    i.nombre AS nombreInquilino, i.apellido AS apellidoInquilino,
                    p.nombre AS nombrePropietario, p.apellido AS apellidoPropietario,
                    u.nombre AS nombreUsuario, u.apellido AS apellidoUsuario,
                    inm.direccion AS direccionInmueble, inm.ambientes AS ambientes
                FROM contrato c INNER JOIN inquilino i ON c.idInquilino = i.id
                                INNER JOIN propietario p ON c.idPropietario = p.id
                                INNER JOIN usuario u ON c.idUsuario = u.id
                                INNER JOIN inmueble inm ON c.idInmueble = inm.id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Contrato contrato = new Contrato
                            {
                                Id = reader.GetInt32("Id"),
                                FechaFin = reader.GetDateTime("fechaFin"),
                                FechaInicio = reader.GetDateTime("fechaInicio"),
                                IdUsuario = reader.GetInt32("idUsuario"),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32("idInquilino"),
                                    Nombre = reader.GetString("nombreInquilino"),
                                    Apellido = reader.GetString("apellidoInquilino"),
                                },
                                Propietario = new Propietario
                                {
                                    Id = reader.GetInt32("idPropietario"),
                                    Nombre = reader.GetString("nombrePropietario"),
                                    Apellido = reader.GetString("apellidoPropietario")
                                },
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32("idInmueble"),
                                    Direccion = reader.GetString("direccionInmueble"),
                                    Ambientes = reader.GetInt32("ambientes")
                                },
                                Usuario = new Usuario
                                {
                                    Id = reader.GetInt32("idUsuario"),
                                    Nombre = reader.GetString("nombreUsuario"),
                                    Apellido = reader.GetString("apellidoUsuario")
                                },

                                Vigente = reader.GetBoolean("vigente"),
                                MontoMensual = reader.GetDouble("montoMensual")
                            };
                            listaContratos.Add(contrato);
                        }
                        connection.Close();
                    }
                }
                return listaContratos;
            }
        }

        public void InsertarContrato(Contrato contrato)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO contrato (idInquilino, idInmueble, idPropietario, fechaInicio, fechaFin,montoMensual, idUsuario)
                VALUES (@IdInquilino, @IdInmueble, @IdPropietario, @FechaInicio, @FechaFin, @MontoMensual, @IdUsuario)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdPropietario", contrato.IdPropietario);
                    command.Parameters.AddWithValue("@IdInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", contrato.IdInmueble);
                    command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
                    command.Parameters.AddWithValue("@IdUsuario", contrato.IdUsuario);


                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public Contrato? GetContrato(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @"SELECT c.Id, idInmueble, idInquilino, c.idPropietario, vigente, montoMensual, fechaInicio, fechaFin,
                                i.nombre AS nombreInquilino,i.apellido AS apellidoInquilino,
                                p.nombre AS nombrePropietario, p.apellido AS apellidoPropietario,
                                inm.direccion AS direccionInmueble, inm.ambientes AS ambientes
                            FROM contrato c INNER JOIN inquilino i ON c.idInquilino = i.id
                                            INNER JOIN propietario p ON c.idPropietario = p.id
                                            INNER JOIN inmueble inm ON c.idInmueble = inm.id
                            WHERE c.Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Contrato
                            {
                                Id = reader.GetInt32("Id"),
                                FechaFin = reader.GetDateTime("fechaFin"),
                                FechaInicio = reader.GetDateTime("fechaInicio"),
                                IdInquilino = reader.GetInt32("idInquilino"),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32("idInquilino"),
                                    Nombre = reader.GetString("nombreInquilino"),
                                    Apellido = reader.GetString("apellidoInquilino"),
                                },
                                IdPropietario = reader.GetInt32("idPropietario"),
                                Propietario = new Propietario
                                {
                                    Id = reader.GetInt32("idPropietario"),
                                    Nombre = reader.GetString("nombrePropietario"),
                                    Apellido = reader.GetString("apellidoPropietario")
                                },
                                IdInmueble = reader.GetInt32("idInmueble"),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32("idInmueble"),
                                    Direccion = reader.GetString("direccionInmueble"),
                                    Ambientes = reader.GetInt32("ambientes")
                                },
                                Vigente = reader.GetBoolean("vigente"),
                                MontoMensual = reader.GetDouble("montoMensual")
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void ActualizarContrato(Contrato contrato)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = @"UPDATE contrato SET idInquilino = @idInquilino, idInmueble = @idInmueble, idPropietario = @idPropietario, fechaInicio = @fechaInicio, fechaFin = @fechaFin, montoMensual = @montoMensual
                                WHERE id = @id";
                // string sql = @"UPDATE Inmueble SET Direccion = @Direccion, Ambientes = @Ambientes, Superficie = @Superficie,
                //       Latitud = @Latitud, Longitud = @Longitud, IdPropietario = @IdPropietario, Disponible = @Disponible,
                //       IdTipo = @IdTipo, Uso = @Uso, Precio = @Precio WHERE Id = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {

                    command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                    command.Parameters.AddWithValue("@idPropietario", contrato.IdPropietario);
                    command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                    command.Parameters.AddWithValue("@montoMensual", contrato.MontoMensual);
                    command.Parameters.AddWithValue("@id", contrato.Id);
                    // command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                    // command.Parameters.AddWithValue("@Superficie", inmueble.Superficie);
                    // command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                    // command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                    // command.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);
                    // command.Parameters.AddWithValue("@Disponible", inmueble.Disponible);
                    // command.Parameters.AddWithValue("@IdTipo", inmueble.IdTipo);
                    // command.Parameters.AddWithValue("@Uso", inmueble.Uso);
                    // command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    // command.Parameters.AddWithValue("@Id", inmueble.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void EliminarContrato(int id)
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @$"DELETE FROM contrato WHERE {nameof(Contrato.Id)} = @Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        //metodo para controlar el inmueble no este ocupado por otro contrato en las mismas fechas 
        public bool InmuebleOcupadoEnOtroContrato(int idInmueble, DateTime fechaInicio, int idContrato)
        {
            // Consulta para verificar si hay algún contrato que coincida con las fechas del contrato nuevo
            string sql = @"SELECT COUNT(*) FROM contrato 
                   WHERE idInmueble = @IdInmueble 
                   AND fechaFin >= @FechaInicio
                   AND id != @IdContrato";

            using (var connection = new MySqlConnection(ConnectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@IdContrato", idContrato);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Si count es mayor que 0, significa que el inmueble está ocupado en otro contrato con fecha de fin después de la fecha de inicio del contrato nuevo
                    return count > 0;
                }
            }
        }



    }
}