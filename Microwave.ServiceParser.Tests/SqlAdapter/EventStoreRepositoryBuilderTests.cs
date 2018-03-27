using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.ServiceParser.SqlAdapter;

namespace Microwave.ServiceParser.Tests.SqlAdapter
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

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/EventStoreRepository.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Base/EventStoreRepository.g.cs"), @"\s+", String.Empty));
        }
    }
}