using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Utilities;

namespace UrlShortener.Controllers;

[ApiController]
[Route("")]
public class RedirectController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    public RedirectController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortUrl)
    {
        try
        {
            var id = Base62Encoder.Decode(shortUrl);
            var urlById = await _dbContext.Url.FirstOrDefaultAsync(u => u.Id == id);
            if (urlById != null)
            {
                return RedirectPermanent(urlById.LongUrl);
            }
            else
            {
                return NotFound("Short URL not found");
            }
        }
        catch (ArgumentException)
        {
            return NotFound("Short URL not found");
        }
    }
}