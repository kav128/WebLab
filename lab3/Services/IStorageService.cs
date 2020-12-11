using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace lab3.Services
{
    public interface IStorageService
    {
        public void PutToken(string key, JToken value);
        public IEnumerable<(string, JToken)> GetAll();
        public bool IsEmpty();

        public void Clear();
    }
}
