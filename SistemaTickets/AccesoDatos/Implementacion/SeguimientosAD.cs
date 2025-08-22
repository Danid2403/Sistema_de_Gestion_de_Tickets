using System.Collections.Generic;
using System.Linq;
using Entidades;

namespace AccesoDatos.Implementacion
{
    public class SeguimientosAD
    {
        private STEntities db = new STEntities();

        public List<sp_ListarSeguimientosPorTicket_Result> ListarPorTicket(int ticketId)
        {
            return db.sp_ListarSeguimientosPorTicket(ticketId).ToList();
        }

        public void Agregar(Seguimientos s)
        {
            db.sp_InsertarSeguimiento(s.TicketId, s.Descripcion);
        }
    }
}
