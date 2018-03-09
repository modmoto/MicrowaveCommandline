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
        protected DomainEventBaseClassWriter BaseClassBuilder;
        protected string BasePath = "Generated/";
        protected DomainClassWriter ClassWriter;
        protected DomainEventBaseClassWriter DomainEventBaseClassBuilder;
        protected DomainEventWriter DomainEventWriter;
        protected DslParser DslParser;
        protected FileWriter FileWriter;
        protected Parser Parser;
        protected PropBuilder PropBuilder;
        protected StaticConstructorBuilder StaticConstructorBuilder;
        protected Tokenizer Tokenizer;
        protected ValidationResultBaseClassBuilder ValidationResultBaseClassBuilder;

        [TestInitialize]
        public void Setup()
        {
            if (Directory.Exists("DomainActual")) Directory.Delete("DomainActual", true);

            FileWriter = new FileWriter(BasePath);

            DomainEventWriter =
                new DomainEventWriter(new PropBuilder(), FileWriter, new ClassBuilder(), new ConstBuilder());

            ClassWriter = new DomainClassWriter(new InterfaceBuilder(), new PropBuilder(), new ClassBuilder()
                , FileWriter, new ConstBuilder(), new StaticConstructorBuilder(), new NameSpaceBuilder(),
                DomainNameSpace);

            StaticConstructorBuilder = new StaticConstructorBuilder();

            PropBuilder = new PropBuilder();

            Tokenizer = new Tokenizer();

            Parser = new Parser();

            DslParser = new DslParser(Tokenizer, Parser);

            DomainEventBaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(),
                new FileWriter(BasePath), new NameSpaceBuilder(), new ClassBuilder(), DomainNameSpace);

            BaseClassBuilder = new DomainEventBaseClassWriter(new PropBuilder(), new ConstBuilder(), FileWriter,
                new NameSpaceBuilder(), new ClassBuilder(), DomainNameSpace);

            ValidationResultBaseClassBuilder = new ValidationResultBaseClassBuilder(DomainNameSpace, FileWriter,
                StaticConstructorBuilder, PropBuilder, new ConstBuilder(), new NameSpaceBuilder(), new ClassBuilder());
        }
    }
}