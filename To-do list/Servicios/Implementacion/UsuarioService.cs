using Microsoft.EntityFrameworkCore;
using To_do_list.Models;
using To_do_list.Servicios.Contrato;

namespace To_do_list.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService //Añadir la herencia del contratro que se creó
    {
        private readonly ToDoListDbContext _dbContext;

        public UsuarioService(ToDoListDbContext dbContext) //Se crea el constructor
        {
            _dbContext = dbContext;
        }

        //Nos devuelve los métodos que estaban en la interfaz IUsuarioService
        public async Task<Usuario> GetUsuario(string correo, string contrasena)
        {
            Usuario usuarioEncontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Contrasena == contrasena)
                .FirstOrDefaultAsync();

            return usuarioEncontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario usuario)
        {
            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();

            return usuario;
        }
    }
}
