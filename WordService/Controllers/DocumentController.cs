using Microsoft.AspNetCore.Mvc;
using Document = WordService.Model.Document;

namespace WordService.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly Database _database;
    public DocumentController(Database db) { 
        _database = db;
    }

    [HttpPost("InsertDocument")]
    public ActionResult InsertDocument([FromBody] Document document) {
        _database.InsertDocument(document.id, document.url);
        return Ok();
    }

    [HttpPost("GetDocuments")]
    public Dictionary<int, int> GetDocuments([FromBody] List<int> wordIds) {
        return _database.GetDocuments(wordIds);
    }

    [HttpPost("GetDocDetails")]
    public List<string> GetDocDetails([FromBody] List<int> docIds) {
        return _database.GetDocDetails(docIds);
    }
}