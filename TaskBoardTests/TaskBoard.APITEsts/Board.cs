using System.Text.Json.Serialization;

namespace TaskBoard.APITEsts
{
    internal class Board
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
