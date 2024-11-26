using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Configurations;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(tmp => tmp.Id);
        builder.Property(tmp=>tmp.UserName)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(tmp=>tmp.Email)
            .IsRequired()
            .HasMaxLength(80)
            .HasAnnotation("EmailAddress", true);
        builder.Property(tmp=>tmp.Password)
            .IsRequired()
            .HasMaxLength(255)
            .HasAnnotation("Password", true);
        builder.Property(tmp => tmp.UserRole)
            .IsRequired();

        builder.ToTable(tmp =>
        {
            tmp.HasCheckConstraint("CHK_UserName_Len", "LEN(UserName) >= 3");
            tmp.HasCheckConstraint("CHK_Email_Len", "LEN(Email) >= 3");
            tmp.HasCheckConstraint("CHK_Password_Len", "LEN(Password) >= 8");
        });
    }
}
