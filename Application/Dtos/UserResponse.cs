using System.Text.Json.Serialization;
using URLshortner.Enums;

namespace URLshortner.Dtos
{
    public class UserResponse
    {

        public required int id { get; set; }
        public required string username { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required Roles role { get; set; }
    }
}