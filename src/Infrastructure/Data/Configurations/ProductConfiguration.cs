using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProductName).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.CreatedOn).IsRequired();
        builder.Property(p => p.ModifiedBy);
        builder.Property(p => p.ModifiedOn);
    }
}
// This configuration class defines the primary key for the Product entity and specifies that the ProductName, CreatedBy, and CreatedOn properties are required.
// It also establishes a one-to-many relationship with the Item entity, indicating that each Product can have multiple Items associated with it.
// The foreign key for this relationship is defined on the Item entity, linking it to the Product entity through the ProductId property.
