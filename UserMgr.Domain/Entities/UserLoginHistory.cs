using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain.Entities
{
    public record UserLoginHistory : IAggreateRoot
    {
        public long Id { get; init; }
        //为空是因为可能输入一个不存在用户id
        public Guid? UserId { get; init; }//user login history是一个单独的聚合，不会引用User实体，便于以后服务拆分
        public PhoneNumber PhoneNumber { get; init; }
        public DateTime CreatedDateTime { get; init; }
        public string Message { get; init; }
        private UserLoginHistory() { }
        public UserLoginHistory(Guid? userId, PhoneNumber phoneNumber, string message)
        {
            this.UserId = userId;
            this.PhoneNumber = phoneNumber;
            this.Message = message;
            this.CreatedDateTime = DateTime.Now;
        }
    }
}
