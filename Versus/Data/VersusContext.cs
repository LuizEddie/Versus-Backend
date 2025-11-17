using Microsoft.EntityFrameworkCore;
using Versus.Models;
using Versus.Models.Tournament;

namespace Versus.Data
{
    public class VersusContext : DbContext
    {
        public VersusContext(DbContextOptions<VersusContext> options) : base(options) { }
        public DbSet<User> User { get; set; } = default!; 
        public DbSet<ResetPassword> ResetPassword { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
        //Tournament
        public DbSet<TournamentStatus> TournamentStatuses { get; set; } = default!;
        public DbSet<TournamentType> TournamentTypes { get; set;} = default!;
        public DbSet<Tournament> Tournaments { get; set; } = default!;
        public DbSet<Player> Players { get; set; } = default!;
        public DbSet<Match> Matches { get; set; } = default!;
        public DbSet<Classification> Classifications { get; set; } = default!;
        public DbSet<TournamentSpec> TournamentSpecs { get; set; } = default!;
        public DbSet<TournamentTieBreaker> TournamentTieBreaker { get; set; } = default!;
        public DbSet<TournamentWinConditon> TournamentWinConditon { get; set; } = default!;
    }
}
