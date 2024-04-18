using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABM_inmobiliaria.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        
        public int IdUsuario {get;set;}

        public Usuario? Usuario {get;set;}

        public int IdEntidad {get;set;}

        public Boolean Entidad {get;set;}

        public DateTime FechaAccion{get;set;}


        public Auditoria(int id,int idUsuario , int idEntidad, Boolean entidad, DateTime fechaAccion){
            Id = id;
            IdUsuario = idUsuario;
            //Usuario = usuario;
            IdEntidad = idEntidad;
            Entidad = entidad;
            FechaAccion = fechaAccion;
        }

        public Auditoria(){

        }
    }
}