using ABM_inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZstdSharp.Unsafe;


namespace ABM_inmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        private readonly ILogger<PagoController> _logger;
        RepositorioPago rp = new RepositorioPago();
        RepositorioContrato rc = new RepositorioContrato();

        public PagoController(ILogger<PagoController> logger)
        {
            _logger = logger;
        }


        [Authorize]
        public IActionResult Insertar(int idContrato)
        {
            var contrato = rc.GetContrato(idContrato);
            var pago = new Pago
            {
                IdContrato = idContrato,
                Contrato = contrato
            };
            return View(pago);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insertar(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pago.NumeroPago = rp.ObtenerNumeroPago(pago.IdContrato);
                    pago.IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // id del usuario autenticado

                    rp.InsertarPago(pago);
                    TempData["Mensaje"] = "Se realizó el pago correctamente";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                return View(pago);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar el pago en la bd");
                return RedirectToAction("Error");
            }

        }

          [Authorize]
        public IActionResult Actualizar(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Actualizar(Pago pago)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    rp.ActualizarPago(pago);
                    TempData["Mensaje"] = "El pago se ha actualizado correctamente.";
                    TempData["TipoMensaje"] = "success";
                    return RedirectToAction("Index");
                }
                return View(pago);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el pago");
                return RedirectToAction("Error");
            }
        }





    }
}