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

        RepositorioAuditoria ra = new RepositorioAuditoria();

        RepositorioUsuario ru = new RepositorioUsuario();

        public PagoController(ILogger<PagoController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var pagos = rp.GetPagos();
                return View(pagos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de pagos");
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                rp.EliminarPago(id);
                // Insertar entrada en la tabla de auditoría
                var usuario = ru.GetUsuarioEmail(User.FindFirst(ClaimTypes.Email).Value); ///Obtengo el usuario que inicio sesion desde la claim 
                var idUsuario = usuario.Id;
                int idEntidad = id; // El ID de la entidad eliminada (en este caso, el contrato)
                bool entidad = false; // El nombre de la entidad eliminada
                DateTime fechaAccion = DateTime.Now; // La fecha y hora actual
                ra.InsertarAuditoria(idUsuario, id, entidad, fechaAccion);
                
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


        [Authorize]
        public IActionResult Insertar(int idContrato)
        {
            var contrato = rc.GetContrato(idContrato);
            var pago = new Pago
            {
                IdContrato = idContrato,
                Contrato = contrato
            };
            ViewBag.Contratos = rc.GetContratos();
            return View(pago);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Insertar(Pago pago)
        {
            try
            {
                if (pago.Importe <= 0)
                {
                    ModelState.AddModelError("Importe", "El importe debe ser un número válido y mayor que cero.");
                     ViewBag.Contratos = rc.GetContratos();
                }

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