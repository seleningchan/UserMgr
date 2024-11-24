using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
    public interface IUserDomainRepository
    {
        Task<User?> FindOneAsync(PhoneNumber phoneNumber);
        Task<User?> FindOneAsync(Guid id);
        Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string message);
        Task PublishEventAsync(UserAccessResultEvent @event);
        Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code);
        Task <String?> RetrievePhoneCodeAsync(PhoneNumber phoneNumber);
    }
}
