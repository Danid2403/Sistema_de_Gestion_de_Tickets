using Entidades;
using LogicaNegocio.Implementacion;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SeguimientosController : Controller
    {
        private readonly SeguimientosLN ln = new SeguimientosLN();
        private readonly TicketsLN ticketsLN = new TicketsLN(); // Necesario para obtener el correo del usuario

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarSeguimiento(int TicketId, string Descripcion)
        {
            if (string.IsNullOrWhiteSpace(Descripcion))
            {
                TempData["Error"] = "Debe ingresar una descripción.";
                return RedirectToAction("Details", "Tickets", new { id = TicketId });
            }

            var seguimiento = new Seguimientos
            {
                TicketId = TicketId,
                Descripcion = Descripcion
            };

            ln.Agregar(seguimiento);

            // ✅ Obtener el ticket para enviar el correo al dueño
            var ticket = ticketsLN.ObtenerPorId(TicketId);
            if (ticket != null && !string.IsNullOrEmpty(ticket.CorreoUsuario))
            {
                var enviado = EnviarCorreo(ticket.CorreoUsuario, ticket.NombreUsuario, Descripcion, TicketId);
                if (enviado)
                    TempData["CorreoExito"] = "Correo enviado correctamente al usuario.";
                else
                    TempData["CorreoError"] = "El seguimiento se agregó, pero no se pudo enviar el correo.";
            }

            TempData["Exito"] = "Seguimiento agregado correctamente.";
            return RedirectToAction("Details", "Tickets", new { id = TicketId });
        }

        // Método para enviar el correo
        private bool EnviarCorreo(string correoDestino, string nombreUsuario, string descripcion, int ticketId)
        {
            var remitente = "daniel030431@gmail.com";
            var asunto = $"Actualización de seguimiento - Ticket #{ticketId}";
            var cuerpo = $@"
                <h3>Hola {nombreUsuario},</h3>
                <p>Se ha agregado un nuevo seguimiento a tu ticket <strong>#{ticketId}</strong>.</p>
                <p><strong>Descripción:</strong></p>
                <p>{descripcion}</p>
                <br />
                <p>Gracias,</p>
                <p>Equipo de soporte</p>";

            var mensaje = new MailMessage(remitente, correoDestino)
            {
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = true
            };

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(remitente, "qwkz cxno ktfp izgy") // tu contraseña o clave de aplicación
            };

            try
            {
                smtp.Send(mensaje);
                return true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al enviar correo: " + ex.Message);
                return false;
            }
        }
    }
}
