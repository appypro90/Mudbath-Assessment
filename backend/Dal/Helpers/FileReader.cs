using System;
using System.IO;

namespace Dal.Helpers
{
    public class FileReader
    {
        public static string LoadJsonFile(string fileName)
        {
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            var actualPath = path.Substring(0, path.LastIndexOf("bin", StringComparison.Ordinal));
            var projectPath = new Uri(actualPath).LocalPath;
            var filePath = projectPath + @fileName;
            string json;
            json = ReadJsonFile(filePath);
            return json;
        }

        public static string ReadJsonFile(string fileName)
        {
            string json;
            using (var r = new StreamReader(fileName))
            {
                json = r.ReadToEnd();
            }
            return json;
        }
    }
}
