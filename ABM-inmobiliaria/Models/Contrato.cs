using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABM_inmobiliaria.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Propietario? Propietario { get; set; }
        public Inmueble? Inmueble { get; set; }
        public Usuario? usuario {get;set;}
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Vigente { get; set; }
        public double MontoMensual { get; set; }

        //public string? Usuario { get; set; } = null;

        //LONGITUD???

        public Contrato(int id, Inquilino inquilino, Inmueble inmueble, Propietario propietario, DateTime fechaInicio, DateTime fechaFin, bool vigente, double montoMensual)
        {
            Id = id;
            Inquilino = inquilino;
            Inmueble = inmueble;
            Propietario = propietario;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Vigente = vigente;
            MontoMensual = montoMensual;
        }

        public Contrato()
        {

        }

        // public static implicit operator Contrato(Contrato v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}