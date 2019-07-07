namespace WebStore.Domain.Identity
{
    public class PasswordHashDTO: UserInfoDTO
    {
        public string Hash { get; set; }
    }
}
