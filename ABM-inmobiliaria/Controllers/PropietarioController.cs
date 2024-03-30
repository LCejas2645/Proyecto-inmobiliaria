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
    public class PropietarioController : Controller
    {
        private readonly ILogger<PropietarioController> _logger;
        RepositorioPropietario rp = new RepositorioPropietario();

        public PropietarioController(ILogger<PropietarioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var listaPropietarios = rp.GetPropietarios();
                return View(listaPropietarios);
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
                var propietario = rp.GetPropietario(id.Value);
                if (propietario == null)
                {
                    return NotFound();
                }
                return View(propietario);
            }
            // Si no trae id, es una solicitud para insertar
            var nuevoPropietario = new Propietario();
            return View(nuevoPropietario);
        }

        [HttpPost]
        public IActionResult Insertar(Propietario propietario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (propietario.Id > 0)
                    {
                        // Si el Id es mayor que cero, es una solicitud de actualización.
                        rp.ActualizarPropietario(propietario);
                        TempData["Mensaje"] = $"Se han actualizado los datos de {propietario.Nombre} {propietario.Apellido}";
                    }
                    else
                    {
                        // Si el Id es cero o menos, es una solicitud de inserción.
                        rp.InsertarPropietario(propietario);
                        TempData["Mensaje"] = $"Se ha insertado correctamente a {propietario.Nombre} {propietario.Apellido}";
                    }
                    return RedirectToAction("Index");
                }
                return View(propietario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar o actualizar el propietario");
                return RedirectToAction("Error");
            }

        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                rp.EliminarPropietario(id);
                TempData["Mensaje"] = $"Se eliminó correctamente al propietario";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el propietario");
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