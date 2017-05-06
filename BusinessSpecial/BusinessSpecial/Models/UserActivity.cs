using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.Models
{
    public class UserActivity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("advertCategory")]
        public string AdvertCategory { get; set; }

        [JsonProperty("specialName")]
        public string SpecialName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("type")]
        public string ActivityType { get; set; }

        [JsonProperty("businessName")]
        public string BusinessName { get; set; }

        [JsonProperty("BusinessLoge")]
        public string BusinessLoge { get; set; }

        [JsonProperty("businessId")]
        public string BusinessId { get; set; }
    }
}
