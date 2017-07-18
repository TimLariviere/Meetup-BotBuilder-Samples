using Newtonsoft.Json;
using System.Collections.Generic;

namespace DemoDialogs.Wiki
{
    public class WikiSearchResult
    {
        [JsonProperty("query")]
        public WikiSearchResultQuery Query { get; set; }
    }

    public class WikiSearchResultQuery
    {
        [JsonProperty("search")]
        public IEnumerable<WikiSearchResultItem> Items { get; set; }
    }

    public class WikiSearchResultItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}