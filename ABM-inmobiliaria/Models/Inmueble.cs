using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABM_inmobiliaria.Models
{
    public class Inmueble
    {

        public int Id { get; set; }
        public String Direccion { get; set; } = "";
        public int Ambientes { get; set; }
        public int Superficie { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int IdPropietario { get; set; }
        public Propietario? Propietario { get; set; }
        public int IdTipo { get; set; }
        public Tipo? Tipo { get; set; }
        public string Uso { get; set; } = "";
        public double Precio { get; set; }
        public bool Disponible { get; set; }




        public override string ToString()
        {
            return $"{Ambientes} Ambientes";
        }

    }
}