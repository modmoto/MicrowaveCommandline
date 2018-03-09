using System.IO;
using FileToDslModel;
using FileToDslModel.Lexer;
using FileToDslModel.ParseAutomat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DslModelToCSharp.Tests
{
    public class TestBase
    {
        protected readonly string DomainNameSpace = "Domain";
        protected readonly string BasePathDomain = "Domain/";
        protected readonly string ApplicationNameSpace = "Application";
        protected readonly string BasePathApplication = "Application/";
        protected DomainEventBaseClassWriter BaseClassBuilder;
        protected DomainClassWriter ClassWriter;
        protected DomainEventBaseClassWriter DomainEventBaseClassBuilder;
        protected DomainEventWriter DomainEventWriter;
        protected DslParser DslParser;
        protected Parser Parser;
        protected PropBuilder PropBuilder;
        protected StaticConstructorBuilder StaticConstructorBuilder;
        protected Tokenizer Tokenizer;
        protected ValidationResultBaseClassBuilder ValidationResultBaseClassBuilder;

        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);

            DomainEventWriter =
                new DomainEventWriter(new PropBuilder(), new FileWriter(DomainNameSpace), new ClassBuilder(), new ConstBuilder());

            ClassWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder()
                , new FileWriter(DomainNameSpace), new ConstBuilder(), new StaticConstructorBuilder(), new NameSpaceBuilder(),
                DomainNameSpace);

            StaticConstructorBuilder = new StaticConstructorBuilder();

            PropBuilder = new PropBuilder();

            Tokenizer = new Tokenizer();

            Parser = new Parser();

            DslParser = new DslParser(Tokenizer, Parser);

            DomainEventBaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(),
                new FileWriter(BasePathDomain), new NameSpaceBuilder(), new ClassBuilder(), DomainNameSpace);

            BaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(), new FileWriter(DomainNameSpace),
                new NameSpaceBuilder(), new ClassBuilder(), DomainNameSpace);

            ValidationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(DomainNameSpace, new FileWriter(DomainNameSpace),
                StaticConstructorBuilder, PropBuilder, new ConstBuilder(), new NameSpaceBuilder(), new ClassBuilder());
        }
    }
}