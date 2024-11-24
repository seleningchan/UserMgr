using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Infrastructure
{

    /// <summary>
    /// 0.Install-Package Microsoft.EntityFrameworkCore.Tools
    /// 1.Add-Migration init  (init名字是可以更改的)
    /// 2.Update-Database
    /// </summary>
    public class DbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserDbContext>();
            builder.UseSqlServer("Server=.;DataBase=DDD;User ID=admin;Password=123456;Trusted_Connection=true;TrustServerCertificate=true");
            return new UserDbContext(builder.Options);
        }
    }
}
