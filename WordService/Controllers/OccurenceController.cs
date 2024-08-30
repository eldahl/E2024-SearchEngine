using Microsoft.AspNetCore.Mvc;
using WordService.Model;

namespace WordService.Controllers;

[ApiController]
[Route("[controller]")]
public class OccurrenceController : ControllerBase
{
    private readonly Database _database;
    public OccurrenceController(Database db) { 
        _database = db;
    }

    [HttpPost("InsertAllOccurrences")]
    public ActionResult InsertAllOccurrences([FromBody] Occurrences occ) {
        // Hashset because all word ids are unique, and it is an easy shortcut from enumerable
        _database.InsertAllOcc(occ.documentId, occ.wordIds);
        return Ok();
    }
}