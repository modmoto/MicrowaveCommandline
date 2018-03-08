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

            var fileWriter = new FileWriter(basePath);
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, fileWriter, new ConstBuilder(), new StaticConstructorBuilder(), "Domain");
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

            var domainEventBaseClassBuilder = new DomainEventBaseClassBuilder(new PropBuilder(), new ConstBuilder(), fileWriter, "Domain");
            domainEventBaseClassBuilder.Build(new DomainEventBaseClass().Name, new DomainEventBaseClass().Properties);
        }
    }
}