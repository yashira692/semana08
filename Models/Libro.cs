using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "El título debe tener entre 2 y 150 caracteres")]
    [DataType(DataType.Text)]
    public string Titulo { get; set; } = string.Empty;

    [Display(Name = "Autor")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un autor")]
    public int AutorId { get; set; }

    [Display(Name = "Autor")]
    public string? AutorNombre { get; set; }

    [Range(1, 1000, ErrorMessage = "Los ejemplares deben estar entre 1 y 1000")]
    public int Ejemplares { get; set; } = 1;

    public bool Activo { get; set; }
}