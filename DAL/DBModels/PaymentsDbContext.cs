using Microsoft.EntityFrameworkCore;

namespace PaymentAPI.DBModels
{
    public partial class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext()
        {
        }

        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TransactionDTO> Transactions { get; set; }
        public virtual DbSet<UserDTO> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<TransactionDTO>(entity =>
            {
                entity.HasKey(e => e.TransactionId)
                    .HasName("PK__Transact__55433A4B9FB4AEC4");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(50);

                entity.Property(e => e.Instrument).HasMaxLength(10);

                entity.Property(e => e.Metadata).HasMaxLength(1000);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Response).HasMaxLength(4000);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.TransactionTime).HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasMaxLength(20);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VendorId).HasColumnName("VendorID");
            });

            modelBuilder.Entity<UserDTO>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__1788CCACA2DA0F4F");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(50);

                entity.Property(e => e.UserCardToken).HasMaxLength(50);
            });
        }
    }
}
