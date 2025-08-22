using Entidades;
using LogicaNegocio.Implementacion;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosLN ln = new UsuariosLN();

        // Verificación de sesión antes de cada acción
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var actionName = filterContext.ActionDescriptor.ActionName.ToLower();

            // Permitir solo Index sin sesión (opcional)
            if (actionName == "index")
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

        // GET: Usuarios
        public ActionResult Index()
        {
            var todos = ln.Listar();

            ViewBag.Activos = todos.Where(u => u.Activo == true).ToList();
            ViewBag.Inactivos = todos.Where(u => u.Activo == false).ToList();

            return View();
        }







        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            var usuario = ln.ObtenerPorId(id);
            if (usuario == null)
                return HttpNotFound();
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            var usuario = Session["Usuario"] as sp_LoginUsuario_Result;
            if (usuario == null || usuario.Rol != "Supervisor")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuarios u)
        {
            var usuario = Session["Usuario"] as sp_LoginUsuario_Result;
            if (usuario == null || usuario.Rol != "Supervisor")
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                ln.Agregar(u);
                return RedirectToAction("Index");
            }
            return View(u);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var usuario = ln.ObtenerPorId(id.Value);
            if (usuario == null)
                return HttpNotFound();

            var model = new Usuarios
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Cedula = usuario.Cedula,
                Departamento = usuario.Departamento,
                Contacto = usuario.Contacto,
                Correo = usuario.Correo,
                Tipo = usuario.Tipo,
                Rol = usuario.Rol,
                Activo = usuario.Activo
            };

            return View(model);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                ln.Editar(u);
                return RedirectToAction("Index");
            }
            return View(u);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = ln.ObtenerPorId(id);
            if (usuario == null)
                return HttpNotFound();
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            if (usuario.Rol != "Supervisor")
                return new HttpStatusCodeResult(403);

            ln.Eliminar(id);
            return RedirectToAction("Index");
        }

    }
}
