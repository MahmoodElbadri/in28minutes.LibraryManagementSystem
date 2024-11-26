using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations;

public class BookConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(tmp => tmp.Id);
        builder.Property(tmp => tmp.Title)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(tmp => tmp.Author)
            .IsRequired()
            .HasMaxLength(80);
        builder.Property(tmp => tmp.ISBN)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(tmp => tmp.Publisher)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(tmp => tmp.PublishDate)
            .IsRequired();
        
        builder.ToTable(tmp =>
        {
            tmp.HasCheckConstraint("CHK_Title_Len", "LEN(Title) >= 3");
            tmp.HasCheckConstraint("CHK_Title_Len", "LEN(Author) >= 3");
            tmp.HasCheckConstraint("CHK_Title_Len", "LEN(ISBN) >= 3");
            tmp.HasCheckConstraint("CHK_Title_Len", "LEN(Publisher) >= 3");
        });
        
        
    }
}
