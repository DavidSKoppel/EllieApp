using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EllieApp.Models
{
    public class Alarm
    {
        [JsonPropertyName("id")]
        public int id {  get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("activatingTime")]
        public DateTime activatingTime { get; set; }
        [JsonPropertyName("imageUrl")]
        public string imageUrl { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
        [JsonPropertyName("alarmTypeId")]
        public int alarmTypeId { get; set; }
    }
}
