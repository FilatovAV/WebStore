using System;

namespace WebStore.Domain.Identity
{
    public class SetLockoutDTO : UserInfoDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
