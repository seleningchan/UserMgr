﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Domain
{
    public enum UserAccessResult
    {
        OK,
        PhoneNumberNotFound,
        Lockout,
        NoPassword,
        PasswordError
    }
}
