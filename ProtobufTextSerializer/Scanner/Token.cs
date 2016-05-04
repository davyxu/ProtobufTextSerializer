﻿
namespace ProtobufText
{
    public enum TokenType
    {
        None,
        Unknown,
        Whitespace,
        Comment,
        EOF,

        Identifier,
        Number,
        String,


        Comma,          // :
        DoubleQuote,     // "
        SingleQuote,     // '
        Sub,            // -
        LSqualBracket,   // [
        RSqualBracket,   // ]
        LBrace,         // {
        RBrace,         // }
    }
    public class Token
    {
        string _value;
        TokenType _type;

        public Token( TokenType type, string value )
        {
            _type = type;
            _value = value;
        }

        public TokenType Type
        { 
            get { return _type;  } 
        }

        public string Value
        {
            get { return _value;  }
        }

        public float ToNumber()
        {
            return float.Parse(_value);
        }

        public override string ToString()
        {            
            return _type.ToString() + " " + Value;
        }
    }

    public class TokenPos
    {
        public int Pos;

        public TokenPos(int line, int col)
        {

        }
    }
}
