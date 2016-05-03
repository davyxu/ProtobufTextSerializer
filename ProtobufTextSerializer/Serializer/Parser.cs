using System;

namespace ProtobufTextSerializer
{
    public class Parser
    {
        Lexer _lexer = new Lexer();
        Token _token;

        public Parser( )
        {
            _lexer.AddMatcher(new NumeralMatcher());
            _lexer.AddMatcher(new StringMatcher());

            _lexer.AddMatcher(new WhitespaceMatcher().Ignore());
            _lexer.AddMatcher(new CommentMatcher().Ignore());


            _lexer.AddMatcher(new TokenMatcher(TokenType.Comma, ":"));
            //_lexer.AddMatcher(new TokenMatcher(TokenType.DoubleQuote, "\""));
            //_lexer.AddMatcher(new TokenMatcher(TokenType.SingleQuote, "'"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.Sub, "-"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.LSqualBracket, "["));
            _lexer.AddMatcher(new TokenMatcher(TokenType.RSqualBracket, "]"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.LBrace, "{"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.RBrace, "}"));


            _lexer.AddMatcher(new IdentifierMatcher());

            _lexer.AddMatcher(new UnknownMatcher());
        }

        public void Merge( string src )
        {
            _lexer.Start(src);

            Next();

            string key;

            do
            {
                key = _token.Value;

                Expect(TokenType.Identifier);

                bool isNormalValue = false;

                switch (_token.Type)
                {
                    case TokenType.Comma:
                        {
                            isNormalValue = true;
                        }
                        break;
                    case TokenType.LBrace:
                        {                            
                            OnRepeatedBegin(key);

                            Next();
                            continue;
                        }            
                }

                Next();

                var valueToken = _token;

                if ( isNormalValue )
                {
                    OnSetValue(key, valueToken.Value );
                }

                Next();

                if ( _token.Type == TokenType.RBrace)
                {
                    OnRepeatedEnd();
                    Next();
                }                
                




            } while (_token.Type != TokenType.EOF);
            
        }


        void Expect(TokenType t)
        {
            if (_token.Type != t)
            {
                throw new Exception(string.Format("expect token: {0}", t.ToString()));
            }

            Next();
        }

        void Error(string str)
        {
            throw new Exception(str);
        }

        public override string ToString()
        {
            return _token.ToString();
        }

        void Next()
        {
            _token = _lexer.Read();
        }



        void OnRepeatedBegin(string name)
        {

        }


        void OnSetValue(string name, string value )
        {

        }

        void OnRepeatedEnd()
        {

        }

    }
}
