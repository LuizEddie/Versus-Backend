using Microsoft.EntityFrameworkCore;
using Versus.Models;

namespace Versus.Data
{
    public class VersusContext : DbContext
    {
        public VersusContext(DbContextOptions<VersusContext> options) : base(options) { }
        public DbSet<User> User { get; set; } = default!; 
        public DbSet<ResetPassword> ResetPassword { get; set; } = default!;
    }
}
