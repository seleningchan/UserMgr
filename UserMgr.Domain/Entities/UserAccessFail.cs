using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Domain.Entities
{
    public record UserAccessFail
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public User User { get; init; }//user和useraccessfail属于同一聚合，可以使用实体
        private bool lockOut;//是否锁定
        public DateTime? LockoutEnd { get; private set; }
        public int AccessFailedCount { get; private set;}
        private UserAccessFail() { }
        public UserAccessFail(User user)
        {
            Id = Guid.NewGuid();
            User = user;
        }
        public void Reset()
        {
            lockOut = false;
            LockoutEnd = null;
            AccessFailedCount = 0;
        }

        public void Fail()
        {
            AccessFailedCount++;
            if(AccessFailedCount >= 3)
            {
                lockOut = true;
                LockoutEnd = DateTime.Now.AddMinutes(5);
            }
        }

        public bool IsLockOut()
        {
            if (lockOut)
            {
                if (LockoutEnd >= DateTime.Now)
                {
                    return true;
                }
                else
                {
                    AccessFailedCount = 0;
                    LockoutEnd = null;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }








    }
}
