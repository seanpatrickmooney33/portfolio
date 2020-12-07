using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data.Model;

namespace SpecialElection.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }


        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<CandidateResult> CandidateResults { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Election> Elections { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<RaceCountyData> RaceCountyData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

}
