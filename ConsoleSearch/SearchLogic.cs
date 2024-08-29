using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace ConsoleSearch
{
    public class SearchLogic
    {
        HttpClient _httpClient = new HttpClient();
        //Database mDatabase = new();

        Dictionary<string, int> mWords;

        public SearchLogic()
        { 
            var response = _httpClient.GetAsync("localhost/api/words").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(body);
            //mWords = mDatabase.GetAllWords();
        }

        public int GetIdOf(string word)
        {
            if (mWords.ContainsKey(word))
                return mWords[word];
            return -1;
        }

        public Dictionary<int, int> GetDocuments(List<int> wordIds)
        {
            return null;//mDatabase.GetDocuments(wordIds);
        }

        public List<string> GetDocumentDetails(List<int> docIds)
        {
            return null;//mDatabase.GetDocDetails(docIds);
        }
    }
}