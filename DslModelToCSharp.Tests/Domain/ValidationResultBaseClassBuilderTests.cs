using System.IO;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests.Domain
{
    [TestClass]
    public class ValidationResultBaseClassBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(DomainNameSpace, DomainBasePath);

            validationResultBaseClassBuilder.Write(new ValidationResultBaseClass());

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(File.ReadAllText("../../../DomainExpected/Generated/Base/ValidationResult.g.cs"),
                File.ReadAllText("Domain/Base/ValidationResult.g.cs"));
        }
    }
}