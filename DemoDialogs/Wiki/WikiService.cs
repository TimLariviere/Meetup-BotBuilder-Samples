using DemoDialogs.Wiki;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoDialogs.Wiki
{
    public class WikiService
    {
        private const string Url = "https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={0}&format=json";

        public static async Task<IEnumerable<string>> SearchArticlesAsync(string name)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(string.Format(Url, name));
            var result = JsonConvert.DeserializeObject<WikiSearchResult>(json);

            return result?.Query?.Items?.Select(i => i.Title) ?? new string[0];
        }
    }
}