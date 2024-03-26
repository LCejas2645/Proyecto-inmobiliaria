using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABM_inmobiliaria.Models
{
    public class Inmueble
    {
        private int Id { get; set; }
        private String? Direccion { get; set; }
        private int CantAmbientes { get; set; }
        public double Superficie { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public Propietario Propietario { get; set; }
        public bool Disponible { get; set; }
        public Tipo Tipo { get; set; }
        public string Uso { get; set; }
        public double Precio { get; set; }

        public Inmueble(int id, string direccion, int cantAmbientes, double superficie, double latitud, double longitud, Propietario propietario, bool disponible, Tipo tipo, string uso, double precio)
        {
            Id = id;
            Direccion = direccion;
            CantAmbientes = cantAmbientes;
            Superficie = superficie;
            Latitud = latitud;
            Longitud = longitud;
            Propietario = propietario;
            Disponible = disponible;
            Tipo = tipo;
            Uso = uso;
            Precio = precio;
        }
    }
}