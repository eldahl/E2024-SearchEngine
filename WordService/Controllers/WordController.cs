using Microsoft.AspNetCore.Mvc;
using Word = WordService.Model.Word;

namespace WordService.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController : ControllerBase
{   
    private readonly Database _database;
    public WordController(Database db) {
        _database = db;
    }

    [HttpPost("InsertAllWords")]
    public ActionResult InsertAllWords([FromBody] Dictionary<string, int> words)
    {
        _database.InsertAllWords(words);
        return Ok();
    }

    [HttpGet("GetAllWords")]
    public Dictionary<string, int> GetAllWords() {
        return _database.GetAllWords();
    }
}