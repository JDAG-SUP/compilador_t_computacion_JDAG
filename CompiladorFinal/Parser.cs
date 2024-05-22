using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorFinal
{
    // Analizador sintáctico
        class Parser
        {
            private Lexer _lexer;
            private Token _currentToken;
            private List<string> _identifiers = new List<string>();

            public Parser(Lexer lexer)
            {
                _lexer = lexer;
                _currentToken = _lexer.NextToken();
            }

            public void Parse()
            {
                // Llamar a la regla de inicio
                Program();
            }

            private void Program()
            {
                // Analizar las declaraciones de variables
                while (_currentToken.Type == TokenType.Keyword && (_currentToken.Value == "int" || _currentToken.Value == "float" || _currentToken.Value == "double"))
                {
                    VariableDeclaration();
                }

                // Analizar las sentencias
                while (_currentToken.Type != TokenType.Unknown)
                {
                    Statement();
                }
            }

            private void VariableDeclaration()
            {
                // Consumir el tipo de dato
                TokenType dataType = _currentToken.Type;
                Consume(TokenType.Keyword);

                // Consumir el identificador
                Consume(TokenType.Identifier, _identifiers);

                // Consumir el punto y coma
                Consume(TokenType.Punctuation, ";");
            }

        private void Statement()
        {
            switch (_currentToken.Type)
            {
                case TokenType.Keyword:
                    if (_currentToken.Value == "if")
                    {
                        IfStatement();
                    }
                    else if (_currentToken.Value == "while")
                    {
                        WhileStatement();
                    }
                    break;
                case TokenType.Identifier:
                    AssignmentStatement();
                    break;
                default:
                    // Ignorar sentencias no reconocidas
                    Consume();
                    break;
            }
        }

        private void IfStatement()
        {
            // Consumir la palabra clave "if"
            Consume(TokenType.Keyword, "if");

            // Consumir la expresión entre paréntesis
            Consume(TokenType.Punctuation, "(");
            Expression();
            Consume(TokenType.Punctuation, ")");

            // Consumir el bloque de código entre llaves
            Block();

            // Verificar si hay una cláusula "else"
            if (_currentToken.Type == TokenType.Keyword && _currentToken.Value == "else")
            {
                Consume(TokenType.Keyword, "else");
                Block();
            }
        }

        private void WhileStatement()
            {
                // Consumir la palabra clave "while"
                Consume(TokenType.Keyword, "while");

                // Consumir la expresión entre paréntesis
                Consume(TokenType.Punctuation, "(");
                Expression();
                Consume(TokenType.Punctuation, ")");

                // Consumir el bloque de código entre llaves
                Block();
            }

        private void AssignmentStatement()
        {
            string identifier = _currentToken.Value;
            Consume(TokenType.Identifier);

            if (!_identifiers.Contains(identifier))
            {
                throw new Exception($"Error semántico: el identificador '{identifier}' no ha sido declarado.");
            }

            Consume(TokenType.Operator, "=");
            Expression();
            Consume(TokenType.Punctuation, ";");
        }
        private void Expression()
        {
            Term();
            while (_currentToken.Type == TokenType.Operator && (_currentToken.Value == "+" || _currentToken.Value == "-"))
            {
                Consume();
                Term();
            }
        }

        private void Term()
        {
            Factor();
            while (_currentToken.Type == TokenType.Operator && (_currentToken.Value == "*" || _currentToken.Value == "/" || _currentToken.Value == "%"))
            {
                Consume();
                Factor();
            }
        }

        private void Factor()
        {
            if (_currentToken.Type == TokenType.Number)
            {
                Consume();
            }
            else if (_currentToken.Type == TokenType.Identifier)
            {
                Consume();
            }
            else if (_currentToken.Type == TokenType.Punctuation && _currentToken.Value == "(")
            {
                Consume();
                Expression();
                Consume(TokenType.Punctuation, ")");
            }
            else
            {
                throw new Exception($"Error de sintaxis: se esperaba un número, identificador o paréntesis, pero se encontró {_currentToken.Type} {_currentToken.Value}");
            }
        }
        private void Block()
        {
            // Consumir la llave de apertura
            Consume(TokenType.Punctuation, "{");

            // Analizar el bloque de código
            while (_currentToken.Type != TokenType.Punctuation || (_currentToken.Type == TokenType.Punctuation && _currentToken.Value != "}"))
            {
                Statement();
            }

            // Consumir la llave de cierre
            Consume(TokenType.Punctuation, "}");
        }

        private void Consume(TokenType expectedType = TokenType.Unknown, string? expectedValue = null)
        {
            // Verificar si el token actual coincide con el tipo y valor esperados
            if (_currentToken.Type == expectedType && (expectedValue == null || _currentToken.Value == expectedValue))
            {
                _currentToken = _lexer.NextToken();
            }
            else
            {
                // Imprimir un mensaje de error y avanzar al siguiente token
                Console.WriteLine($"Error de sintaxis: se esperaba {expectedType} {expectedValue}, pero se encontró {_currentToken.Type} {_currentToken.Value}");
                _currentToken = _lexer.NextToken();
            }
        }
        private void Consume(TokenType expectedType, List<string> identifierList)
            {
                // Verificar si el token actual es un identificador
                if (_currentToken.Type == expectedType)
                {
                    // Agregar el identificador a la lista
                    string identifier = _currentToken.Value;
                    identifierList.Add(identifier);

                    _currentToken = _lexer.NextToken();
                }
                else
                {
                    // Manejar el error de sintaxis
                    throw new Exception($"Error de sintaxis: se esperaba un identificador, pero se encontró {_currentToken.Type} {_currentToken.Value}");
                }
            }
        }
    }

