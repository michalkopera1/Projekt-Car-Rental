using System.ComponentModel.DataAnnotations;

namespace tekst.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Imie jest wymagane")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasRentedCar { get; set; }
    }
}
