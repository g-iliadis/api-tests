using Newtonsoft.Json;

namespace ApiTests.Models
{
    public class UsersResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public List<User> Data { get; set; } = new();
    }

    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("last_name")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("avatar")]
        public string Avatar { get; set; } = string.Empty;
    }
    
    public class ErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; } = string.Empty;
    }
} 