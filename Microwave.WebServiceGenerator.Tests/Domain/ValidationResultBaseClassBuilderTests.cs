using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microwave.WebServiceGenerator.Domain;
using Microwave.WebServiceModel.Domain;

namespace Microwave.WebServiceGenerator.Tests.Domain
{
    [TestClass]
    public class ValidationResultBaseClassBuilderTests : TestBase
    {
        [TestMethod]
        public void Write()
        {
            var validationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(DomainNameSpace, DomainBasePath);

            var validationResult = validationResultBaseClassBuilder.Build(new ValidationResultBaseClass());
            new FileWriter(DomainBasePath).WriteToFile("ValidationResult", "Base", validationResult);

            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(DomainBasePath);

            Assert.AreEqual(Regex.Replace(File.ReadAllText("../../../DomainExpected/Generated/Base/ValidationResult.g.cs"), @"\s+", String.Empty),
                Regex.Replace(File.ReadAllText("Domain/Base/ValidationResult.g.cs"), @"\s+", String.Empty));
        }
    }
}
