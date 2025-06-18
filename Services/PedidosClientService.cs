using System.Text;
using System.Text.Json;
using frontendnet.Models;

namespace frontendnet.Services;

public class PedidosClientService(HttpClient client)
{
    public async Task<List<Pedido>?> ObtenerPedidos(string? email)
    {
        return await client.GetFromJsonAsync<List<Pedido>>($"api/pedidos/{email}");
    }

    public async Task<DetallePedido?> ObtenerDetallesPedido(int? idPedido)
    {
        return await client.GetFromJsonAsync<DetallePedido>($"api/pedidos/detalles/{idPedido}");
    }
}
