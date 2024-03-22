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

        public IActionResult Insertar()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Insertar(Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                RepositorioInquilino rp = new RepositorioInquilino();
                rp.InsertarInquilino(inquilino);
                TempData["Mensaje"] = $"Se ha insertado correctamente a {inquilino.Nombre} {inquilino.Apellido}";
                return RedirectToAction("Index");
            }
            return View(inquilino);
        }

        public IActionResult Actualizar(int id)
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            var inquilino = rp.GetInquilino(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        [HttpPost]
        public IActionResult Actualizar(Inquilino inquilino)
        {

            if (ModelState.IsValid)
            {
                RepositorioInquilino rp = new RepositorioInquilino();
                rp.ActualizarInquilino(inquilino);
                TempData["Mensaje"] = $"Se han actualizado los datos de {inquilino.Nombre} {inquilino.Apellido}";
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