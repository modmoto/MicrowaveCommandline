using Microwave.LanguageParser.Lexer;

namespace Microwave.LanguageParser.ParseAutomat
{
    public abstract class ParseState
    {
        protected ParseState(MicrowaveLanguageParser microwaveLanguageParser)
        {
            MicrowaveLanguageParser = microwaveLanguageParser;
        }

        public MicrowaveLanguageParser MicrowaveLanguageParser { get; }
        public abstract ParseState Parse(DslToken token);
    }
}