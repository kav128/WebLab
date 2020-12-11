using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace lab3.Services
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<StorageService> _logger;
        private readonly IDictionary<string, JToken> _dictionary;

        public StorageService(ILogger<StorageService> logger)
        {
            _logger = logger;
            _dictionary = new SortedDictionary<string, JToken>();
        }
        
        public void PutToken(string key, JToken value) => _dictionary[key] = value;

        public IEnumerable<(string, JToken)> GetAll() => _dictionary.Select(pair => (pair.Key, pair.Value));
        
        public bool IsEmpty() => _dictionary.Keys.Count == 0;
        
        public void Clear() => _dictionary.Clear();
    }
}
