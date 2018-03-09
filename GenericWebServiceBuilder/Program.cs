using System.IO;
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
            var basePath = "../../GeneratedWebService/GeneratedWebService/Domain/Generated/";

            var nameSpaceBuilder = new NameSpaceBuilder();
            var fileWriter = new FileWriter(basePath);
            var classBuilder = new ClassBuilder();
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, classBuilder, new ConstBuilder());
            var domainNameSpace = "Domain";
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), classBuilder,
                fileWriter, new ConstBuilder(), new StaticConstructorBuilder(), nameSpaceBuilder,
                domainNameSpace);
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            var domainEventBaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(),
                fileWriter, nameSpaceBuilder, classBuilder, domainNameSpace);
            var domainBuilder = new DomainBuilder(classWriter, domainEventWriter, domainEventBaseClassBuilder);
            
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                domainBuilder.Build(domainTree, domainNameSpace, basePath);
            }
        }
    }
}