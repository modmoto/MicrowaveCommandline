using System.IO;
using DslModel;
using DslModelToCSharp;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var basePath = "../GeneratedWebService/Domain/Generated/";

            var nameSpaceBuilder = new NameSpaceBuilder();
            var fileWriter = new FileWriter(basePath);
            var classBuilder = new ClassBuilder();
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, classBuilder, new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), classBuilder,
                domainEventWriter, fileWriter, new ConstBuilder(), new StaticConstructorBuilder(), nameSpaceBuilder,
                "Domain");
            var tokenizer = new Tokenizer();
            var parser = new Parser();


            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass, basePath);

                classWriter.Write(new ValidationResultBaseClass());
                classWriter.Write(new CreationResultBaseClass());
            }

            var domainEventBaseClassBuilder = new DomainEventBaseClassBuilder(new PropBuilder(), new ConstBuilder(),
                fileWriter, nameSpaceBuilder, classBuilder, "Domain");
            domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
        }
    }
}