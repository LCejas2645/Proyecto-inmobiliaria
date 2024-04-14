using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ABM_inmobiliaria.Models;

namespace ABM_inmobiliaria.Controllers{
    public class TipoController : Controller{
        private readonly ILogger<TipoController> _logger;

        RepositorioTipo rt = new RepositorioTipo();

        public TipoController(ILogger<TipoController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var listaTipo = rt.GetTipo();
                return View(listaTipo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de tipos de inmueble");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Insertar()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Insertar(Tipo tipo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rt.InsertarTipo(tipo);
                    TempData["Mensaje"] = "El tipo de inmueble se ha creado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                return View(tipo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar el tipo de inmueble");
                return RedirectToAction("Error");
            }

        }
    }

}