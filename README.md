# Sistema de Gestion de Tickets

Aplicacion web desarrollada en **ASP.NET MVC** con **SQL Server** para la gestion de tickets de soporte en una empresa.  
Este sistema permite a los usuarios reportar problemas de **hardware, software o redes**, y da seguimiento al ciclo de vida de cada ticket.

##  Funcionalidades principales
- **Gestion de usuarios y roles**: Usuario, Soportista y Supervisor.
- **Creacion de tickets** con datos del usuario, tipo de problema y evidencia (imagen).
- **Asignacion de soportistas** y categorias de prioridad (Bajo, Medio, Alto).
- **Seguimiento de tickets**: cambios de estado (Abierto, En revision, En proceso, Finalizado).
- **Bitacora de tickets finalizados** (historial sin eliminar registros).
- **Reportes** por estado, tipo, usuario, soportista y categoria.
- **Notificaciones por correo** al agregar seguimientos.
- Consola de administracion y visualizacion de casos de usuarios.

## Tecnologias utilizadas
- ASP.NET MVC (.NET Framework)
- SQL Server (procedimientos almacenados y ADO.NET Entity Data Model)
- HTML, CSS y Bootstrap
- C#

##  Estructura del sistema
- **Models/** Entidades y conexion a base de datos.  
- **Controllers/**  Logica de negocio y controladores de tickets, usuarios y seguimientos.  
- **Views/**  Vistas con Razor para gestion y reporteria.  
- **Database/**  Scripts SQL de creacion de tablas, roles y procedimientos almacenados.  



##  Estado del proyecto
Proyecto academico funcional, disenado como sistema de soporte tecnico con reporteria y control de tickets.

---

 Desarrollado por **Daniel Rodríguez**

