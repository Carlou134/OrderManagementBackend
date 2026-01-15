using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementBackend.Domain;

namespace OrderManagementBackend.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(10);

            builder.Property(x => x.OrderDate).ValueGeneratedOnAdd();

            builder.Property(x => x.Status).HasConversion<byte>();

            builder.Property(x => x.FinalPrice).IsRequired().HasPrecision(10,2);
        }
    }
}
