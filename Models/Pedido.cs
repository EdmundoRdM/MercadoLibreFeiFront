using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Models
{
    public class Pedido
    {
        [Display(Name = "Id")]
        public int? Id { get; set; }

        [Display(Name = "Usuario")]
        public string? UsuarioId { get; set; }

        [Display(Name = "Fecha del Pedido")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Costo Total")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.?\d{0,2}$", ErrorMessage = "El valor debe ser un precio válido.")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? CostoTotal { get; set; }

        [Display(Name = "Número de Productos")]
        public int? NumeroProductos { get; set; }

        public List<Producto>? Productos { get; set; }
    }
}
