using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public UserType UserType { get; set; }
        public int UserTypeId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public bool Status { get; set; }
    }
}
