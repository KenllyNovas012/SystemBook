using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Services.Modelo
{
    public class Book
    {
        public int Id { get; set; }        

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El número de páginas es obligatorio.")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "El extracto es obligatorio.")]
        public string Excerpt { get; set; }

        [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
        public DateTime PublishDate { get; set; }
    }
}
