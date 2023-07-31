using Microsoft.EntityFrameworkCore;

namespace OpenCredentialPublisher.PublishingService.Data
{
    public class OcpDbContext : DbContext
    {
        public OcpDbContext(DbContextOptions<OcpDbContext> options)
            : base(options)
        {
        }

        public DbSet<PublishRequest> PublishRequests { get; set; }
        public DbSet<RevocationList> RevocationLists { get; set; }
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

                entity.Property(x => x.PushAfterPublish)
                        .HasDefaultValue(false);
            });

            modelBuilder.Entity<File>(entity => { entity.ToTable("File", "dbo"); });
            modelBuilder.Entity<AccessKey>(entity => { entity.ToTable("AccessKey", "dbo"); });
            modelBuilder.Entity<SigningKey>(entity => { 
                entity.ToTable("SigningKey", "dbo"); 
                entity.Property(e => e.StoredInKeyVault)
                    .HasDefaultValue(false);
            });
            modelBuilder.Entity<ClrPublishLog>(entity => { entity.ToTable("ClrPublishLog", "dbo"); });

            modelBuilder.Entity<RevocationList>(entity => entity.HasIndex(ix => ix.PublicId).HasName("IX_RevocationList_PublicId").IsUnique());
        }
    }
}
