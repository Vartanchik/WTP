using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.DTOs.PlayerDTOs
{
    public class PlayerListItemDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("n")]
        public string Name { get; set; }
        [JsonProperty("ga")]
        public string Game { get; set; }
        [JsonProperty("r")]
        public string Rank { get; set; }
        [JsonProperty("s")]
        public string Server { get; set; }
        [JsonProperty("g")]
        public string Goal { get; set; }
    }
}
