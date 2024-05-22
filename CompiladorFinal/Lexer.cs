using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    class Lexer
    {
        private string _sourceCode;
        private int _position;

        public Lexer(string sourceCode)
        {
            _sourceCode = sourceCode;
            _position = 0;
        }

        public Token NextToken()
        {
            // Omitir los espacios en blanco
            SkipWhitespace();

            // Si llegamos al final del código, devolver un token de fin de archivo
            if (_position >= _sourceCode.Length)
            {
                return new Token(TokenType.Unknown, "Se termino el archivo");
            }

            // Determinar el tipo de token
            char currentChar = _sourceCode[_position];
            if (char.IsLetter(currentChar))
            {
                return ReadIdentifierOrKeyword();
            }
            else if (char.IsDigit(currentChar))
            {
                return ReadNumber();
            }
            else if (IsOperator(currentChar))
            {
                return ReadOperator();
            }
            else if (IsPunctuation(currentChar))
            {
                return ReadPunctuation();
            }
            else
            {
                // Token desconocido
                _position++;
                return new Token(TokenType.Unknown, currentChar.ToString());
            }
        }

        private void SkipWhitespace()
        {
            while (_position < _sourceCode.Length)
            {
                if (char.IsWhiteSpace(_sourceCode[_position]))
                {
                    _position++;
                }
                else if (_sourceCode[_position] == '/' && _position + 1 < _sourceCode.Length && _sourceCode[_position + 1] == '/')
                {
                    // Omitir comentarios de una línea
                    while (_position < _sourceCode.Length && _sourceCode[_position] != '\n')
                    {
                        _position++;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        private Token ReadIdentifierOrKeyword()
        {
            int start = _position;
            while (_position < _sourceCode.Length && (char.IsLetterOrDigit(_sourceCode[_position]) || _sourceCode[_position] == '_'))
            {
                _position++;
            }

            string value = _sourceCode.Substring(start, _position - start);
            if (IsKeyword(value))
            {
                return new Token(TokenType.Keyword, value);
            }
            else
            {
                return new Token(TokenType.Identifier, value);
            }
        }

        private Token ReadNumber()
        {
            int start = _position;
            while (_position < _sourceCode.Length && char.IsDigit(_sourceCode[_position]))
            {
                _position++;
            }

            string value = _sourceCode.Substring(start, _position - start);
            return new Token(TokenType.Number, value);
        }

        private Token ReadOperator()
        {
            int start = _position;
            _position++;

            string value = _sourceCode.Substring(start, _position - start);
            return new Token(TokenType.Operator, value);
        }

        private Token ReadPunctuation()
        {
            int start = _position;
            _position++;

            string value = _sourceCode.Substring(start, _position - start);
            return new Token(TokenType.Punctuation, value);
        }

        private bool IsOperator(char c)
        {
            return "+-*/%=".IndexOf(c) != -1;
        }

        private bool IsPunctuation(char c)
        {
            return "{}()[];,".IndexOf(c) != -1;
        }

        private bool IsKeyword(string value)
        {
            // Lista de palabras clave (puedes agregar más según tu lenguaje)
            string[] keywords = { "if", "else", "while", "for", "int", "float", "double" };
            return Array.IndexOf(keywords, value) != -1;
        }
    }
}