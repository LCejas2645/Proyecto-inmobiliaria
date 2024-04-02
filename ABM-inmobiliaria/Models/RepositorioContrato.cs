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
                var sql = @"SELECT c.Id, idInquilino, idPropietario, vigente, montoMensual, fechaInicio, fechaFin,
                                i.nombre AS nombreInquilino,i.apellido AS apellidoInquilino,
                                p.nombre AS nombrePropietario, p.apellido AS apellidoPropietario
                            FROM contrato c INNER JOIN inquilino i ON c.idInquilino = i.id
                                            INNER JOIN propietario p ON c.idPropietario = p.id";

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
    }
}