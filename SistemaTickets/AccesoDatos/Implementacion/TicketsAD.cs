using System.Collections.Generic;
using System.Linq;
using Entidades;

namespace AccesoDatos.Implementacion
{
    public class TicketsAD
    {
        private STEntities db = new STEntities();

        public List<sp_ListarTickets_Result> Listar()
        {
            return db.sp_ListarTickets().ToList();
        }

        public List<sp_ListarTickets_Result> ListarPorUsuario(int usuarioId)
        {
            return db.sp_ListarTickets()
                     .Where(t => t.UsuarioId == usuarioId)
                     .ToList();
        }

        public sp_ObtenerTicketPorId_Result ObtenerPorId(int id)
        {
            return db.sp_ObtenerTicketPorId(id).FirstOrDefault();
        }

        public void Agregar(Tickets t)
        {
            db.sp_InsertarTicket(t.UsuarioId, t.Tipo, t.Descripcion, t.Categoria, t.Estado, t.Evidencia, t.SoportistaAsignadoId);

        }

        public void Editar(Tickets t)
        {
            db.sp_ActualizarTicket(t.Id, t.Tipo, t.Descripcion, t.Categoria, t.Estado, t.Evidencia, t.SoportistaAsignadoId);
        }

        public void Eliminar(int id)
        {
            db.sp_EliminarTicket(id);
        }

        public void MoverABitacora(int ticketId)
        {
            db.Database.ExecuteSqlCommand("EXEC sp_MoverTicketABitacora @Id = {0}", ticketId);
        }


        public List<sp_ListarBitacora_Result> ListarBitacora()
        {
            return db.sp_ListarBitacora().ToList();
        }

        public Tickets ObtenerEntidadPorId(int id)
        {
            return db.Tickets.FirstOrDefault(t => t.Id == id);
        }

    }
}
