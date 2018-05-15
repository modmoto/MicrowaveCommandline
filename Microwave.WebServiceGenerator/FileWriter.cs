using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;

namespace Microwave.WebServiceGenerator
{
    public interface IFileWriter
    {
        void WriteToFile(string folderName, CodeNamespace nameSpace, bool isGeneratedFile = true);
    }

    public class FileWriter : IFileWriter
    {
        private readonly string _basePath;

        public FileWriter(string basePath)
        {
            _basePath = basePath;
        }

        public void WriteToFile(string folderName, CodeNamespace nameSpace, bool isGeneratedFile = true)
        {
            var fileName = nameSpace.Types[0].Name;
            var targetUnit = new CodeCompileUnit();
            targetUnit.Namespaces.Add(nameSpace);

            var provider = new CSharpCodeProvider();
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            var ending = isGeneratedFile ? ".g" : "";
            Directory.CreateDirectory($"{_basePath}/{folderName}");
            using (var sourceWriter =
                new StreamWriter($"{_basePath}/{folderName}/{fileName}{ending}.cs"))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }
    }
}