using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
//using ZstdSharp.Unsafe;

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

        //private RepositorioUsuario ru = new RepositorioUsuario();




        public ContratoController(ILogger<InmuebleController> logger)
        {
            _logger = logger;
        }



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

        public IActionResult Insertar()
        {
            ViewBag.Propietarios = rp.GetPropietarios();
            ViewBag.Inquilinos = ri.GetInquilinos();
            ViewBag.inmuebles = rinm.GetInmuebles();
            return View();
        }

        [HttpPost]
        public IActionResult Insertar(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rc.InsertarContrato(contrato);
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.inmuebles = rinm.GetInmuebles();
                return View(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar el contrato");
                return RedirectToAction("Error");
            }

        }

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

        [HttpPost]
        public IActionResult Actualizar(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rc.ActualizarContrato(contrato);
                    return RedirectToAction("Index");
                }
                ViewBag.Propietarios = rp.GetPropietarios();
                ViewBag.Inquilinos = ri.GetInquilinos();
                ViewBag.Inmuebles = rinm.GetInmuebles();
                return View(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar o actualizar al inquilino");
                return RedirectToAction("Error");
            }
        }
    }
}