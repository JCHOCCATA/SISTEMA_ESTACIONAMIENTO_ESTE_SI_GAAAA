using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEstacionamiento.Services;
using System.Security.Claims;

namespace SistemaEstacionamiento.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService _authService;

        public LoginController(LoginService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarLogin([FromForm] string username, [FromForm] string password)
        {
            var usuarioLogueado = await _authService.IniciarSesion(username, password);

            if (usuarioLogueado != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuarioLogueado.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, usuarioLogueado.Username),
                    new Claim("SedeId", usuarioLogueado.SedeId.ToString()),
                    new Claim("SedeNombre", usuarioLogueado.SedeNombre)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                });

                return Json(new { success = true, message = "¡Bienvenido al sistema!", redirectUrl = Url.Action("Index", "Home") });
            }

            return Json(new { success = false, message = "Usuario o contraseña incorrectos" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }


    }
}
