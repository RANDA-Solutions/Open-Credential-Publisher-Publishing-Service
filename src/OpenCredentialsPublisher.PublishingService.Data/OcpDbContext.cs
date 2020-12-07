using Microsoft.EntityFrameworkCore;

namespace OpenCredentialsPublisher.PublishingService.Data
{
    public class OcpDbContext : DbContext
    {
        public OcpDbContext(DbContextOptions<OcpDbContext> options)
            : base(options)
        {
        }

        public DbSet<PublishRequest> PublishRequests { get; set; }
        public DbSet<AccessKey> AccessKeys { get; set; }
        public DbSet<SigningKey> SigningKeys { get; set; }

        public DbSet<ClrPublishLog> ClrPublishLogs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PublishRequest>(entity => {

                entity.ToTable("PublishRequest", "dbo");

                entity.HasKey(x => x.RequestId)
                        .IsClustered(clustered: false);

                entity.HasIndex(x => x.Id)
                        .HasName("CIX_PublishRequest_Id")
                        .IsUnique()
                        .IsClustered(clustered: true);
            });

            modelBuilder.Entity<File>(entity => { entity.ToTable("File", "dbo"); });
            modelBuilder.Entity<AccessKey>(entity => { entity.ToTable("AccessKey", "dbo"); });
            modelBuilder.Entity<SigningKey>(entity => { entity.ToTable("SigningKey", "dbo"); });
            modelBuilder.Entity<ClrPublishLog>(entity => { entity.ToTable("ClrPublishLog", "dbo"); });
        }
    }
}
