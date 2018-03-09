using System.IO;
using DslModel.Application;
using DslModelToCSharp.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Application
{
    [TestClass]
    public class HookResultBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var hookResultBuilder = new HookResultBuilder(ApplicationNameSpace, new FileWriter(ApplicationNameSpace), StaticConstructorBuilder, PropBuilder, new ConstBuilder(), new NameSpaceBuilder(), new ClassBuilder() );

            hookResultBuilder.Write(new HookResultBaseClass());


            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePathApplication);

            Assert.AreEqual(File.ReadAllText("../../../ApplicationExpected/Generated/Base/HookResult.g.cs"),
                File.ReadAllText("Application/Base/HookResult.g.cs"));
        }
    }
}