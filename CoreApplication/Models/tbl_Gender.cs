using System.ComponentModel.DataAnnotations;

namespace CoreApplication.Models
{
    public class tbl_Gender
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Gender { get; set; }

    }
}
