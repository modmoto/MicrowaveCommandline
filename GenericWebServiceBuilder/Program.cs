using System.IO;
using DslModel.Application;
using DslModelToCSharp;
using DslModelToCSharp.Application;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var domainNameSpace = "Domain";
            var applicationNameSpace = "Application";
            var domainBasePath = $"../../GeneratedWebService/{domainNameSpace}/Generated/";
            var applicationBasePath = $"../../GeneratedWebService/{applicationNameSpace}/Generated/";

            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var domainBuilder = new DomainWriter(domainNameSpace, domainBasePath);

            var applicationWriter = new ApplicationWriter(applicationNameSpace, applicationBasePath);
            using (var reader = new StreamReader("Schema.wsb"))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                domainBuilder.Build(domainTree, domainNameSpace, domainBasePath);
                applicationWriter.Write(domainTree, applicationBasePath);
            }
        }
    }
}