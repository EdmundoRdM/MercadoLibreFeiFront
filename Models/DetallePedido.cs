using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Models
{
    public class DetallePedido
    {
        public Pedido? Pedido { get; set; }
        public List<Producto>? Productos { get; set; }

    }
}
