using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Entities
{
    public class User: IdentityUser
    {
        public const string AdminUserName = "Admin";
        public const string DefaultAdminPassword = "12345";

        public const string RoleAdmin = "Administrator";
        public const string RoleUser = "User";
    }

    //class Role: IdentityRole
    //{
    //}
}
