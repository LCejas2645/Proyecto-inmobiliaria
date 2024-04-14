using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;
using Microsoft.AspNetCore.Authorization;

namespace ABM_inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;
        RepositorioInmueble ri = new RepositorioInmueble();
        private RepositorioPropietario rp = new RepositorioPropietario();
        private RepositorioTipo rt = new RepositorioTipo();


        public InmuebleController(ILogger<InmuebleController> logger)
        {
            _logger = logger;
        }



         [Authorize]
        public IActionResult Index()
        {
            try
            {
                var inmuebles = ri.GetInmuebles();
                return View(inmuebles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de inmuebles");
                return RedirectToAction("Error");
            }

        }


        [Authorize]
        public IActionResult Insertar()
        {
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.TiposInmueble = rt.GetTipo();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insertar(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ri.InsertarInmueble(inmueble);
                    TempData["Mensaje"] = "El inmueble se ha creado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.TiposInmueble = rt.GetTipo();
                return View(inmueble);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar el inmueble");
                return RedirectToAction("Error");
            }

        }


        [Authorize]
        public IActionResult Actualizar(int id)
        {
            var inmueble = ri.GetInmueble(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.TiposInmueble = rt.GetTipo();
            return View(inmueble);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Actualizar(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ri.ActualizarInmueble(inmueble);
                    TempData["Mensaje"] = "El inmueble se ha actualizado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.TiposInmueble = rt.GetTipo();
                return View(inmueble);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar o actualizar al inquilino");
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                ri.EliminarInmueble(id);
                TempData["Mensaje"] = $"Se eliminó correctamente el inmueble";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el inmueble");
                return RedirectToAction("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}