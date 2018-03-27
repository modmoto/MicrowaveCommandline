using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.LanguageModel.Domain;
using Microwave.WebServiceGenerator.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
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

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/ValidationResult.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Base/ValidationResult.g.cs"), @"\s+", String.Empty));
        }
    }
}
