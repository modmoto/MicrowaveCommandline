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
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(), new ClassBuilder(), new ConstBuilder());
            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder(),
                domainEventWriter, new FileWriter(), new ConstBuilder(), new StaticConstructorBuilder());
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            var basePath = "../GeneratedWebService/Domain/Generated/";

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass, basePath);

                classWriter.Write(new ValidationResultBaseClass(), basePath);
                classWriter.Write(new DomainEventBaseClass(), basePath);
                classWriter.Write(new CreationResultBaseClass(), basePath);
            }
        }
    }
}