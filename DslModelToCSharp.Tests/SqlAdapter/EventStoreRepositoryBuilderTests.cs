using System.IO;
using DslModelToCSharp.SqlAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.SqlAdapter
{
    [TestClass]
    public class EventStoreRepositoryBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var classFactory = new ClassFactory();
            var buildInstance = classFactory.BuildInstance(new EventStoreRepositoryBuilder(SqlAdpaterNameSpace));
            new FileWriter(SqlAdpaterNameSpace).WriteToFile(buildInstance.Types[0].Name, "Base/", buildInstance);
           
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/EventStoreRepository.g.cs"),
                File.ReadAllText("SqlAdapter/Base/EventStoreRepository.g.cs"));
        }
    }
}