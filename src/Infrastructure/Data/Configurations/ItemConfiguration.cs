using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item");
        builder.HasKey(i => i.Id);
        builder.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId);
        builder.Property(i => i.Quantity).IsRequired();
    }
}
// This configuration class defines the primary key for the Item entity and specifies that the Quantity property is required.
// It also establishes a relationship with the Product entity, indicating that each Item is associated with a single Product.