using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecial.Models
{
    public class Advert
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("specialName")]
        public string SpecialName { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
