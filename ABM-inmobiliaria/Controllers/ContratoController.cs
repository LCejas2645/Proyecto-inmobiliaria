using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZstdSharp.Unsafe;

namespace ABM_inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;
        RepositorioContrato rc = new RepositorioContrato();
        private RepositorioPropietario rp = new RepositorioPropietario();
        private RepositorioTipo rt = new RepositorioTipo();
        private RepositorioInquilino ri = new RepositorioInquilino();
        private RepositorioInmueble rinm = new RepositorioInmueble();
        private RepositorioUsuario ru = new RepositorioUsuario();

        private RepositorioAuditoria ra = new RepositorioAuditoria();




        public ContratoController(ILogger<InmuebleController> logger)
        {
            _logger = logger;
        }



        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var contratos = rc.GetContratos();
                return View(contratos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de contratos");
                return RedirectToAction("Error");
            }
        }

        [Authorize]
        public IActionResult Insertar()
        {
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.Inquilinos = ri.GetInquilinos();
            ViewBag.inmuebles = rinm.GetInmuebles();

            

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insertar(Contrato contrato)
        {
            try
            {
                // Verificar si la fecha de inicio es mayor o igual que la fecha de fin
                if (contrato.FechaInicio >= contrato.FechaFin)
                {
                    throw new ArgumentException("Error: La fecha de inicio no puede ser mayor o igual que la fecha de fin.");
                }

                // Verificar si el inmueble está ocupado en otro contrato entre las fechas proporcionadas
                bool inmuebleOcupado = rc.InmuebleOcupadoEnOtroContrato(contrato.IdInmueble, contrato.FechaInicio, contrato.Id);
                if (inmuebleOcupado)
                {
                    throw new ArgumentException("Error: El inmueble ya está ocupado en otro contrato durante las fechas proporcionadas.");
                }

                contrato.Inmueble = rinm.GetInmueble(contrato.IdInmueble);

                //Verificar que el inmueble este disponible
                if (contrato.Inmueble != null && !contrato.Inmueble.Disponible)
                {
                    throw new ArgumentException("Error: El inmueble seleccionado no está disponible actualmente");
                }


                // Si no hay errores, insertar el contrato
                if (ModelState.IsValid)
                {
                    //Obtengo el propietario perteneciente al inmueble
                    contrato.IdPropietario = contrato.Inmueble.IdPropietario;
                    //Obtengo el usuario que realizo el contrato
                    if (User.Identity.IsAuthenticated)
                    {
                        var usuario = ru.GetUsuarioEmail(User.FindFirst(ClaimTypes.Email).Value); ///Obtengo el usuario que inicio sesion desde la claim 
                        contrato.Usuario = usuario;
                        contrato.IdUsuario = usuario.Id;
                    }
                    rc.InsertarContrato(contrato);
                    TempData["Mensaje"] = "El contrato se ha creado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.inmuebles = rinm.GetInmuebles();

                return View(contrato);
            }
            catch (ArgumentException ex)
            {
                // Capturar la excepción y mostrar el mensaje de error
                TempData["Mensaje"] = ex.Message;
                TempData["TipoMensaje"] = "error";
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.inmuebles = rinm.GetInmuebles();

                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar el contrato");
                return RedirectToAction("Error");
            }
        }


        [Authorize]
        public IActionResult Actualizar(int id)
        {
            var contrato = rc.GetContrato(id);
            if (contrato == null)
            {
                return NotFound();
            }
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.Inquilinos = ri.GetInquilinos();
            ViewBag.Inmuebles = rinm.GetInmuebles();
            return View(contrato);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Actualizar(Contrato contrato)
        {
            try
            {
                // Verificar si la fecha de inicio es mayor o igual que la fecha de fin
                if (contrato.FechaInicio >= contrato.FechaFin)
                {
                    throw new ArgumentException("Error: La fecha de inicio no puede ser mayor o igual que la fecha de fin.");
                }

                // Verificar si el inmueble está ocupado en otro contrato entre las fechas proporcionadas
                bool inmuebleOcupado = rc.InmuebleOcupadoEnOtroContrato(contrato.IdInmueble, contrato.FechaInicio, contrato.Id);
                if (inmuebleOcupado)
                {
                    throw new ArgumentException("Error: El inmueble ya está ocupado en otro contrato durante las fechas proporcionadas.");
                }

                contrato.Inmueble = rinm.GetInmueble(contrato.IdInmueble);

                //Verificar que el inmueble este disponible
                if (contrato.Inmueble != null && !contrato.Inmueble.Disponible)
                {
                    throw new ArgumentException("Error: El inmueble seleccionado no está disponible actualmente");
                }

                if (ModelState.IsValid)
                {
                    //Obtengo el propietario perteneciente al inmueble
                    contrato.IdPropietario = contrato.Inmueble.IdPropietario;
                    rc.ActualizarContrato(contrato);
                    TempData["Mensaje"] = "El contrato se ha actualizado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.Inmuebles = rinm.GetInmuebles();
                return View(contrato);
            }
            catch (ArgumentException ex)
            {
                TempData["Mensaje"] = ex.Message;
                TempData["TipoMensaje"] = "error";
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.Inmuebles = rinm.GetInmuebles();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el contrato");
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                rc.EliminarContrato(id);

                // Insertar entrada en la tabla de auditoría
                var usuario = ru.GetUsuarioEmail(User.FindFirst(ClaimTypes.Email).Value); ///Obtengo el usuario que inicio sesion desde la claim 
                var idUsuario = usuario.Id;
                int idEntidad = id; // El ID de la entidad eliminada (en este caso, el contrato)
                bool entidad = true; // El nombre de la entidad eliminada
                DateTime fechaAccion = DateTime.Now; // La fecha y hora actual

                ra.InsertarAuditoria(idUsuario, id, entidad, fechaAccion);
                //ra.InsertarAuditoria(id);
                TempData["Mensaje"] = $"Se eliminó correctamente el contrato";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el contrato");
                return RedirectToAction("Error");
            }
        }

    }
}