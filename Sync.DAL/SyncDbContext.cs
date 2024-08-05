using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sync.Common.Options;
using Sync.DAL.Models;

namespace Sync.DAL
{
    public class SyncDbContext : DbContext
    {
        private readonly IOptions<SQLConnectionString>? _option;

        public SyncDbContext(DbContextOptions<SyncDbContext> options, IOptions<SQLConnectionString>? option = null) : base(options)
        {
            _option = option;
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<Models.TimePeriod> TimePeriods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (_option is not null)
            {
                optionsBuilder.UseSqlServer(_option?.Value.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Image>()
                .HasOne(x => x.Field)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.FieldId);

            modelBuilder.Entity<Polygon>()
                .HasOne(x => x.Field)
                .WithMany(x => x.Polygons)
                .HasForeignKey(x => x.FieldId);

            modelBuilder.Entity<Field>()
                .HasMany(x => x.TimePeriods)
                .WithMany(x => x.Fields)
                .UsingEntity<Dictionary<string, object>>(
                    "TimePeriodField",
                    j => j.HasOne<Models.TimePeriod>().WithMany().HasForeignKey("TimePeriodId"),
                    j => j.HasOne<Field>().WithMany().HasForeignKey("FieldId"),
                    j => { j.HasKey("FieldId", "TimePeriodId"); });
        }
    }
}
