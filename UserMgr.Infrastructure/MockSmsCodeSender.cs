using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserMgr.Domain;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Infrastructure
{
    public class MockSmsCodeSender : ISmsCodeSender
    {
        private readonly ILogger<MockSmsCodeSender> _logger;

        public MockSmsCodeSender(ILogger<MockSmsCodeSender> logger)
        {
            this._logger = logger;
        }

        public Task SendCodeAsync(PhoneNumber phoneNumber, string code)
        {
            this._logger.LogInformation($"send mobile {phoneNumber} with code {code}");
            return Task.CompletedTask;
        }
    }
}
