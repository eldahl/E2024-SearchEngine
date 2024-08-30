using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
            _httpClient.BaseAddress = new Uri("http://localhost:5225");
            var response = _httpClient.GetAsync("/Word/words").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(body);
            
            var res = _httpClient.GetAsync("/Word/GetAllWords").Result.Content.ReadAsStringAsync().Result;
            mWords = JsonSerializer.Deserialize<Dictionary<string, int>>(res);
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
            var res = _httpClient.PostAsync(
                    "/Document/GetDocuments", 
                    new StringContent(JsonSerializer.Serialize(wordIds), Encoding.UTF8, "application/json"))
                .Result.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<Dictionary<int, int>>(res);
            //return mDatabase.GetDocuments(wordIds);
        }

        public List<string> GetDocumentDetails(List<int> docIds)
        {
            var res = _httpClient.PostAsync("/Document/GetDocDetails", new StringContent(JsonSerializer.Serialize(docIds), Encoding.UTF8, "application/json"))
                .Result.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<List<string>>(res);
            //return mDatabase.GetDocDetails(docIds);
        }
    }
}