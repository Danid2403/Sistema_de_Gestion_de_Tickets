using System.Web.Mvc;
using Entidades;
using LogicaNegocio.Implementacion;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuariosLN ln = new UsuariosLN();

        // Permitir acceso a login sin requerir sesión
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Evita que se bloquee el acceso a la pantalla de login
            var action = filterContext.ActionDescriptor.ActionName.ToLower();
            if (action == "index" || action == "cerrarsesion")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(string correo, string contrasena)
        {
            var usuario = ln.Login(correo, contrasena);

            if (usuario != null)
            {
                Session["Usuario"] = usuario;

                // Redirigir según rol
                switch (usuario.Rol)
                {
                    case "Usuario":
                        return RedirectToAction("Create", "Tickets"); // Para solicitar ticket
                    case "Soportista":
                    case "Supervisor":
                        return RedirectToAction("Index", "Tickets"); // Ver todos los tickets
                    default:
                        return RedirectToAction("Index", "Home"); // Por si acaso
                }
            }

            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
