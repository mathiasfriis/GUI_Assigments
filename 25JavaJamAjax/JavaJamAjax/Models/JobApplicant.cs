using System.ComponentModel.DataAnnotations;

namespace JavaJamAjax.Models
{
    public class JobApplicant
    {
        [StringLength(60), Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        
        
        [DataType(DataType.MultilineText)]
        [Required]
        public string Expirience { get; set; }
    }
}
