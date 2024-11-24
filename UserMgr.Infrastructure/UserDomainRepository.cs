using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using static UserMgr.Infrastructure.ExpressionHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace UserMgr.Infrastructure
{
    public class UserDomainRepository : IUserDomainRepository
    {
        private readonly UserDbContext _userDbContext;
        private readonly IDistributedCache _distributedCache;
        private readonly IMediator _mediator;

        public UserDomainRepository(UserDbContext userDbContext, IDistributedCache distributedCache, IMediator mediator)
        {
            this._userDbContext = userDbContext;
            this._distributedCache = distributedCache;
            this._mediator = mediator;
        }

        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message)
        {
            var user = await FindOneAsync(phoneNumber);
            var history = new UserLoginHistory(user?.Id, phoneNumber,message);
            this._userDbContext.LoginHistories.Add(history);
            //?这里不保存
            //因为会使用unit work来保存
        }

        public Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            return this._userDbContext.Users.Include(i=>i.AccessFail)
                .SingleOrDefaultAsync(MakeEqual((User u) => u.PhoneNumber, phoneNumber));
        }

        public Task<User?> FindOneAsync(Guid id)
        {
            return this._userDbContext.Users.Include(u=>u.AccessFail)
                .SingleOrDefaultAsync(u=>u.Id==id);
        }

        public Task PublishEventAsync(UserAccessResultEvent @event)
        {
            return this._mediator.Publish(@event);
        }

        public Task<string?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber)
        {
            string fullNumber = phoneNumber.RegionNumber + phoneNumber.MobileNumber;
            string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
            string? code = this._distributedCache.GetString(cacheKey);
            this._distributedCache.Remove(cacheKey);
            return Task.FromResult(code);
        }

        public Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code)
        {
            string fullNumber = phoneNumber.RegionNumber + phoneNumber.MobileNumber;
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
            this._distributedCache.SetString($"LoginByPhoneAndCode_Code_{fullNumber}", code, options);
            return Task.CompletedTask;
        }
    }
}
