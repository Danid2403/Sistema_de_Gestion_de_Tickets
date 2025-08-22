using System.Collections.Generic;
using System.Linq;
using Entidades;

namespace AccesoDatos.Implementacion
{
    public class UsuariosAD
    {
        private STEntities db = new STEntities();

        public sp_LoginUsuario_Result Login(string correo, string contrasena)
        {
            using (var db = new STEntities())
            {
                return db.sp_LoginUsuario(correo, contrasena).FirstOrDefault();
            }
        }


        public List<sp_ListarUsuarios_Result> Listar()
        {
            return db.sp_ListarUsuarios().ToList();
        }

        public sp_ObtenerUsuarioPorId_Result ObtenerPorId(int id)
        {
            return db.sp_ObtenerUsuarioPorId(id).FirstOrDefault();
        }

        public void Agregar(Usuarios u)
        {
            db.sp_InsertarUsuario(u.Nombre, u.Cedula, u.Departamento, u.Contacto, u.Correo, u.Tipo, u.Rol, u.Activo, u.Contrasena);
        }

        public void Editar(Usuarios u)
        {
            db.sp_ActualizarUsuario(u.Id, u.Nombre, u.Cedula, u.Departamento, u.Contacto, u.Correo, u.Tipo, u.Rol, u.Activo);
        }

        public void Eliminar(int id)
        {
            db.sp_EliminarUsuario(id);
        }
    }
}
