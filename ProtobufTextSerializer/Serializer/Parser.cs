using System;
using System.Collections.Generic;
using SharpLexer;

namespace ProtobufText
{
    enum TokenType
    {
        Unknown = 0,
        Numeral,
        String,

        Whitespace,
        LineEnd,
        Comment,

        Identifier,


        Comma,          // :
        DoubleQuote,     // "
        SingleQuote,     // '
        LSqualBracket,   // [
        RSqualBracket,   // ]
        LBrace,         // {
        RBrace,         // }
    }


    public class Parser
    {
        Lexer _lexer = new Lexer();
        Token _token;

        
        Message _msg;

        Stack<Message> _msgStack = new Stack<Message>();

        public Parser( )
        {
            _lexer.AddMatcher(new NumeralMatcher((int)TokenType.Numeral));
            _lexer.AddMatcher(new StringMatcher((int)TokenType.String));

            _lexer.AddMatcher(new WhitespaceMatcher((int)TokenType.Whitespace).Ignore());
            _lexer.AddMatcher(new LineEndMatcher((int)TokenType.LineEnd).Ignore());
            _lexer.AddMatcher(new UnixStyleCommentMatcher((int)TokenType.Comment).Ignore());


            _lexer.AddMatcher(new SignMatcher((int)TokenType.Comma, ":"));
            _lexer.AddMatcher(new SignMatcher((int)TokenType.LSqualBracket, "["));
            _lexer.AddMatcher(new SignMatcher((int)TokenType.RSqualBracket, "]"));
            _lexer.AddMatcher(new SignMatcher((int)TokenType.LBrace, "{"));
            _lexer.AddMatcher(new SignMatcher((int)TokenType.RBrace, "}"));

            _lexer.AddMatcher(new IdentifierMatcher((int)TokenType.Identifier));

            _lexer.AddMatcher(new UnknownMatcher((int)TokenType.Unknown));
        }

        TokenType GetTokenType( )
        {
            return (TokenType)_token.MatcherID;
        }

        public void Merge( string src, Message msg )
        {
            _msg = msg;

            _lexer.Start(src);

            Next();

            string key;

            do
            {
                key = _token.Value;

                switch (GetTokenType())
                {
                    case TokenType.Identifier:
                        {
                            Expect(TokenType.Identifier);

                            bool isNormalValue = false;

                            switch (GetTokenType())
                            {
                                case TokenType.Comma:
                                    {
                                        isNormalValue = true;
                                    }
                                    break;
                                case TokenType.LBrace:
                                    {
                                        OnMsgBegin(key);

                                        Next();
                                        continue;
                                    }
                            }

                            Next();

                            var valueToken = _token;

                            if (isNormalValue)
                            {
                                OnSetValue(key, valueToken.Value);
                            }

                            Next();

                            if (GetTokenType() == TokenType.RBrace)
                            {
                                OnMsgEnd();
                                Next();
                            }                
                        }
                        break;
                    case TokenType.RBrace:
                        {
                            OnMsgEnd();
                            Next();
                        }
                        break;
                }




            } while (GetTokenType() != 0);
            
        }


        void Expect(TokenType t)
        {
            if (GetTokenType() != t)
            {
                throw new Exception(string.Format("expect token: {0} @ {1}", t.ToString(), _lexer.ToString() ));
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

        void OnMsgBegin(string name)
        {
            _msgStack.Push(_msg);
            _msg = _msg.AddMessage(name);
        }

        void OnSetValue(string name, string value )
        {
            try
            {
                _msg.SetValue(name, value);
            }
            catch( Exception ex )
            {
                string err = ex.ToString() + _lexer.ToString();
                throw new Exception(err);
            }
            
        }

        void OnMsgEnd()
        {
            _msg = _msgStack.Pop();
        }

    }
}
