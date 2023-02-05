using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenBasedAuthApplication.Core.Entities;

namespace TokenBasedAuthApplication.DataAccess.Configurations;

public class UserRefreshTokenConfiguration: IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.ToTable("UserRefreshTokens");
        builder.HasKey(x => x.UserId);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(255);
    }
}