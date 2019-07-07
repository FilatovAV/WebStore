using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;

namespace WebStore.Clients.Users
{
    public class RoleClient : BaseClient
    {
        public RoleClient(IConfiguration configuration) : base(configuration, "api/roles")
        { }
    }
}
