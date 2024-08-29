using Microsoft.AspNetCore.Mvc;

namespace WordService.Controllers;

[ApiController]
[Route("[controller]")]
public class OccurenceCntroller : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "test";
    }
}