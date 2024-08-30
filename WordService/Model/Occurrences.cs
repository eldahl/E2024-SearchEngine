namespace WordService.Model;

public class Occurrences
{
    public int documentId { get; set; }
    public ISet<int> wordIds { get; set; }
}