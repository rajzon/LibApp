using System;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Models
{
    public class AppUser : IdentityUser<int>
    {
    }

    public class CustomerUser : IdentityUser<int>
    {
    }
    
}