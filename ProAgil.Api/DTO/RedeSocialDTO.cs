using Newtonsoft.Json;

namespace ProAgil.Api.DTO
{
    public class RedeSocialDTO
    {

		[JsonProperty("idRedeSocial")]
		public int RedeSocialId { get; set; }
		public string  Nome { get; set; }

		public string URL { get; set; }

    }
}