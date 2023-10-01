using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PayMasta.Admin.Models
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public long UserId { get; set; }

        public string Name { get; set; }

        public string[] roles { get; set; }

        public string UserGroup { get; set; }

        public int UserLevel { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }

        public string[] roles { get; set; }

        public string UserGroup { get; set; }

        public int UserLevel { get; set; }
        public string Token { get; set; }
    }
}