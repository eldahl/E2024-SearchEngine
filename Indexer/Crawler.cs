using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WordService.Model;

namespace Indexer
{
    public class Crawler
    {
        private readonly char[] sep = " \\\n\t\"$'!,?;.:-_**+=)([]{}<>/@&%€#".ToCharArray();

        private Dictionary<string, int> words = new Dictionary<string, int>();
        private List<Document> documents = new();
    
        private HttpClient httpClient = new HttpClient();

        public Crawler()
        {
            httpClient.BaseAddress = new Uri("http://localhost:5225");
        }

        //Return a dictionary containing all words (as the key)in the file
        // [f] and the value is the number of occurrences of the key in file.
        private ISet<string> ExtractWordsInFile(FileInfo f)
        {
            ISet<string> res = new HashSet<string>();
            var content = File.ReadAllLines(f.FullName);
            foreach (var line in content)
            {
                foreach (var aWord in line.Split(sep, StringSplitOptions.RemoveEmptyEntries))
                {
                    res.Add(aWord);
                }
            }

            return res;
        }

        private ISet<int> GetWordIdFromWords(ISet<string> src)
        {
            ISet<int> res = new HashSet<int>();

            foreach ( var p in src)
            {
                res.Add(words[p]);
            }
            return res;
        }

        // Return a dictionary of all the words (the key) in the files contained
        // in the directory [dir]. Only files with an extension in
        // [extensions] is read. The value part of the return value is
        // the number of occurrences of the key.
        public void IndexFilesIn(DirectoryInfo dir, List<string> extensions)
        {
            Console.WriteLine("Crawling " + dir.FullName);

            foreach (var file in dir.EnumerateFiles())
            {
                if (extensions.Contains(file.Extension))
                {
                    Document document = new();
                    document.url = file.FullName;
                    document.id = documents.Count + 1;
                    documents.Add(document);
                    var json = JsonSerializer.Serialize(document);
                    Console.WriteLine("Doc Json: \n" + json + "\n");
                    var res = httpClient.PostAsync(
                        "/Document/InsertDocument", 
                        new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    Console.WriteLine("Status: " + res.StatusCode);
                    //mdatabase.InsertDocument(documents[file.FullName], file.FullName);
                    Dictionary<string, int> newWords = new Dictionary<string, int>();
                    ISet<string> wordsInFile = ExtractWordsInFile(file);
                    foreach (var aWord in wordsInFile)
                    {
                        if (!words.ContainsKey(aWord))
                        {
                            words.Add(aWord, words.Count + 1);
                            newWords.Add(aWord, words[aWord]);
                        }
                    }
                    
                    var result = httpClient.PostAsync("/Word/InsertAllWords", new StringContent(JsonSerializer.Serialize(newWords), Encoding.UTF8, "application/json")).Result;
                    Console.WriteLine("Status: " + result.StatusCode);
                    //mdatabase.InsertAllWords(newWords);
                    
                    Occurrences occ = new Occurrences();
                    occ.documentId = document.id;
                    occ.wordIds = GetWordIdFromWords(wordsInFile);
                    Console.WriteLine("Occ JSON: \n" + JsonSerializer.Serialize(occ) + "\n");
                    result = httpClient.PostAsync("/Occurrence/InsertAllOccurrences", new StringContent(JsonSerializer.Serialize(occ), Encoding.UTF8, "application/json")).Result;
                    Console.WriteLine("Status: " + result.StatusCode);
                    //mdatabase.InsertAllOcc(documents[file.FullName], GetWordIdFromWords(wordsInFile));
                }
            }

            foreach (var d in dir.EnumerateDirectories())
            {
                IndexFilesIn(d, extensions);
            }
        }
    }
}
