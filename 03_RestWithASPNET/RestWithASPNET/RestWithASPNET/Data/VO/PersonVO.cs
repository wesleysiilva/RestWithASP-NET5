//using System.Text.Json.Serialization;

using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermedia.Abstract;
using System.Collections.Generic;

namespace RestWithASPNET.Data.VO {
  
  public class PersonVO : ISupportsHyperMedia {

    //Custom Serialization Example
    //[JsonPropertyName("code")]
    public long Id { get; set; }

    //[JsonPropertyName("name")]
    public string FirstName { get; set; }

    //[JsonPropertyName("last_name")]
    public string LastName { get; set; }

    //[JsonIgnore]
    public string Adress { get; set; }

    //[JsonPropertyName("sex")]
    public string Gender { get; set; }

    public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
  }
}
