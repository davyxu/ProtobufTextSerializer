

namespace ProtobufText
{
    public class StringMatcher : Matcher
    {
        public override Token Match(Tokenizer tz)
        {
            if (tz.Current != '"' && tz.Current != '\'')
                return null;

            tz.Consume(1);

            int beginIndex = tz.Index;

            do
            {
                tz.Consume();

            } while (tz.Current != '\n' && tz.Current != '\0' && tz.Current != '\'' && tz.Current != '"');

            var endIndex = tz.Index;

            tz.Consume();

            return new Token(TokenType.String, tz.Source.Substring(beginIndex, endIndex - beginIndex));
        }

    }
}
