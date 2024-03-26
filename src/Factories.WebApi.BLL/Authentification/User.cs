using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factories.WebApi.BLL.Authentification
{
    public class User
    {
        public Guid? Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public UserRole? Role { get; set; }
    }

    public enum UserRole
    {
        Administrator,
        User
    }
}
