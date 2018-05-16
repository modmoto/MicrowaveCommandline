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
            var director = new ClassBuilderDirector();
            var validationResult = director.BuildInstance(new ValidationResultBaseClassBuilder(new ValidationResultBaseClass()));
            TestUtils.SnapshotTest(validationResult);
        }
    }
}
