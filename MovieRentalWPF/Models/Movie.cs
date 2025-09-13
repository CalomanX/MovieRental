using System.ComponentModel.DataAnnotations;

namespace MovieRentalWPF.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}