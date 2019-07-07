using System.Security.Claims;

namespace WebStore.Domain.Identity
{
    public class ReplaceClaimDTO: UserInfoDTO
    {
        public Claim OldClaim { get; set; }
        public Claim newClaim { get; set; }

    }
}
