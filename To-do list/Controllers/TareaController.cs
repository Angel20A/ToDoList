using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;
using To_do_list.Models;
using To_do_list.Models.ViewModel;

namespace To_do_list.Controllers
{
    public class TareaController : Controller
    {
        private readonly IWebHostEnvironment _enviroment;

        public TareaController(IWebHostEnvironment env)
        {
            _enviroment = env;
        }

        public async Task<IActionResult> Listar(string buscar )
        {
            using (var tarea = new ToDoListDbContext())
            {
                //Obtener el id del usuario con Claims
                ClaimsPrincipal claimuser = HttpContext.User;
                Usuario usuario = new Usuario();

                if (claimuser.Identity.IsAuthenticated)
                {
                    usuario.Id = (int)Convert.ToInt64(claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                        .Select(c => c.Value).SingleOrDefault());
                }

                if (!String.IsNullOrEmpty(buscar))
                {
                    var tareas = tarea.Tareas.Where(e => e.IdUsuario == usuario.Id).Where(e => e.Finalizado == false)
                        .Where(e => e.Nombre == buscar);
                    return View(await tareas.ToListAsync());
                }
                else
                {
                    var tareas = tarea.Tareas.Where(e => e.IdUsuario == usuario.Id).Where(e => e.Finalizado == false);
                    return View(await tareas.ToListAsync());
                }
            }
        }


        public IActionResult Guardar()
        {
            ViewBag.message = TempData["message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(TareaModel upload)
        {
            using (var tareaContexto = new ToDoListDbContext())
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    var tarea = new Tarea();
                    tarea.Nombre = upload.Nombre;
                    tarea.FechaVencimiento = upload.FechaVencimiento;
                    tarea.HoraVencimiento = upload.HoraVencimiento;
                    tarea.Descripcion = upload.Descripcion;

                    //Guargar el archivo
                    if (upload.Archivo != null)
                    {
                        await upload.Archivo.CopyToAsync(ms);
                        tarea.Archivo = ms.ToArray();
                        tarea.Finalizado = false;
                    }
                    
                    //Obtener el id del usuario con Claims
                    ClaimsPrincipal claimuser = HttpContext.User;
                    if (claimuser.Identity.IsAuthenticated)
                    {
                        upload.IdUsuario = (int)Convert.ToInt64(claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                            .Select(c => c.Value).SingleOrDefault());
                    }
                    tarea.IdUsuario = upload.IdUsuario;

                    tareaContexto.Tareas.Add(tarea);
                    tareaContexto.SaveChanges();
                }
            }

            TempData["messsage"] = "Archivo arriba";
            return RedirectToAction("Listar");
        }

        public async Task<IActionResult> Modificar(int id)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                Tarea tarea = await tareacontexto.Tareas.FindAsync(id);

                TareaModel tareamodel = new TareaModel();
                tareamodel.idTarea = tarea.IdTarea;
                tareamodel.Nombre = tarea.Nombre;
                tareamodel.FechaVencimiento = tarea.FechaVencimiento;
                tareamodel.HoraVencimiento = tarea.HoraVencimiento;
                tareamodel.Descripcion = tarea.Descripcion;
                if(tarea.Archivo != null)
                {
                    var ms = new System.IO.MemoryStream(tarea.Archivo);
                    IFormFile archivo = new FormFile(ms, 0, tarea.Archivo.Length, "archivo", "archivo.png");
                    tareamodel.Archivo = archivo;
                }
                tareamodel.Finalizado = tarea.Finalizado;
                tareamodel.IdUsuario = tarea.IdUsuario;

                ViewBag.message = TempData["Message"];
                return View(tareamodel);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Modificar(TareaModel tarea)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                var tareaActualizada = await tareacontexto.Tareas.FindAsync(tarea.idTarea);

                tareaActualizada.Nombre = tarea.Nombre;
                tareaActualizada.FechaVencimiento = tarea.FechaVencimiento;
                tareaActualizada.HoraVencimiento = tarea.HoraVencimiento;
                tareaActualizada.Descripcion = tarea.Descripcion;

                if (tarea.Archivo != null)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        await tarea.Archivo.CopyToAsync(ms);
                        tareaActualizada.Archivo = ms.ToArray();
                    }
                }
                tareaActualizada.Finalizado = tarea.Finalizado;
                tareaActualizada.IdUsuario = tarea.IdUsuario;

                tareacontexto.SaveChangesAsync();

                return RedirectToAction("Listar");
            }
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                Tarea tarea = await tareacontexto.Tareas.FindAsync(id);
                return View(tarea);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar(Tarea tarea)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                var tareaeliminar = await tareacontexto.Tareas.FindAsync(tarea.IdTarea);

                if (tareaeliminar == null)
                {
                    return NotFound();
                }

                tareacontexto.Tareas.Remove(tareaeliminar);
                await tareacontexto.SaveChangesAsync();
                return RedirectToAction("Listar");
            }
        }

        
        public async Task<IActionResult> Ver(int id)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {                
                var tarea = await tareacontexto.Tareas.FindAsync(id);

                if(tarea != null)
                {
                    TareaModel tareamodelo = new TareaModel();
                    tareamodelo.idTarea = tarea.IdTarea;
                    tareamodelo.Nombre = tarea.Nombre;
                    tareamodelo.FechaVencimiento = tarea.FechaVencimiento;
                    tareamodelo.HoraVencimiento = tarea.HoraVencimiento;
                    tareamodelo.Descripcion = tarea.Descripcion;
                    if (tarea.Archivo != null)
                    {
                        var ms = new System.IO.MemoryStream(tarea.Archivo);
                        IFormFile archivo = new FormFile(ms, 0, tarea.Archivo.Length, "archivo", "descarga.png");
                        tareamodelo.Archivo = archivo;
                    }
                    tareamodelo.Finalizado = tarea.Finalizado;
                    tareamodelo.IdUsuario = tarea.IdUsuario;

                    return View("Ver", model: tareamodelo);
                }
                else
                {
                    return RedirectToAction("Listar");
                }
            }
        }
        
        public async Task<IActionResult> DescargarArchivo(int id)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                var tarea = await tareacontexto.Tareas.FindAsync(id);
                return File(tarea.Archivo, "image/png", fileDownloadName: "descarga.png");
            }
        }

        public async Task<IActionResult> Finalizar(int id)
        {
            using (var tareacontexto = new ToDoListDbContext())
            {
                Tarea tarea = await tareacontexto.Tareas.FindAsync(id);

                if (tarea != null)
                {
                    tarea.Finalizado = true;
                    tareacontexto.SaveChangesAsync();
                    return RedirectToAction("Listar");
                }
                else
                {
                    return RedirectToAction("Listar");
                }
            }
        }
    }
}