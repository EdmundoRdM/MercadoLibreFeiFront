using frontendnet.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace frontendnet.Services;

public class AuthClientService(HttpClient client, IHttpContextAccessor httpContextAccessor)
{
    public async Task<AuthUser> ObtenTokenAsync(string email, string password)
    {
        Login usuario = new() { Email = email, Password = password };
        // Realizo la llamada al Web API
        var response = await client.PostAsJsonAsync("api/auth", usuario);
        var token = await response.Content.ReadFromJsonAsync<AuthUser>();

        return token!;
    }

    public async Task IniciaSesionAsync(List<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();
        await httpContextAccessor.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties)!;
    }

    public async Task<string?> RefreshTokenAsync()
    {
        try
        {
            // Envía la cookie automáticamente (HttpClient maneja cookies)
            var response = await client.PostAsync("/auth/refresh", null); 

            if (response.IsSuccessStatusCode)
            {
                // Deserializa la respuesta
                var result = await response.Content.ReadFromJsonAsync<RefreshResponse>();
                return result?.AccessToken;
            }
        }
        catch (Exception ex)
        {
            // Log del error (opcional)
            Console.WriteLine($"Error refreshing token: {ex.Message}");
        }
        return null;
    }
}