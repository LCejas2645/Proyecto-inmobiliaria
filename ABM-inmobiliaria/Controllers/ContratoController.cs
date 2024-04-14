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
                // Verificar si la fecha de inicio es mayor o igual que la fecha de fin
                if (contrato.FechaInicio >= contrato.FechaFin)
                {
                    throw new ArgumentException("Error: La fecha de inicio no puede ser mayor o igual que la fecha de fin.");
                }

                // Verificar si el inmueble está ocupado en otro contrato entre las fechas proporcionadas
                bool inmuebleOcupado = rc.InmuebleOcupadoEnOtroContrato(contrato.idInmueble, contrato.FechaInicio, contrato.Id);
                if (inmuebleOcupado)
                {
                    throw new ArgumentException("Error: El inmueble ya está ocupado en otro contrato durante las fechas proporcionadas.");
                }

                contrato.Inmueble = rinm.GetInmueble(contrato.idInmueble);

                //Verificar que el inmueble este disponible
                if (contrato.Inmueble != null && !contrato.Inmueble.Disponible)
                {
                    throw new ArgumentException("Error: El inmueble seleccionado no está disponible actualmente");
                }


                // Si no hay errores, insertar el contrato
                if (ModelState.IsValid)
                {
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
                // Verificar si la fecha de inicio es mayor o igual que la fecha de fin
                if (contrato.FechaInicio >= contrato.FechaFin)
                {
                    throw new ArgumentException("Error: La fecha de inicio no puede ser mayor o igual que la fecha de fin.");
                }

                // Verificar si el inmueble está ocupado en otro contrato entre las fechas proporcionadas
                bool inmuebleOcupado = rc.InmuebleOcupadoEnOtroContrato(contrato.idInmueble, contrato.FechaInicio,contrato.Id);
                if (inmuebleOcupado)
                {
                    throw new ArgumentException("Error: El inmueble ya está ocupado en otro contrato durante las fechas proporcionadas.");
                }

                contrato.Inmueble = rinm.GetInmueble(contrato.idInmueble);

                //Verificar que el inmueble este disponible
                if (contrato.Inmueble != null && !contrato.Inmueble.Disponible)
                {
                    throw new ArgumentException("Error: El inmueble seleccionado no está disponible actualmente");
                }

                if (ModelState.IsValid)
                {
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

        public IActionResult Eliminar(int id)
        {
            try
            {
                rc.EliminarContrato(id);
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