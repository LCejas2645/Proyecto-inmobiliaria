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

        public InquilinoController(ILogger<InquilinoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            var listaInquilino = rp.GetInquilinos();
            return View(listaInquilino);
        }

        public IActionResult Insertar(int? id)
        {
            if (id != null)
            {
                // Si tiene un id, es una solicitud para actualizar
                RepositorioInquilino rp = new RepositorioInquilino();
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
            if (ModelState.IsValid)
            {
                RepositorioInquilino rp = new RepositorioInquilino();
                if (inquilino.Id > 0)
                {
                    // Si el Id es mayor que cero, es una solicitud de actualización.
                    rp.ActualizarInquilino(inquilino);
                    TempData["Mensaje"] = $"Se han actualizado los datos de {inquilino.Nombre} {inquilino.Apellido}";
                }
                else
                {
                    // Si el Id es cero o menos, es una solicitud de inserción.
                    rp.InsertarInquilino(inquilino);
                    TempData["Mensaje"] = $"Se ha insertado correctamente a {inquilino.Nombre} {inquilino.Apellido}";
                }
                return RedirectToAction("Index");
            }
            return View(inquilino);
        }


        public IActionResult Eliminar(int id)
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            rp.EliminarInquilino(id);
            TempData["Mensaje"] = $"Se eliminó correctamente al inquilino";
            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}