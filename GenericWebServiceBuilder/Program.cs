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
            IDomainEventWriter domainEventWriter = new DomainEventWriter(new PropertyParser(), new FileWriter(),  new ClassParser());
            var classWriter = new DomainClassWriter(new InterfaceParser(), new PropertyParser(), new ClassParser(), domainEventWriter, new FileWriter());
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass);

                classWriter.Write(new ValidationResultBaseClass());
                classWriter.Write(new DomainEventBaseClass());
            }
        }
    }
}