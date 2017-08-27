using HackerNewsApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsApp.Api
{
    public class HackerNewsApi
    {
        private const string BASE_URL = "https://hacker-news.firebaseio.com/v0/";
        private const int SIZE_LIMIT = 30;

        public async Task<List<HackerNewsStory>> GetStoriesAsync()
        {
            var result = new List<HackerNewsStory>(SIZE_LIMIT);
            var topStoriesIds = new List<string>(SIZE_LIMIT);

            var topStoriesJson = string.Empty;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(BASE_URL + "topstories.json?print=pretty");

                if (!response.IsSuccessStatusCode) return result;

                topStoriesJson = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(topStoriesJson)) return result;

                var jsonResult = JArray.Parse(topStoriesJson);

                var i = 0;

                foreach (var id in jsonResult)
                {
                    if (i == SIZE_LIMIT) break;

                    topStoriesIds.Add(id?.ToString());

                    i++;
                }

                foreach (var id in topStoriesIds)
                {
                    response = await client.GetAsync(BASE_URL + $"item/{id}.json?print=pretty");

                    if (response.IsSuccessStatusCode)
                    {
                        var storyJson = await response.Content.ReadAsStringAsync();

                        var story = JsonConvert.DeserializeObject<HackerNewsStory>(storyJson);

                        result.Add(story);
                    }
                }
            }

            return result;
        }
    }
}
