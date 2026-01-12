using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecursosHumanos.Models.ViewModels;

using RecursosHumanos.Models;
using Microsoft.IdentityModel.Abstractions;

namespace RecursosHumanos.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ApplicationDbContext _context;

        //Constructor
        public CuentaController(ApplicationDbContext context) 
        {

            _context = context;
        }

        [HttpGet]
        public IActionResult Login() 
        {

            return View();
        
        }

        [HttpPost]
        public IActionResult Login(LoginViewModelscs model) 
        {
            if (ModelState.IsValid)
            {
                //Creando usuario con la propiedade _context
                var usuario = _context.Usuarios
                    .FirstOrDefault(u=> u.Usuario1== model.Username);

                //Se checa no sea nulo el dato
                if (usuario != null && usuario.Contraseña ==model.Password) 
                {
                    //Se establece la cookie
                    CookieOptions options = new CookieOptions
                    {
                        Expires = model.RememberMe ? DateTimeOffset.UtcNow : DateTimeOffset.UtcNow,
                        HttpOnly = true, //Evita haceso desde javascript
                        Secure = true //Solo por HTTPS

                    };

                    Response.Cookies.Append("UsuarioLogueado",usuario.Usuario1,options);

                    return RedirectToAction("Index","Home");
                }

                ModelState.AddModelError("","Usuario o contraseña incorrecta");
            }
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //Se elimina la cookie
        public IActionResult Logout() 
        {
            //Elimina la cookie del navegador
            Response.Cookies.Delete("UsuarioLogueado");

            //Redirigir a Login
            return RedirectToAction("Login");
        }

    }
}
