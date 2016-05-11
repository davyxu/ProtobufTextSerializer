﻿

namespace ProtobufText
{    
    class WhitespaceMatcher : Matcher
    {
        public static Token WhiteToken = new Token(TokenType.Whitespace, null);

        bool IsWhiteSpace( char c )
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }

        public override Token Match(Tokenizer tz)
        {
            
            int count = 0;
            for (; ; count++)
            {
                var c = tz.Peek(count);

                if ( c == '\n')
                {
                    tz.IncLine();
                }

                if (!IsWhiteSpace(c))
                    break;                
            }

            if (count == 0)
                return null;

            tz.Consume( count );

            return WhiteToken;
        }
    }
}
