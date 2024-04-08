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
                var sql = @"SELECT c.Id, idInmueble, idInquilino, c.idPropietario, vigente, montoMensual, fechaInicio, fechaFin,
                                i.nombre AS nombreInquilino,i.apellido AS apellidoInquilino,
                                p.nombre AS nombrePropietario, p.apellido AS apellidoPropietario,
                                inm.direccion AS direccionInmueble, inm.ambientes AS ambientes
                            FROM contrato c INNER JOIN inquilino i ON c.idInquilino = i.id
                                            INNER JOIN propietario p ON c.idPropietario = p.id
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
                                Inquilino = new Inquilino{
                                    Id = reader.GetInt32("idInquilino"),
                                    Nombre = reader.GetString("nombreInquilino"),
                                    Apellido = reader.GetString("apellidoInquilino"),
                                },
                                Propietario = new Propietario{
                                    Id = reader.GetInt32("idPropietario"),
                                    Nombre = reader.GetString("nombrePropietario"),
                                    Apellido = reader.GetString("apellidoPropietario")
                                },
                                Inmueble = new Inmueble{
                                    Id = reader.GetInt32("idInmueble"),
                                    Direccion = reader.GetString("direccionInmueble"),
                                    Ambientes = reader.GetInt32("ambientes")
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
                string sql = @"INSERT INTO contrato (idInquilino, idInmueble, idPropietario, fechaInicio, fechaFin,montoMensual)
                VALUES (@IdInquilino, @IdInmueble, @IdPropietario, @FechaInicio, @FechaFin, @MontoMensual)";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@IdPropietario", contrato.idPropietario);
                    command.Parameters.AddWithValue("@IdInquilino", contrato.idInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", contrato.idInmueble);
                    command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    command.Parameters.AddWithValue("@MontoMensual", contrato.MontoMensual);
                

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public Contrato? GetContrato(int id){
            using (var connection = new MySqlConnection(ConnectionString)){
                var sql = @"SELECT c.Id, idInmueble, idInquilino, c.idPropietario, vigente, montoMensual, fechaInicio, fechaFin,
                                i.nombre AS nombreInquilino,i.apellido AS apellidoInquilino,
                                p.nombre AS nombrePropietario, p.apellido AS apellidoPropietario,
                                inm.direccion AS direccionInmueble, inm.ambientes AS ambientes
                            FROM contrato c INNER JOIN inquilino i ON c.idInquilino = i.id
                                            INNER JOIN propietario p ON c.idPropietario = p.id
                                            INNER JOIN inmueble inm ON c.idInmueble = inm.id
                            WHERE c.Id = @Id";

                using (var command = new MySqlCommand(sql, connection)){
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader()){
                        if (reader.Read()){
                            return new Contrato{
                                Id = reader.GetInt32("Id"),
                                FechaFin = reader.GetDateTime("fechaFin"),
                                FechaInicio = reader.GetDateTime("fechaInicio"),
                                idInquilino = reader.GetInt32("idInquilino"),
                                Inquilino = new Inquilino{
                                    Id = reader.GetInt32("idInquilino"),
                                    Nombre = reader.GetString("nombreInquilino"),
                                    Apellido = reader.GetString("apellidoInquilino"),
                                },
                                idPropietario = reader.GetInt32("idPropietario"),
                                Propietario = new Propietario{
                                    Id = reader.GetInt32("idPropietario"),
                                    Nombre = reader.GetString("nombrePropietario"),
                                    Apellido = reader.GetString("apellidoPropietario")
                                },
                                idInmueble = reader.GetInt32("idInmueble"),
                                Inmueble = new Inmueble{
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
                        
                         command.Parameters.AddWithValue("@idInquilino", contrato.idInquilino);
                         command.Parameters.AddWithValue("@idInmueble", contrato.idInmueble);
                         command.Parameters.AddWithValue("@idPropietario", contrato.idPropietario);
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
    }
}