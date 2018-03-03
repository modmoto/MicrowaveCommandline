using GenericWebServiceBuilder.FileToDSL.Lexer;

namespace GenericWebServiceBuilder.FileToDSL.ParseAutomat
{
    public abstract class ParseState
    {
        protected ParseState(Parser parser)
        {
            Parser = parser;
        }

        public Parser Parser { get; }
        public abstract ParseState Parse(DslToken token);
    }
}