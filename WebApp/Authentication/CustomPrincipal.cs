using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ToolsApp.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity {     get; private set; }


        public bool IsInRole(string role)
        {
            return true;
        }

        public CustomPrincipal(string UserName)
        {
            this.Identity = new GenericIdentity(UserName);
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int idDepartment { get; set; }
        public string role { get; set; }
        public string token { get; set; }
    }

    public class CustomPrincipalSerializeModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int idDepartment { get; set; }
        public string role { get; set; }
        public string token { get; set; }
    }
}