using ERNST.Dto.UserType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERNST.Dto.User
{
    public class UserForDisplay
    {
        public int Id { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public bool Status { get; set; }
    }
}
