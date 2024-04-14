using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Core;


namespace ABM_inmobiliaria.Models
{
    public enum Roles
    {
        Administrador = 1,
        Empleado = 2
    }

    public class Usuario
    {

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        //para cuando cambio la contraseña
        // public string? PasswordActual { get; set; }

        public string AvatarUrl { get; set; } = "";

        [DataType(DataType.Upload)]
        [Display(Name = "Avatar")]
        [BindNever]
        public IFormFile AvatarFile { get; set; }

        public Roles Rol { get; set; }

        public override string ToString()
        {
            return Id + "";
        }

    }


}