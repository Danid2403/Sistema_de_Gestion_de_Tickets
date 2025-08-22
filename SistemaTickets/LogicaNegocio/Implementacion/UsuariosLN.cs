using System.Collections.Generic;
using AccesoDatos.Implementacion;
using Entidades;

namespace LogicaNegocio.Implementacion
{
    public class UsuariosLN
    {
        private UsuariosAD ad = new UsuariosAD();

        public sp_LoginUsuario_Result Login(string correo, string contrasena)
        {
            return new UsuariosAD().Login(correo, contrasena);
        }


        public List<sp_ListarUsuarios_Result> Listar()
        {
            return ad.Listar();
        }

        public sp_ObtenerUsuarioPorId_Result ObtenerPorId(int id)
        {
            return ad.ObtenerPorId(id);
        }

        public void Agregar(Usuarios u)
        {
            ad.Agregar(u);
        }

        public void Editar(Usuarios u)
        {
            ad.Editar(u);
        }

        public void Eliminar(int id)
        {
            ad.Eliminar(id);
        }
    }
}
