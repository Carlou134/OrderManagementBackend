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

            builder.HasData(new Product
            {
                Id = 1,
                Name = "Laptop Lenovo ThinkPad E14",
                UnitPrice = 3200.00m
            },
                new Product
                {
                    Id = 2,
                    Name = "Mouse Logitech M185",
                    UnitPrice = 75.50m
                },
                new Product
                {
                    Id = 3,
                    Name = "Teclado Mecánico Redragon Kumara",
                    UnitPrice = 249.90m
                });
        }
    }
}
