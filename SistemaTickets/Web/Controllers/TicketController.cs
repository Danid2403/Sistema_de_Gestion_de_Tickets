using Entidades;
using LogicaNegocio.Implementacion;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketsLN ln = new TicketsLN();

        // Validar sesión activa antes de cada acción
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        // Vista principal de tickets, con filtros aplicables a Supervisor y Soportista
        public ActionResult Index(string estado, string tipo, string categoria, int? usuarioId, int? soportistaId)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];

            // Filtros para los dropdowns
            ViewBag.Estados = new SelectList(new[] { "Abierto", "En revisión", "En proceso", "Finalizado" });
            ViewBag.Tipos = new SelectList(new[] { "Redes", "Software", "Hardware" });
            ViewBag.Categorias = new SelectList(new[] { "Bajo", "Medio", "Alto" });

            var usuariosLN = new UsuariosLN();
            var usuarios = usuariosLN.Listar();
            ViewBag.Usuarios = new SelectList(usuarios, "Id", "Nombre");
            ViewBag.Soportistas = new SelectList(usuarios.Where(u => u.Rol == "Soportista"), "Id", "Nombre");

            // Lógica para filtrar los tickets
            var tickets = usuario.Rol == "Usuario" ? ln.ListarPorUsuario(usuario.Id) : ln.Listar();

            if (!string.IsNullOrEmpty(estado) && estado != "Todos")
                tickets = tickets.Where(t => t.Estado == estado).ToList();

            if (!string.IsNullOrEmpty(tipo) && tipo != "Todos")
                tickets = tickets.Where(t => t.Tipo == tipo).ToList();

            if (!string.IsNullOrEmpty(categoria) && categoria != "Todos")
                tickets = tickets.Where(t => t.Categoria == categoria).ToList();

            if (usuarioId.HasValue)
                tickets = tickets.Where(t => t.UsuarioId == usuarioId.Value).ToList();

            if (soportistaId.HasValue)
                tickets = tickets.Where(t => t.SoportistaAsignadoId == soportistaId.Value).ToList();

            return View(tickets);
        }





        public ActionResult Details(int id)
        {
            var ticket = ln.ObtenerPorId(id);
            if (ticket == null)
                return HttpNotFound();

            var seguimientosLN = new SeguimientosLN();
            ViewBag.Seguimientos = seguimientosLN.ListarPorTicket(id);

            return View(ticket);
        }

        public ActionResult Create()
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            if (usuario.Rol != "Usuario")
                return new HttpStatusCodeResult(403);

            CargarSoportistas();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tickets t, HttpPostedFileBase evidenciaArchivo)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];

            if (usuario.Rol != "Usuario")
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                t.UsuarioId = usuario.Id;
                t.Estado = "Abierto";
                t.FechaIngreso = DateTime.Now;

                if (evidenciaArchivo != null && evidenciaArchivo.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(evidenciaArchivo.InputStream))
                    {
                        t.Evidencia = reader.ReadBytes(evidenciaArchivo.ContentLength);
                    }
                }

                ln.Agregar(t);
                return RedirectToAction("Index");
            }

            CargarSoportistas();
            return View(t);
        }

        public ActionResult Edit(int id)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            var ticket = ln.ObtenerEntidadPorId(id);

            if (ticket == null)
                return HttpNotFound();

            if (usuario.Rol == "Usuario" && ticket.UsuarioId != usuario.Id)
                return new HttpStatusCodeResult(403);

            CargarSoportistas();
            return View(ticket);
        }

        [HttpPost]
        public ActionResult Edit(Tickets t, HttpPostedFileBase evidenciaArchivo)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];

            if (usuario.Rol == "Usuario" && t.UsuarioId != usuario.Id)
                return new HttpStatusCodeResult(403);

            if (ModelState.IsValid)
            {
                if (evidenciaArchivo != null && evidenciaArchivo.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(evidenciaArchivo.InputStream))
                    {
                        t.Evidencia = reader.ReadBytes(evidenciaArchivo.ContentLength);
                    }
                }

                if (t.Estado == "Finalizado" && (usuario.Rol == "Soportista" || usuario.Rol == "Supervisor"))
                {
                    ln.Editar(t);
                    ln.MoverABitacora(t.Id);
                }
                else
                {
                    ln.Editar(t);
                }

                return RedirectToAction("Index");
            }

            CargarSoportistas();
            return View(t);
        }

        public ActionResult Delete(int id)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            var ticket = ln.ObtenerPorId(id);
            if (ticket == null)
                return HttpNotFound();

            if (usuario.Rol != "Supervisor" && usuario.Rol != "Soportista")
                return new HttpStatusCodeResult(403);

            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            if (usuario.Rol != "Supervisor" && usuario.Rol != "Soportista")
                return new HttpStatusCodeResult(403);

            ln.Eliminar(id);
            return RedirectToAction("Index");
        }

        public ActionResult Bitacora()
        {
            var usuario = (sp_LoginUsuario_Result)Session["Usuario"];
            if (usuario.Rol != "Supervisor" && usuario.Rol != "Soportista")
                return new HttpStatusCodeResult(403);

            var bitacora = ln.ListarBitacora();
            return View(bitacora);
        }

        private void CargarSoportistas()
        {
            var usuariosLN = new UsuariosLN();
            var soportistas = usuariosLN.Listar().Where(u => u.Rol == "Soportista").ToList();
            ViewBag.Soportistas = new SelectList(soportistas, "Id", "Nombre");
        }
        public FileContentResult MostrarEvidencia(int id)
        {
            var ticket = ln.ObtenerPorId(id);
            if (ticket != null && ticket.Evidencia != null)
            {
                return File(ticket.Evidencia, "image/png"); // o "image/png" "image/jpeg"
            }
            return null;
        }

    }
}
