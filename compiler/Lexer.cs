using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    public enum TokenType 
    {
        Divide,
        Double,
        Equals,
        Int,
        Length,
        Minus,
        Modulo,
        ParenthesisStart,
        ParenthesisEnd,
        Plus,
        Print,
        Return,
        Semicolon,
        String,
        Times,
        Variable
    }

    public class Token
    {
        public TokenType Type { get; set; }
        public string? Value { get; set; }
    }

    public class Lexer
    {
        public List<Token> Tokens { get; private set; } = new List<Token>();

        public void Analyze(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            int parenthesisLevel = 0; 
            Dictionary<int, Token> parenthesisParent = new Dictionary<int, Token>();

            bool syntaxError = false;
            string errorMessage = "";

            for (int i = 0; i < fileContent.Length; i++)
            {
                // Skip whitespace
                if (char.IsWhiteSpace(fileContent[i]) || fileContent[i] == '\n') 
                {
                    continue;
                }
                // Tokenize various symbols
                else if (fileContent[i] == ';') 
                {
                    Tokens.Add(new Token { Type = TokenType.Semicolon });
                }
                else if (fileContent[i] == '=') 
                {
                    Tokens.Add(new Token { Type = TokenType.Equals });
                }
                else if (fileContent[i] == '+') 
                {
                    Tokens.Add(new Token { Type = TokenType.Plus });
                }
                else if (fileContent[i] == '-') 
                {
                    Tokens.Add(new Token { Type = TokenType.Minus });
                }
                else if (fileContent[i] == '/') 
                {
                    Tokens.Add(new Token { Type = TokenType.Divide });
                }
                else if (fileContent[i] == '*') 
                {
                    Tokens.Add(new Token { Type = TokenType.Times });
                }
                else if (fileContent[i] == '%') 
                {
                    Tokens.Add(new Token { Type = TokenType.Modulo });
                }
                else if (fileContent[i] == '(') 
                { 
                    Tokens.Add(new Token { Type = TokenType.ParenthesisStart });
                    parenthesisLevel++;
                    parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.ParenthesisEnd };
                }
                else if (fileContent[i] == ')') 
                {
                    if (parenthesisLevel > 0)
                    {
                        Tokens.Add(parenthesisParent[parenthesisLevel]);
                        parenthesisLevel--;
                    }
                    else
                    {
                        syntaxError = true;
                        errorMessage = $"Unmatched closing parenthesis at character {i}.";
                    }
                }
                else if (fileContent[i] == '"') 
                {
                    string stringContent = "";
                    i++;
                    while (i < fileContent.Length && fileContent[i] != '"')
                    {
                        if (fileContent[i] == '\\' && i + 1 < fileContent.Length) // Handle escape sequences
                        {
                            stringContent += fileContent[i + 1];
                            i++;
                        }
                        else
                        {
                            stringContent += fileContent[i];
                        }
                        i++;
                    }
                    if (fileContent[i] != '"')
                    {
                        syntaxError = true;
                        errorMessage = $"The string needs to be closed! At character {i-1}";
                    }
                    Tokens.Add(new Token { Type = TokenType.String, Value = stringContent });
                }
                else if (char.IsLetter(fileContent[i])) 
                {
                    string identifier = "";
                    while (i < fileContent.Length && (char.IsLetter(fileContent[i]) || char.IsDigit(fileContent[i])))
                    {
                        identifier += fileContent[i];
                        i++;
                    }
                    i--; // Adjust for the increment in the while loop

                    // Check for keywords
                    if (identifier == "Return")
                    {
                        Tokens.Add(new Token { Type = TokenType.Return });
                    }
                    else if (identifier == "Print")
                    {
                        Tokens.Add(new Token { Type = TokenType.Print });
                    }
                    else if (identifier == "Length")
                    {
                        Tokens.Add(new Token { Type = TokenType.Length });
                    }
                    else
                    {
                        Tokens.Add(new Token { Type = TokenType.Variable, Value = identifier });
                    }
                }
                else if (char.IsDigit(fileContent[i])) 
                {
                    string number = fileContent[i].ToString();
                    i++;
                    while (i < fileContent.Length && (char.IsDigit(fileContent[i]) || fileContent[i] == '.'))
                    {
                        number += fileContent[i];
                        i++;
                    }
                    i--; // Adjust for the increment in the while loop

                    if (number.Contains('.'))
                    {
                        Tokens.Add(new Token { Type = TokenType.Double, Value = number });
                    }
                    else 
                    {
                        Tokens.Add(new Token { Type = TokenType.Int, Value = number });
                    }
                }
                else
                {
                    Console.WriteLine($"Unknown: {fileContent[i]} at {i}");
                }
            }

            if (syntaxError) 
            {
                Console.WriteLine(errorMessage);
            }
            else 
            {
                var tokenOutput = "";

                foreach (var token in Tokens)
                {
                    if (token.Value != null)
                    {
                        tokenOutput += $"{token.Type}: {token.Value}\n";
                    }
                    else if (token.Type == TokenType.Semicolon)
                    {
                        tokenOutput += $"{token.Type}\n\n";
                    }
                    else 
                    {
                        tokenOutput += $"{token.Type}\n";
                    }
                }

                File.WriteAllText("./compiler/output/tokens.txt", tokenOutput);
                Console.WriteLine(tokenOutput);
            }
        }
    }
}
