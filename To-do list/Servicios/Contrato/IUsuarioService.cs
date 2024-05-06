using Microsoft.EntityFrameworkCore;
using To_do_list.Models;

namespace To_do_list.Servicios.Contrato
{
    public interface IUsuarioService //Interfaz
    {
        //Declarar los métodos que se van a estar utilizando

        Task<Usuario> GetUsuario(string correo, string contrasena); //Va a devolver un usuario a través de un correo y una contraseña

        Task<Usuario> SaveUsuario(Usuario usuario); //Guardar un usuario
    }
}
