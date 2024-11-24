using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities;

namespace UserMgr.Infrastructure.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("T_Users");
            builder.OwnsOne(x => x.PhoneNumber, nb =>
            {
                nb.Property(x => x.RegionNumber).HasMaxLength(5).IsUnicode(false);
                nb.Property(x => x.MobileNumber).HasMaxLength(20).IsUnicode(false);
            });
            builder.Property("passwordHash").HasMaxLength(100).IsUnicode(false);
            builder.HasOne(x => x.AccessFail).WithOne(x => x.User)
                .HasForeignKey<UserAccessFail>(x => x.UserId);
        }
    }
}
