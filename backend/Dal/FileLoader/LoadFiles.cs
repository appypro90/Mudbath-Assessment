using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dal.FileLoader
{
    public class LoadFiles
    {
        public static List<T> LoadData<T>(string fileContent) where T: class
        {
            var prices = JsonConvert.DeserializeObject<List<T>>(fileContent);
            return prices;
        }
    }
}
