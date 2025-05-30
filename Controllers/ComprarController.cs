using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

[Authorize(Roles = "Usuario")]
public class ComprarController(ProductosClientService productos, ComprarClientService comprar, IConfiguration configuration) : Controller
{
    public async Task<IActionResult> Index(string? s)
    {
        List<Producto>? lista = [];
        try
        {
           lista = await productos.GetAsync(s);
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

    [HttpPost]
    public async Task<IActionResult> Comprar(int productoId, int cantidad)
    {
        ViewBag.Url = configuration["UrlWebAPI"];

        try
        {

            var pedido = new
            {
                usuarioid = "bd65fbfe-95ff-4bbf-ba14-610738f2eef0",
                productos = new[]
                {
                    new { productoid = productoId, cantidad = cantidad }
                }
            };

            await comprar.EnviarPedidoAsync(pedido);
            return RedirectToAction(nameof(Index));

        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Salir", "Auth");
        }
        
        ModelState.AddModelError("Nombre", "No ha sido posible realizar la acción. Inténtelo nuevamente.");
        return View(null);
    }
}
