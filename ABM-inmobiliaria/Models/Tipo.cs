using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABM_inmobiliaria.Models
{
    public class Tipo
    {
        private int IdTipo { get; set; }
        private string? TipoInmueble { get; set; }


        public Tipo() { }

        public Tipo(int idTipo, string tipoInmueble)
        {
            IdTipo = idTipo;
            TipoInmueble = tipoInmueble;
        }

    }
}