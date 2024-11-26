using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations;

public class CategoryConfiguration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(tmp => tmp.Id);
        builder.Property(tmp => tmp.Name)
            .IsRequired()
            .HasMaxLength(255);
        builder.ToTable(tmp =>
        {
            tmp.HasCheckConstraint("CK_Category_Name", "LEN([Name]) >= 3");
        });
    }
}
