using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using To_do_list.Models;

namespace To_do_list.Controllers
{
    public class TareaFController : Controller
    {
        private readonly ToDoListDbContext _contexto;

        public TareaFController(ToDoListDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IActionResult> ListarFinalizado(string buscar)
        {
            //Obtener el id del usuario
            ClaimsPrincipal claimsUser = HttpContext.User;
            Usuario usuario = new Usuario();

            if (claimsUser.Identity.IsAuthenticated)
            {
                usuario.Id = (int)Convert.ToInt64(claimsUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                    .Select(c => c.Value).SingleOrDefault());
            }

            if (!String.IsNullOrEmpty(buscar))
            {
                var tareas = _contexto.Tareas.Where(e => e.IdUsuario == usuario.Id).Where(e => e.Finalizado == true)
                    .Where(e=> e.Nombre == buscar);
                return View(await tareas.ToListAsync());
            }
            else
            {
                var tareas = _contexto.Tareas.Where(e => e.IdUsuario == usuario.Id).Where(e => e.Finalizado == true);
                return View(await tareas.ToListAsync());
            }
        }

        public async Task<IActionResult> Pendiente(int id)
        {
            Tarea tarea = await _contexto.Tareas.FindAsync(id);

            if (tarea != null)
            {
                tarea.Finalizado = false;
                _contexto.SaveChangesAsync();
                return RedirectToAction("ListarFinalizado");
            }
            else
            {
                return RedirectToAction("ListarFinalizado");
            }
        }
    }
}
