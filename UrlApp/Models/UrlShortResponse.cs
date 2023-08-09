using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Models;

public class UrlShortResponse
{
    public string OriginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string CreatedByUserId { get; set; }
    public IdentityUser CreatedBy { get; set; }
}
