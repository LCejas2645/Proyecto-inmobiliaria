using System;
using System.Collections.Generic;
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
                var sql = @"SELECT Id, idInquilino, idInmueble, idPropietario, idUsuario, fechaInicio, fechaFin, vigente, montoMensual";

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
                                Inquilino = new Inquilino{

                                },
                                Propietario = new Propietario{

                                },
                                Inmueble = new Inmueble{
                                    
                                },
                                usuario = new Usuario{

                                }
                                
                                
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