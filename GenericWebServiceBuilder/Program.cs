using System.IO;
using GenericWebServiceBuilder.DslToCSharp;
using GenericWebServiceBuilder.FileToDSL;
using GenericWebServiceBuilder.FileToDSL.Lexer;
using GenericWebServiceBuilder.FileToDSL.ParseAutomat;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var classWriter = new DomainClassWriter(new InterfaceParser(), new PropertyParser(), new ClassParser());
            var tokenizer = new Tokenizer();
            var parser = new Parser();

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass);

                foreach (var domainEvent in domainTree.Events)
                    classWriter.Write(domainEvent);
            }
        }
    }
}