using IdGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Models;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/v1/data")]
public class DataController : ControllerBase
{
    private readonly ILogger<DataController> _logger;
    private readonly IdGenerator _idGenerator;
    private readonly AppDbContext _dbContext;
    public DataController(ILogger<DataController> logger, IdGenerator idGenerator, AppDbContext dbContext)
    {
        _logger = logger;
        _idGenerator = idGenerator;
        _dbContext = dbContext;
    }

    [HttpPost("shorten")]
    public async Task<ActionResult<string>> ShortenUrl([FromBody] string longUrlString)
    {
        if (string.IsNullOrEmpty(longUrlString))
        {
            return BadRequest("Long URL is required");
        }

        var existingUrl = await _dbContext.Url.FirstOrDefaultAsync(u => u.LongUrl == longUrlString);
        if (existingUrl != null)
        {
            return Ok(existingUrl.ShortUrl);
        }

        else
        {
            var id = _idGenerator.CreateId();
            var encodedId = Base62Encoder.Encode(id);
            var shortUrl = $"https://shorty-url.up.railway.app{encodedId}";
            var url = new Url
            {
                Id = id,
                LongUrl = longUrlString,
                ShortUrl = shortUrl
            };
            _dbContext.Url.Add(url);
            await _dbContext.SaveChangesAsync();
            return Ok(url.ShortUrl);
        }
    }
}
