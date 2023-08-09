using Microsoft.AspNetCore.Identity;
using UrlApp.Data;

namespace UrlShortener.Models;

public class UrlShortRequest
{
    public string OriginalUrl { get; set; }

    public async Task<UrlShort> ToUrlShort(XUser user)
    {
        return new UrlShort
        {
            Id = Guid.NewGuid(),
            OriginalUrl = OriginalUrl,
            ShortUrl = await GetUrlShorter.ShortenUrlAsync(OriginalUrl),
            CreatedDate = DateTime.UtcNow,
            CreatedBy = user.Name
        };
    }
}

public static class GetUrlShorter
{
    private static readonly HttpClient client = new HttpClient();
    private const string BitlyApiKey = "032a5f2e722c5016d51deeec162d0ef87c6f5e83";

    public static async Task<string> ShortenUrlAsync(string originalUrl)
    {
        string apiUrl = $"https://api-ssl.bitly.com/v4/shorten";

        var requestData = new
        {
            long_url = originalUrl
        };

        var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        request.Headers.Add("Authorization", $"Bearer {BitlyApiKey}");
        request.Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(requestData),
                    System.Text.Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
            string shortenedUrl = responseData.link;
            return shortenedUrl;
        }
        else
        {
            throw new Exception($"Failed to shorten URL. StatusCode: {response.StatusCode}");
        }
    }
}