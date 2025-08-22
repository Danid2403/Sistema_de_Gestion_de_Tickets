using System.Collections.Generic;
using AccesoDatos.Implementacion;
using Entidades;

namespace LogicaNegocio.Implementacion
{
    public class TicketsLN
    {
        private readonly TicketsAD ad = new TicketsAD();

        public List<sp_ListarTickets_Result> Listar()
        {
            return ad.Listar();
        }

        public List<sp_ListarTickets_Result> ListarPorUsuario(int usuarioId)
        {
            return ad.ListarPorUsuario(usuarioId);
        }

        public sp_ObtenerTicketPorId_Result ObtenerPorId(int id)
        {
            return ad.ObtenerPorId(id);
        }

        public void Agregar(Tickets t)
        {
            ad.Agregar(t);
        }

        public void Editar(Tickets t)
        {
            ad.Editar(t);
        }

        public void Eliminar(int id)
        {
            ad.Eliminar(id);
        }

        public void MoverABitacora(int ticketId)
        {
            ad.MoverABitacora(ticketId);
        }
        public List<sp_ListarBitacora_Result> ListarBitacora()
        {
            return ad.ListarBitacora();
        }

        public Tickets ObtenerEntidadPorId(int id)
        {
            return ad.ObtenerEntidadPorId(id);
        }


    }
}
