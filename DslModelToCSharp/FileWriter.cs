using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;

namespace DslModelToCSharp
{
    public interface IFileWriter
    {
        void WriteToFile(string fileName, string folderName, CodeNamespace nameSpace);
    }

    public class FileWriter : IFileWriter
    {
        public void WriteToFile(string fileName, string folderName, CodeNamespace nameSpace)
        {
            var targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(nameSpace);

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            Directory.CreateDirectory($"../GeneratedWebService/Domain/Generated/{folderName}");
            using (var sourceWriter =
                new StreamWriter($"../GeneratedWebService/Domain/Generated/{folderName}/{fileName}.g.cs"))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }
    }
}