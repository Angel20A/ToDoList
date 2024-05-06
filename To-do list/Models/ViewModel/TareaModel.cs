using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace To_do_list.Models.ViewModel
{
    public class TareaModel
    {
        [Key]
        public int idTarea {  get; set; }
        
        [Required (ErrorMessage = "El campo Nombre obligatorio.")]
        [MaxLength (100)]
        public string Nombre {  get; set; }

        [Required(ErrorMessage ="La fecha de vencimiento es obligatoria.")]
        [Display(Name = "Fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        [Required(ErrorMessage ="La hora de vencimiento es obligatorio.")]
        [Display(Name = "Hora de vencimiento")]
        public TimeSpan HoraVencimiento { get; set; }

        [MaxLength(ErrorMessage ="El número de carácteres permitidos es de 300.")]
        [Display(Name ="Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name ="Adjuntar archivo")]
        public IFormFile? Archivo { get; set; }

        public bool Finalizado { get; set; }

        public int IdUsuario { get; set; }

    }
}
