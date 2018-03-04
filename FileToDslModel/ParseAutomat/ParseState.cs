using FileToDslModel.Lexer;

namespace FileToDslModel.ParseAutomat
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