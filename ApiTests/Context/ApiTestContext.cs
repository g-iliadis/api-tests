using ApiTests.ApiObjects;

namespace ApiTests.Context
{
    public class ApiTestContext
    {
        public UsersApi Users { get; }

        public ApiTestContext(UsersApi users)
        {
            Users = users;
        }
    }
} 