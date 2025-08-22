using System.Collections.Generic;
using AccesoDatos.Implementacion;
using Entidades;

namespace LogicaNegocio.Implementacion
{
    public class SeguimientosLN
    {
        private SeguimientosAD ad = new SeguimientosAD();

        public List<sp_ListarSeguimientosPorTicket_Result> ListarPorTicket(int ticketId) => ad.ListarPorTicket(ticketId);

        public void Agregar(Seguimientos s) => ad.Agregar(s);
    }
}
