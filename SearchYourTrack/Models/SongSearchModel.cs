using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SearchYourTrack.Models
{
    public class SongSearchModel
    {
        public int RecordCount { get; set; }
        [Required(ErrorMessage ="Please select any one search option!" )]
        public string SearchByOption { get; set; }
        [Required(ErrorMessage="Please enter search keywords!")]
        [MaxLength(50, ErrorMessage ="Keywords can be maximum 50 characters long")]
        public string SearchInput { get; set; }
        public string[] SearchOptions = new string[] { "Pattern", "Artists" };
        public Root Root { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class UseThePrefix
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class Artist
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
        public string nameWithoutThePrefix { get; set; }
        public UseThePrefix useThePrefix { get; set; }
        public string name { get; set; }
    }

    public class ChordsPresent
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class TabTypes
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
        public object TabType { get; set; }
    }

    public class Song
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }
        public string title { get; set; }
        public Artist artist { get; set; }
        public ChordsPresent chordsPresent { get; set; }
        public TabTypes tabTypes { get; set; }
    }

    public class NSArray
    {
        public List<Song> Song { get; set; }
    }

    public class Root
    {
        public NSArray NSArray { get; set; }
    }

}
