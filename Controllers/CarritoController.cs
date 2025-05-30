using System.Net;
using System.Security.Claims;
using System.Text.Json;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class CarritoController(IConfiguration configuration, ComprarClientService comprarClientService) : Controller
{
    public IActionResult Index()
    {
        ViewBag.Url = configuration["UrlWebAPI"];
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ComprarAsync(string carritoJson)
    {
        if (string.IsNullOrWhiteSpace(carritoJson))
        {
            return BadRequest("Carrito vacío.");
        }

        ViewBag.Url = configuration["UrlWebAPI"];

        List<Carrito> carrito;
        try
        {
            carrito = JsonSerializer.Deserialize<List<Carrito>>(carritoJson) ?? new List<Carrito>();
        }
        catch (JsonException)
        {
            return BadRequest("Error al deserializar el carrito.");
        }

        if (carrito == null || !carrito.Any())
        {
            return BadRequest("Carrito inválido.");
        }

        try
        {
            var pedido = new
            {
                usuarioid = "bd65fbfe-95ff-4bbf-ba14-610738f2eef0", // este deberías obtenerlo del usuario autenticado
                productos = carrito.Select(p => new
                {
                    productoid = p.productoid,
                    cantidad = p.cantidad
                }).ToList()
            };

            await comprarClientService.EnviarPedidoAsync(pedido);
            TempData["Mensaje"] = "¡Compra realizada con éxito!";
            TempData["TipoMensaje"] = "success";
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            TempData["Mensaje"] = "Ocurrió un error al procesar la compra. Intenta más tarde.";
            TempData["TipoMensaje"] = "danger";
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View("index");
    }
}

