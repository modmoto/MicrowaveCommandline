using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.SqlAdapter;
using Microwave.WebServiceModel.SqlAdapter;

namespace Microwave.WebServiceGenerator.Tests.SqlAdapter
{
    [TestClass]
    public class HangfireQueueBuilderTests : TestBase
    {
        [TestMethod]
        public void Build()
        {
            var hangfireQueue = new HangfireQueueBuilder(SqlAdpaterNameSpace).Build(new HangfireQueueClass());
            new FileWriter(SqlAdpaterBasePath).WriteToFile(hangfireQueue.Types[0].Name, "Base", hangfireQueue);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(SqlAdpaterBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../SqlAdapterExpected/Generated/Base/HangfireQueue.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("SqlAdapter/Base/HangfireQueue.g.cs"), @"\s+", String.Empty));
        }
    }
}