using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities;

namespace UserMgr.Infrastructure.Configs
{
    public class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("T_UserLoginHistories");
            builder.OwnsOne(x => x.PhoneNumber, nb => {
                nb.Property(x => x.RegionNumber).HasMaxLength(5).IsUnicode(false);
                nb.Property(x => x.MobileNumber).HasMaxLength(20).IsUnicode(false);
            });
        }
    }
}
