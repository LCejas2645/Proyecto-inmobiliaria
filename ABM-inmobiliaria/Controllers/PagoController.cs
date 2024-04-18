using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace ABM_inmobiliaria.Controllers
{

    public class PagoController : Controller
    {
        private readonly ILogger<RepositorioPago> _logger;
        private RepositorioPago rpa = new RepositorioPago();

        public PagoController(ILogger<RepositorioPago> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            try
            {
                var pagos = rpa.GetPagos();
                return View(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Eliminar(int id)
        {
            try
            {
                rpa.EliminarPago(id);
                TempData["Mensaje"] = $"Se eliminó correctamente el pago";
                TempData["TipoMensaje"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el pago");
                return RedirectToAction("Error");
            }
        }
    }

}