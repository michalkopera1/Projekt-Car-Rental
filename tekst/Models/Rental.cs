using System.ComponentModel.DataAnnotations;

namespace tekst.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int CarId { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime ReturnDate { get; set; }

       
        public Customer? Customer { get; set; } 
        public Car? Car { get; set; }           
    }

}
