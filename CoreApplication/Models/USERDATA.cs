using System.ComponentModel.DataAnnotations;

namespace CoreApplication.Models
{
    public class USERDATA
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
