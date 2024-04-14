using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Hosting;

//1.- REFERENCES AUTHENTICATION COOKIE
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


//1.- AÑADIR LA AUTHORIZACION
using Microsoft.AspNetCore.Authorization;


namespace ABM_inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private RepositorioUsuario rp = new RepositorioUsuario();
        private readonly string pepper = "my_secret_pepper"; // peppered hashing, alternativa a utilizar sal
        private const int hashLength = 32; // Longitud del hash en caracteres (por ejemplo, 32 para 256 bits)

        public UsuarioController(IWebHostEnvironment hostingEnvironment, ILogger<UsuarioController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var usuarios = rp.GetUsuarios();
            return View(usuarios);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            // Generar el hash de la contraseña
            _usuario.Password = contraseñaHash(_usuario.Password);
            var usuario = rp.GetUsuarioEmail(_usuario.Email);

            if (usuario != null && VerificarPassword(_usuario.Password, usuario.Password))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
              new Claim(ClaimTypes.NameIdentifier, usuario.Id+""),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()),
             new Claim("AvatarUrl", usuario.AvatarUrl) // Agrego la URL de la imagen como una claim personalizada
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Password", "El usuario y/o la contraseña son inválidos.");
                return View();
            }
        }


        [Authorize]
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //ABM=============================================================================


        [Authorize(Roles = "Administrador")]
        public IActionResult Insertar()
        {
            return View();
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult Insertar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(usuario.Password))
                {
                    ModelState.AddModelError("Password", "La contraseña es requerida.");
                    return View(usuario);
                }

                if (!usuario.Password.Equals(usuario.ConfirmPassword))
                {
                    ModelState.AddModelError("Password", "Las contraseñas no coinciden.");
                    ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
                    return View(usuario);
                }

                try
                {
                    // Generar el hash de la contraseña del usuario concatenando el pepper
                    string passwordHash = HashPassword(usuario.Password + pepper);
                    // Limitar la longitud del hash
                    usuario.Password = passwordHash.Substring(0, hashLength);
                    rp.InsertarUsuario(usuario);
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View(usuario);
        }


        public IActionResult Actualizar()
        {
            return View();
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                rp.EliminarUsuario(id);
                TempData["Mensaje"] = $"Se eliminó correctamente al usuario";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar al usuario");
                TempData["Mensaje"] = "SError al eliminar al usuario";
                TempData["TipoMensaje"] = "error";
                return RedirectToAction("index");
            }
        }


        //PERFIL ==========================================================================
        [Authorize]
        public IActionResult Perfil()
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);//Obtengo el usuario que inicio sesion desde la claim 
            if (userEmailClaim == null)
            {
                return View("Login");
            }
            var usuario = rp.GetUsuarioEmail(userEmailClaim.Value); //Obtengo el usuario de la bd
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Perfil(Usuario usuarioModificado)
        {
            //obtengo el usuario completo
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);//Obtengo el usuario que inicio sesion desde la claim 
            if (userEmailClaim == null)
            {
                return View("Login");
            }
            var usuario = rp.GetUsuarioEmail(userEmailClaim.Value); //Obtengo el usuario de la bd
            usuarioModificado.Rol = usuario.Rol; //me aseguro que el rol no se cambie desde el front
            usuarioModificado.Password = usuario.Password;
            usuarioModificado.Id = usuario.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    rp.ActualizarUsuario(usuarioModificado);
                    TempData["Mensaje"] = "El usuario se ha modificado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar al usuario");
                    return RedirectToAction("Error");
                }
            }
            usuarioModificado.Password = ""; //en caso de que devuelva la vista
            return View(usuarioModificado);
        }

        //AVATAR===================================================================
        // Acción para mostrar el formulario de carga de avatar
        [Authorize]
        [HttpGet]
        public IActionResult CargarAvatar()
        {
            return View();
        }


        // Acción para procesar la carga del avatar
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CargarAvatar(Usuario usuario)
        {
            if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");

                // Verificar si la carpeta de uploads existe, si no, crearla
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(usuario.AvatarFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await usuario.AvatarFile.CopyToAsync(fileStream);
                }

                usuario.AvatarUrl = "/uploads/" + uniqueFileName; // Aquí se incluye la extensión del archivo en la URL

                // Obtener el ID del usuario actualmente autenticado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Actualizar la URL del avatar en la base de datos del usuario actual
                rp.ActualizarAvatar(int.Parse(userId), usuario.AvatarUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        //=================================================================================
        //Cambiar contraseña
        [Authorize]
        [HttpGet]
        public IActionResult CambiarPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarPassword(CambiarPassword password)
        {
            if (ModelState.IsValid)
            {
                var userEmailClaim = User.FindFirst(ClaimTypes.Email);//Obtengo el usuario que inicio sesion desde la claim 
                if (userEmailClaim == null)
                {
                    return View("Login");
                }
                var usuario = rp.GetUsuarioEmail(userEmailClaim.Value); //Obtengo el usuario de la bd
                password.PasswordActual = contraseñaHash(password.PasswordActual);// Generar el hash de la contraseña actual del form


                if (!VerificarPassword(password.PasswordActual, usuario.Password))  // lógica para verificar la contraseña actual del usuario 
                {
                    ModelState.AddModelError("PasswordActual", "La contraseña actual es incorrecta.");
                    return View(password);
                }

                if (!VerificarPassword(password.NuevaPassword, password.ConfirmarPassword))  // Verificar si la nueva contraseña y la confirmación coinciden
                {
                    //ModelState.AddModelError(string.Empty, "La nueva contraseña y la confirmación no coinciden.");
                    ModelState.AddModelError("NuevaPassword", "Las contraseñas no coinciden.");
                    ModelState.AddModelError("ConfirmarPassword", "Las contraseñas no coinciden.");
                    return View(password);
                }
                usuario.Password = contraseñaHash(password.NuevaPassword);  //Genero el hash de la contraseña nueva
                rp.ActualizarPassword(usuario);  //Guardo los cambios en la bd: 
                return RedirectToAction("Index");  //Redirijo al index
            }
            return View(password);   //se vuelve a mostrar el formulario con los errores
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }


        // Método para generar el hash de la contraseña concatenando el pepper
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", ""); // Convertir a una cadena hexadecimal sin guiones
            }
        }

        // Método para verificar la contraseña
        private bool VerificarPassword(string password1, string password2)
        {
            return password1.Equals(password2);
        }

        private string contraseñaHash(string contraseña)
        {
            // Generar el hash de la contraseña del usuario concatenando el pepper
            string passwordHash = HashPassword(contraseña + pepper);
            return passwordHash.Substring(0, hashLength);
        }
    }
}