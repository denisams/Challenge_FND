using Newtonsoft.Json;
using System.Collections.Generic;

namespace DenisChallenge.Domain.Entities
{
    public class Aanbod
    {
        public Aanbod()
        {
            EigenschapBeschrijving = new List<EigenschapBeschrijving>();
        }

        [JsonProperty("Objects")]
        public List<EigenschapBeschrijving> EigenschapBeschrijving { get; set; }
    }
}