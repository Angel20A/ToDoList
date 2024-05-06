using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace To_do_list.Models;

public partial class Tarea
{
    [Key]
    public int IdTarea { get; set; }

    [Required(ErrorMessage ="El campo nombre es obligatorio.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La fecha de vencimiento es obligatorio.")]
    [Display(Name = "Fecha de vencimiento")]
    public DateTime FechaVencimiento { get; set; }

    [Required(ErrorMessage ="La hora de vencimiento es obligatorio.")]
    [Display(Name ="Hora de vencimiento")]
    public TimeSpan HoraVencimiento { get; set; }

    [MaxLength(ErrorMessage = "El número de carácteres permitidos es de 300.")]
    [Display(Name ="Descripción")]
    public string? Descripcion { get; set; }

    [Display(Name ="Adjuntar archivo")]
    public byte[]? Archivo { get; set; }

    public bool Finalizado { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
