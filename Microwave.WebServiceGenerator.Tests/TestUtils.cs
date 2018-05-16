using System.CodeDom;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microwave.WebServiceGenerator.Tests
{
    public static class TestUtils
    {
        public static void SnapshotTest(CodeNamespace codeNamespace, bool isGeneratedFile = true)
        {
            new FileWriter("Generated").WriteToFile(codeNamespace.Name, codeNamespace, isGeneratedFile);
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames("Generated");
            var ending = isGeneratedFile ? ".g" : "";
            var fileName = $"{codeNamespace.Types[0].Name}{ending}.cs";
            var expectedFile = $"../../../GeneratedExpected/{codeNamespace.Name}/{fileName}";
            var actualFile = $"Generated/{codeNamespace.Name}/{fileName}";

            if (File.Exists(expectedFile))
            {
                Assert.AreEqual(File.ReadAllText(expectedFile), File.ReadAllText(actualFile));
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(expectedFile));
                File.Copy(actualFile, expectedFile);
            }
        }
    }
}