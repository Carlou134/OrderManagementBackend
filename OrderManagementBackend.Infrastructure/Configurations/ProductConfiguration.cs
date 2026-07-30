using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(10, 2);

            builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Property(x => x.UpdatedAt);

            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(100);

            builder.Property(x => x.UpdatedBy).HasMaxLength(100);

            var seedCreatedAt = new DateTime(2026, 1, 16, 0, 0, 0, DateTimeKind.Utc);
            const string seedCreatedBy = "system";

            builder.HasData(new Product
            {
                Id = 1,
                Name = "Laptop Lenovo ThinkPad E14",
                UnitPrice = 3200.00m,
                CreatedAt = seedCreatedAt,
                CreatedBy = seedCreatedBy
            },
                new Product
                {
                    Id = 2,
                    Name = "Mouse Logitech M185",
                    UnitPrice = 75.50m,
                    CreatedAt = seedCreatedAt,
                    CreatedBy = seedCreatedBy
                },
                new Product
                {
                    Id = 3,
                    Name = "Teclado Mecánico Redragon Kumara",
                    UnitPrice = 249.90m,
                    CreatedAt = seedCreatedAt,
                    CreatedBy = seedCreatedBy
                });
        }
    }
}
