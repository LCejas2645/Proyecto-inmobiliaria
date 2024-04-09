using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ABM_inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly ILogger<InquilinoController> _logger;
        RepositorioInquilino rp = new RepositorioInquilino();

        public InquilinoController(ILogger<InquilinoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var listaInquilino = rp.GetInquilinos();
                return View(listaInquilino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de propietarios");
                return RedirectToAction("Error");
            }

        }

        public IActionResult Insertar(int? id)
        {
            if (id != null)
            {
                // Si tiene un id, es una solicitud para actualizar
                var inquilino = rp.GetInquilino(id.Value);
                if (inquilino == null)
                {
                    return NotFound();
                }
                return View(inquilino);
            }
            // Si no trae id, es una solicitud para insertar
            var nuevoInquilino = new Inquilino();
            return View(nuevoInquilino);
        }

        [HttpPost]
        public IActionResult Insertar(Inquilino inquilino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inquilino.Id > 0)
                    {
                        // Si el Id es mayor que cero, es una solicitud de actualización.
                        rp.ActualizarInquilino(inquilino);
                        TempData["Mensaje"] = $"Se han actualizado los datos de {inquilino.Nombre} {inquilino.Apellido}";
                        TempData["TipoMensaje"] = "success";
                    }
                    else
                    {
                        // Si el Id es cero o menos, es una solicitud de inserción.
                        rp.InsertarInquilino(inquilino);
                        TempData["Mensaje"] = $"Se ha insertado correctamente a {inquilino.Nombre} {inquilino.Apellido}";
                        TempData["TipoMensaje"] = "success";
                    }
                    return RedirectToAction("Index");
                }
                return View(inquilino);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar o actualizar al inquilino");
                return RedirectToAction("Error");
            }
        }


        public IActionResult Eliminar(int id)
        {
            try
            {
                rp.EliminarInquilino(id);
                TempData["Mensaje"] = $"Se eliminó correctamente al inquilino";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar al inquilino");
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