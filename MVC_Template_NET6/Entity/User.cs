using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Template_NET6.Entity
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public String Fullname { get; set; }

        [Required]
        [StringLength(30)]
        public String Username { get; set; }

        [Required]
        [StringLength(100)]
        public String Password { get; set; }

        public bool Locked { get; set; } = true;

        public DateTime MyProperty { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "user";

    }

}
