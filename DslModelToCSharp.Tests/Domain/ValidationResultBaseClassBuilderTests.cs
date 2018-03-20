using System;
using System.IO;
using System.Text.RegularExpressions;
using DslModel.Domain;
using DslModelToCSharp.Domain;
using NUnit.Framework;

namespace DslModelToCSharp.Tests.Domain
{
    [TestFixture]
    public class ValidationResultBaseClassBuilderTests : TestBase
    {
        [Test]
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
