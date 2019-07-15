using WebStore.Domain.Entities;

namespace WebStore.Domain.Identity
{
    public abstract class UserInfoDTO
    {
        public User User { get; set; }
    }
}
