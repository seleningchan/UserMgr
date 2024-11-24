using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;
using UserMgr.Infrastructure;

namespace UserMgr.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    public class UserMgrController : ControllerBase
    {
        private readonly UserDbContext userDbContext;
        private readonly IUserDomainRepository userDomainRepository;

        public UserMgrController(UserDbContext userDbContext, IUserDomainRepository userDomainRepository)
        {
            this.userDbContext = userDbContext;
            this.userDomainRepository = userDomainRepository;
        }

        [HttpPost]
        [UnitOfwork([typeof(UserDbContext)])]
        public async Task<IActionResult> AddNew()
        {
            var phone = new PhoneNumber(86, "15021518712");
            var user = new User(phone);
            this.userDbContext.Add(user);
            await Task.CompletedTask;
            return Ok("Success");
        }
    }
}
