using Microsoft.EntityFrameworkCore;
using System;

namespace UserMgr.WebAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UnitOfworkAttribute : Attribute
    {
        public Type[] DbContextTypes { get; init; }
        public UnitOfworkAttribute(params Type[] dbContextTypes)
        {
            DbContextTypes = dbContextTypes;
            foreach(var type in dbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(type))
                {
                    throw new ArgumentException($"{type} must be inherited from DbContext");
                }
            }
        }
    }
}
