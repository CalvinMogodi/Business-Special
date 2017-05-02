using BusinessSpecial.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.Models
{
    public class User
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("userTypeId")]
        public int UserTypeId { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("categories")]
        public List<Category> Categories { get; set;}
        [JsonProperty("userType")]
        public UserType UserType { get; set; }
    }
}
