using System.Net;
using System.Net.Mail;

namespace LogicaNegocio
{
    public class CorreoHelper
    {
        public static void EnviarCorreo(string destinatario, string asunto, string cuerpo)
        {
            var mensaje = new MailMessage();
            mensaje.To.Add(destinatario);
            mensaje.Subject = asunto;
            mensaje.Body = cuerpo;
            mensaje.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.Send(mensaje);
            }
        }
    }
}
