using frontendnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace frontendnet.Services;

public class ComprarClientService(HttpClient client)
{
    public async Task EnviarPedidoAsync(object pedido)
    {
        var json = JsonSerializer.Serialize(pedido);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/pedidos/", content);
        response.EnsureSuccessStatusCode();
    }
}
