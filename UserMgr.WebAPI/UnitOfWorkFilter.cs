using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace UserMgr.WebAPI
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private static UnitOfworkAttribute? GetUoAttr(ActionDescriptor actionDescriptor)
        {
            var actionDesc = actionDescriptor as ControllerActionDescriptor;
            if(actionDesc == null)
            {
                return null;
            }
            var uoAttr = actionDesc.ControllerTypeInfo
                .GetCustomAttribute<UnitOfworkAttribute>();
            if (uoAttr != null)
            {
                return uoAttr;
            }
            else
            {
                return actionDesc.MethodInfo.GetCustomAttribute<UnitOfworkAttribute>();
            }
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uoAttr = GetUoAttr(context.ActionDescriptor);
            if(uoAttr == null)
            {
                await next();
                return;
            }
            var dbContexts = new List<DbContext>();
            foreach (var dbType in uoAttr.DbContextTypes)
            {
                var sp = context.HttpContext.RequestServices;
                DbContext dbContext = (DbContext)sp.GetRequiredService(dbType);
                dbContexts.Add(dbContext);
            }
            var result = await next();
            if (result.Exception == null)
            {
                foreach (var dbCtx in dbContexts)
                {
                    await dbCtx.SaveChangesAsync();
                }
            }
        }
       
    }
}
