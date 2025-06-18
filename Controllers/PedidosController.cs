using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class PedidosController(PedidosClientService pedidos, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Pedido>? lista = [];
        try
        {
            lista = await pedidos.ObtenerPedidos(User.Identity.Name);
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }

        ViewBag.Url = configuration["UrlWebAPI"];
        ViewBag.search = s;
        return View(lista);
    }

    public async Task<IActionResult> Detalle(int id)
    {
        DetallePedido? item = null;
        try
        {
            item = await pedidos.ObtenerDetallesPedido(id);
            if (item == null) return NotFound();
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        return View(item);
    }
}
