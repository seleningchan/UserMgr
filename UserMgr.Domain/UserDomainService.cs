using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
    public class UserDomainService
    {
        private readonly IUserDomainRepository _userDomainRepository;
        private readonly ISmsCodeSender _smsSender;

        public UserDomainService(IUserDomainRepository userDomainRepository, ISmsCodeSender smsSender)
        {
            _userDomainRepository = userDomainRepository;
            _smsSender = smsSender;
        }

        public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNumber, string password)
        {
            User? user = await _userDomainRepository.FindOneAsync(phoneNumber);
            UserAccessResult userAccessResult;
            if (user == null)
                userAccessResult = UserAccessResult.PhoneNumberNotFound;
            else if (IsLockOut(user))
                userAccessResult = UserAccessResult.Lockout;
            else if (user.HasPassword() == false)
                userAccessResult = UserAccessResult.NoPassword;
            else if (user.CheckPassword(password))
                userAccessResult = UserAccessResult.OK;
            else
                userAccessResult= UserAccessResult.PasswordError;

            if (user != null)
            {
                if (userAccessResult == UserAccessResult.OK)
                    this.ResetAccessFail(user);
                else
                    this.AccessFail(user);
            }

            UserAccessResultEvent @event = new UserAccessResultEvent(phoneNumber, userAccessResult);
            await _userDomainRepository.PublishEventAsync(@event);
            return userAccessResult;
        }

        public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNumber, string code)
        {
            var user = await _userDomainRepository.FindOneAsync(phoneNumber);
            if (user == null)
                return CheckCodeResult.PhoneNumberNotFound;
            if (IsLockOut(user))
                return CheckCodeResult.LockOut;
            string? codeInServer = await _userDomainRepository.RetrievePhoneCodeAsync(phoneNumber);
            if (string.IsNullOrEmpty(codeInServer))
                return CheckCodeResult.CodeError;
            if (code == codeInServer)
            {
                return CheckCodeResult.OK;
            }
            else
            {
                AccessFail(user);
                return CheckCodeResult.CodeError;
            }
        }

        public void ResetAccessFail(User user)
        {
            user.AccessFail.Reset();
        }

        public bool IsLockOut(User user)
        {
            return user.AccessFail.IsLockOut();
        }

        public void AccessFail(User user)
        {
            user.AccessFail.Fail();
        }

    }
}
