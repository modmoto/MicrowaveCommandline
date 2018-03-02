using System.IO;
using GenericWebServiceBuilder.DomainToCSharp;
using GenericWebServiceBuilder.FileToDSL;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var classWriter = new DomainClassWriter(new InterfaceParser(), new PropertyParser(), new ClassParser());

            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();
                var dslParser = new DslParser();
                var domainTree = dslParser.Parse(content);

                foreach (var domainClass in domainTree.Classes)
                    classWriter.Write(domainClass);

                foreach (var domainEvent in domainTree.Events)
                    classWriter.Write(domainEvent);
            }
        }
    }
}