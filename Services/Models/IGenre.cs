using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public interface IGenre
    {
        int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        string Name { get; set; }
    }
}
