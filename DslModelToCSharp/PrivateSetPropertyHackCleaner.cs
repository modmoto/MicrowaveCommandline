using System.IO;
using System.Text;

namespace DslModelToCSharp
{
    public class PrivateSetPropertyHackCleaner
    {
        public void ReplaceHackPropertyNames(string path)
        {
            foreach (var currentDir in Directory.GetDirectories(path))
            {
                foreach (var file in Directory.GetFiles(currentDir))
                {
                    var readAllText = File.ReadAllText(file);
                    var replace = readAllText.Replace("NewHackGuid302315ed-3a05-4992-9f76-4cf075cde515;", "");
                    File.WriteAllText(file, replace);
                }
                ReplaceHackPropertyNames(currentDir);
            }
        }
    }
}