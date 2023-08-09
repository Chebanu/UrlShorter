using Microsoft.AspNetCore.Identity;

namespace UrlShortener.Models;

public class UrlShort
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; }
}
