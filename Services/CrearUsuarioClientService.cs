using frontendnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace frontendnet.Services;

public class CrearUsuarioClientService(HttpClient client)
{
    public async Task<bool> RegistrarUsuarioAsync(CrearUsuario model)
    {

        if (model.Password != model.ConfirmPassword)
            throw new Exception("Las contraseñas no coinciden");

        var datos = new
        {
            email = model.Email,
            nombre = model.Nombre,
            password = model.Password
        };

        var response = await client.PostAsJsonAsync("api/usuarios/clienteNuevo", datos);
        return response.IsSuccessStatusCode;
    }
}
