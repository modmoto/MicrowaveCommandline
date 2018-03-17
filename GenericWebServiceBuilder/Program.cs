using System.IO;
using DslModel.Application;
using DslModelToCSharp;
using DslModelToCSharp.Application;
using DslModelToCSharp.Domain;
using DslModelToCSharp.HttpAdapter;
using DslModelToCSharp.SqlAdapter;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;

namespace GenericWebServiceBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wsbFile = args.Length >= 1 ? args[0] : "Schema.wsb";
            var basePathToSolution = args.Length >= 2 ? args[1] : "../";
            var domainNameSpace = "Domain";
            var applicationNameSpace = "Application";
            var sqlAdapterNameSpace = "SqlAdapter";
            var webAdapterNameSpace = "HttpAdapter";
            var domainBasePath = $"{basePathToSolution}{domainNameSpace}/Generated/";
            var applicationBasePath = $"{basePathToSolution}{applicationNameSpace}/Generated/";
            var applicationRealClassesBasePath = $"{basePathToSolution}{applicationNameSpace}/";
            var sqlAdapterBasePath = $"{basePathToSolution}{sqlAdapterNameSpace}/Generated/";
            var webAdapterBasePath = $"{basePathToSolution}{webAdapterNameSpace}/Generated/";
            var injectionBasePath = $"{basePathToSolution}Host";

            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var domainBuilder = new DomainWriter(domainNameSpace, domainBasePath, basePathToSolution);
            var applicationWriter = new ApplicationWriter(applicationNameSpace, applicationBasePath, applicationRealClassesBasePath);
            var sqlAdapterWriter = new SqlAdapterWriter(sqlAdapterNameSpace, sqlAdapterBasePath);
            var webAdapterWriter = new WebAdapterWriter(webAdapterNameSpace, webAdapterBasePath);
            
            var dependencyInjectionWriter = new DependencyInjectionWriter(injectionBasePath);

            using (var reader = new StreamReader(wsbFile))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                domainBuilder.Build(domainTree, domainBasePath);
                applicationWriter.Write(domainTree, applicationBasePath);
                sqlAdapterWriter.Write(domainTree, sqlAdapterBasePath);
                webAdapterWriter.Write(domainTree, webAdapterBasePath);
                dependencyInjectionWriter.Write(domainTree.Classes, domainTree.SynchronousDomainHooks, injectionBasePath);
            }
        }
    }
}