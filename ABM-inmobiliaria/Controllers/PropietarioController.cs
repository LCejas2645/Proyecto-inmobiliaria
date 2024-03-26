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

        public PropietarioController(ILogger<PropietarioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            RepositorioPropietario rp = new RepositorioPropietario();
            var listaPropietarios = rp.GetPropietarios();
            return View(listaPropietarios);
        }

        public IActionResult Insertar(int? id)
        {
            if (id != null)
            {
                // Si tiene un id, es una solicitud para actualizar
                RepositorioPropietario rp = new RepositorioPropietario();
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
            if (ModelState.IsValid)
            {
                RepositorioPropietario rp = new RepositorioPropietario();
                if (propietario.Id> 0)
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

        public IActionResult Eliminar(int id)
        {
            
            RepositorioPropietario rp = new RepositorioPropietario();
            rp.EliminarPropietario(id);
            TempData["Mensaje"] = $"Se eliminó correctamente al propietario";
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}