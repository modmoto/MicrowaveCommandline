using System.IO;
using DslModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    [TestClass]
    public class ValidationResulBaseClassBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(DomainNameSpace, FileWriter,
                new StaticConstructorBuilder(), new PropBuilder(), new ConstBuilder(),
                new NameSpaceBuilder(), new ClassBuilder());

            validationResultBaseClassBuilder.Write(new ValidationResultBaseClass());

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(BasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/ValidationResult.g.cs"),
                File.ReadAllText("Generated/Base/ValidationResult.g.cs"));
        }
    }
}