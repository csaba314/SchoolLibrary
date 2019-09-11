using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class Genre : IGenre
    {
        public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
    }
}
