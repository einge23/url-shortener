namespace UrlShortener.Data.Models;
public record Url
{
    public long Id { get; set; }  
    public string ShortUrl { get; set; } = string.Empty;
    public string LongUrl { get; set; } = string.Empty;
}
