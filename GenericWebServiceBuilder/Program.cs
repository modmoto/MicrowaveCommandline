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

            var nameSpaceBuilder = new NameSpaceBuilder();
            var fileWriter = new FileWriter(domainBasePath);
            var classBuilder = new ClassBuilder();
            IDomainEventWriter domainEventWriter =
                new DomainEventWriter(new PropBuilder(), fileWriter, classBuilder, new ConstBuilder());

            var classWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), classBuilder,
                fileWriter, new ConstBuilder(), new StaticConstructorBuilder(), nameSpaceBuilder,
                domainNameSpace);
            var tokenizer = new Tokenizer();
            var parser = new Parser();
            var valBaseClassBuilder = new ValidationResultBaseClassBuilder(domainNameSpace, fileWriter,
                new StaticConstructorBuilder(), new PropBuilder(), new ConstBuilder(), nameSpaceBuilder, classBuilder);
            var domainEventBaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(),
                fileWriter, nameSpaceBuilder, classBuilder, domainNameSpace);
            var domainBuilder = new DomainBuilder(classWriter, domainEventWriter, domainEventBaseClassBuilder, valBaseClassBuilder, new FileWriter(domainBasePath));

            var hookResultBuilder = new HookResultBuilder(applicationNameSpace, new FileWriter(applicationBasePath), new StaticConstructorBuilder(), new PropBuilder(), new ConstBuilder(), nameSpaceBuilder,classBuilder);
            hookResultBuilder.Write(new HookResultBaseClass());
            new PrivateSetPropertyHackCleaner().ReplaceHackPropertyNames(applicationBasePath);


            var applicationWriter = new ApplicationWriter(new FileWriter(applicationBasePath));
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