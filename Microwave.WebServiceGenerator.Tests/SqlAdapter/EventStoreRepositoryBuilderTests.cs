using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class EventStoreRepositoryBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var classFactory = new ClassBuilderDirector();
            var buildInstance = classFactory.BuildInstance(new EventStoreRepositoryBuilder(new EventStoreRepository()));
            new FileWriter(SqlAdpaterNameSpace).WriteToFile("Base/", buildInstance);
           
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/EventStoreRepository.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Base/EventStoreRepository.g.cs"), @"\s+", String.Empty));
        }
    }
}