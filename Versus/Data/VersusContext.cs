using Microsoft.EntityFrameworkCore;

namespace Versus.Data
{
    public class VersusContext : DbContext
    {
        public VersusContext(DbContextOptions<VersusContext> options) : base(options) { }
    }
}
