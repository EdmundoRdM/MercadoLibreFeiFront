using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class AuthController(AuthClientService auth, CrearUsuarioClientService usuarioService) : Controller
{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync(Login model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Esta función verifica en backend que el correo y contraseña sean válidos
                var token = await auth.ObtenTokenAsync(model.Email, model.Password);
                TempData["EmailRegistrado"] = model.Email;
                
                var claims = new List<Claim>
                {
                    // Todo esto se guarda en la Cookie
                    new(ClaimTypes.Name, token.Email),
                    new(ClaimTypes.GivenName, token.Nombre),
                    new("jwt", token.Jwt),
                    new(ClaimTypes.Role, token.Rol),
                };
                await auth.IniciaSesionAsync(claims);
                if (token.Rol == "Administrador")
                    return RedirectToAction("Index", "Productos");
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "credenciales no válidas. Inténtelo nuevamente.");
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult CrearUsuario()
    {
        return View("CrearUsuario"); // Carga la vista de registro
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearUsuario(CrearUsuario model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var resultado = await usuarioService.RegistrarUsuarioAsync(model);

                if (resultado)
                    return RedirectToAction("Index"); // Redirige al login

                ModelState.AddModelError(string.Empty, "No se pudo registrar el usuario.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        return View(model);
    }

    [Authorize(Roles = "Administrador, Usuario")]
    public async Task<IActionResult> SalirAsync()
    {
        // Cierra la sesión
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // Sino, se redirige a la página inicial
        return RedirectToAction("Index", "Auth");
    }
    
}