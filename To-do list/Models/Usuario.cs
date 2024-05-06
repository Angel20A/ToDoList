using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace To_do_list.Models;

public partial class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    [Required]
    public string Apellido { get; set; } = null!;

    [Required]
    public string Correo { get; set; } = null!;

    [Required]
    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
