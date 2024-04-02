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
    public class ContratoController : Controller
    {
        private readonly ILogger<InmuebleController> _logger;
        RepositorioContrato rc = new RepositorioContrato();
        // private RepositorioPropietario rp = new RepositorioPropietario();
        // private RepositorioTipo rt = new RepositorioTipo();


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
    }
}