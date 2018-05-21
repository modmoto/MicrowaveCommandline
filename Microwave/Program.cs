using System.IO;
using Microwave.LanguageParser;
using Microwave.LanguageParser.Lexer;
using Microwave.LanguageParser.ParseAutomat;
using Microwave.WebServiceGenerator;
using Microwave.WebServiceGenerator.Application;
using Microwave.WebServiceGenerator.Domain;
using Microwave.WebServiceGenerator.HttpAdapter;
using Microwave.WebServiceGenerator.SqlAdapter;

namespace Microwave
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var wsbFile = args.Length >= 1 ? args[0] : "Schema.mic";
            var basePathToSolution = args.Length >= 2 ? args[1] : "../";
            var domainNameSpace = "Domain";
            var applicationNameSpace = "Application";
            var sqlAdapterNameSpace = "SqlAdapter";
            var webAdapterNameSpace = "HttpAdapter";
            var domainBasePath = $"{basePathToSolution}{domainNameSpace}/Generated/";
            var applicationBasePath = $"{basePathToSolution}{applicationNameSpace}/Generated/";
            var applicationRealClassesBasePath = $"{basePathToSolution}{applicationNameSpace}/";
            var domainRealClassesBasePath = $"{basePathToSolution}{domainNameSpace}/";
            var sqlAdapterBasePath = $"{basePathToSolution}{sqlAdapterNameSpace}/Generated/";
            var webAdapterBasePath = $"{basePathToSolution}{webAdapterNameSpace}/Generated/";
            var asynHostBasePath = $"{basePathToSolution}AsyncHost";
            var injectionBasePath = $"{basePathToSolution}Host";

            var tokenizer = new MicrowaveLanguageTokenizer();
            var parser = new MicrowaveLanguageParser();
            var domainBuilder = new DomainWriter(domainBasePath, domainRealClassesBasePath);
            var applicationWriter = new ApplicationWriter(applicationNameSpace, applicationBasePath, applicationRealClassesBasePath);
            var sqlAdapterWriter = new SqlAdapterWriter(sqlAdapterNameSpace, sqlAdapterBasePath);
            var webAdapterWriter = new HttpAdapterWriter(webAdapterNameSpace, webAdapterBasePath);
            var dependencyInjectionWriterAsyncHost = new DependencyInjectionBuilderAsyncHost();

            var dependencyInjectionWriter = new DependencyInjectionBuilderHost();

            using (var reader = new StreamReader(wsbFile))
            {
                var content = reader.ReadToEnd();

                var dslParser = new DslParser(tokenizer, parser);
                var domainTree = dslParser.Parse(content);

                domainBuilder.Write(domainTree, domainBasePath);
                applicationWriter.Write(domainTree);
                sqlAdapterWriter.Write(domainTree);
                webAdapterWriter.Write(domainTree);
                var codeNamespace = dependencyInjectionWriter.Build(domainTree.Classes, domainTree.SynchronousDomainHooks, domainTree.OnChildHooks);
                new FileWriter(injectionBasePath).WriteToFile("Base/", codeNamespace);
                var write = dependencyInjectionWriterAsyncHost.Write(domainTree.Classes, domainTree.AsyncDomainHooks);
                new FileWriter(asynHostBasePath).WriteToFile("Base/", write);
            }
        }
    }
}