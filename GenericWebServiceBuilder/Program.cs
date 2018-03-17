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
            var domainNameSpace = "Domain";
            var applicationNameSpace = "Application";
            var sqlAdapterNameSpace = "SqlAdapter";
            var webAdapterNameSpace = "HttpAdapter";
            var domainBasePath = $"../../GeneratedWebService/{domainNameSpace}/Generated/";
            var applicationBasePath = $"../../GeneratedWebService/{applicationNameSpace}/Generated/";
            var applicationRealClassesBasePath = $"../../GeneratedWebService/{applicationNameSpace}/";
            var sqlAdapterBasePath = $"../../GeneratedWebService/{sqlAdapterNameSpace}/Generated/";
            var webAdapterBasePath = $"../../GeneratedWebService/{webAdapterNameSpace}/Generated/";
            var injectionBasePath = "../../GeneratedWebService/GeneratedWebService";

            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var domainBuilder = new DomainWriter(domainNameSpace, domainBasePath, "../../GeneratedWebService");
            var applicationWriter = new ApplicationWriter(applicationNameSpace, applicationBasePath, applicationRealClassesBasePath);
            var sqlAdapterWriter = new SqlAdapterWriter(sqlAdapterNameSpace, sqlAdapterBasePath);
            var webAdapterWriter = new WebAdapterWriter(webAdapterNameSpace, webAdapterBasePath);
            
            var dependencyInjectionWriter = new DependencyInjectionWriter(injectionBasePath);

            using (var reader = new StreamReader("Schema.wsb"))
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