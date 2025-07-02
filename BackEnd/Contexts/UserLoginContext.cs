using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BackEnd.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
       : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
    }
}
