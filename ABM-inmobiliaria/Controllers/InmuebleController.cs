using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;

namespace ABM_inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;

        private RepositorioPropietario rp = new RepositorioPropietario();
        private RepositorioTipo rt = new RepositorioTipo();


        public InmuebleController(ILogger<InmuebleController> logger)
        {
            _logger = logger;
        }



        public IActionResult Index()
        {
            RepositorioInmueble ri = new RepositorioInmueble();
            var inmuebles = ri.GetInmuebles();
            return View(inmuebles);
        }



        public IActionResult Insertar()
        {
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.TiposInmueble = rt.GetTipo();
            return View();
        }

        [HttpPost]
        public IActionResult Insertar(Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                RepositorioInmueble ri = new RepositorioInmueble();
                ri.InsertarInmueble(inmueble);
                return RedirectToAction("Index");
            }
            return View(inmueble);
        }


        public IActionResult Actualizar(int id)
        {
            RepositorioInmueble ri = new RepositorioInmueble();
            var inmueble = ri.GetInmueble(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.TiposInmueble = rt.GetTipo();

            return View(inmueble);
        }

        [HttpPost]
        public IActionResult Actualizar(Inmueble inmueble)
        {
            RepositorioInmueble ri = new RepositorioInmueble();

            if (ModelState.IsValid)
            {
                ri.ActualizarInmueble(inmueble);
                return RedirectToAction("Index");
            }

            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.TiposInmueble = rt.GetTipo();

            return View(inmueble);
        }



        public IActionResult Prueba()
        {
            RepositorioInmueble ri = new RepositorioInmueble();
            var inmueble = ri.GetInmueble(9);
            return View(inmueble);
        }




        public IActionResult Eliminar(int id)
        {
            RepositorioInmueble ri = new RepositorioInmueble();
            ri.EliminarInmueble(id);

            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}