using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    // Tipos de tokens
    enum TokenType
    {
        Identifier,
        Number,
        Operator,
        Keyword,
        Punctuation,
        Unknown
    }
}
