using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WordService.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly Database _database;

    public DatabaseController(Database db)
    {
        _database = db;
    }

    [HttpGet("ClearDB")]
    public ActionResult ClearDatabase()
    {
        _database.ClearDB();
        return Ok();
    }
}