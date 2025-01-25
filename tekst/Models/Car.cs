using System.ComponentModel.DataAnnotations;

namespace tekst.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Marka jest wymagana")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model jest wymagany")]
        public string Model { get; set; }

        [Range(1900, 2100, ErrorMessage = "Rok musi być w przedziale 1900-2100")]
        public int Year { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Cena musi być większa od 0")]
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }

    }
}