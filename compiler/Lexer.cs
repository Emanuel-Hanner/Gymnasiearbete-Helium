using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

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
        Parenthesis,
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
        public List<Token> tokens { get; private set; } = new List<Token>();

        public void Analyze(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            int parenthesisLevel = 0; 
            Dictionary<int, Token> parenthesisParent = new Dictionary<int, Token>();

            bool syntaxError = false;
            string errorMessage = "";

            for (int i = 0; i < fileContent.Length; i++)
            {
                // Whitespaces
                if (char.IsWhiteSpace(fileContent[i]) || fileContent[i] == '\n') { continue; }

                // Symbols
                else if (fileContent[i] == ';') { tokens.Add(new Token { Type = TokenType.Semicolon });}
                else if (fileContent[i] == '=') { tokens.Add(new Token { Type = TokenType.Equals }); }
                else if (fileContent[i] == '+') { tokens.Add(new Token { Type = TokenType.Plus }); }
                else if (fileContent[i] == '-') { tokens.Add(new Token { Type = TokenType.Minus }); }
                else if (fileContent[i] == '/') { tokens.Add(new Token { Type = TokenType.Divide }); }
                else if (fileContent[i] == '*') { tokens.Add(new Token { Type = TokenType.Times }); }
                else if (fileContent[i] == '%') { tokens.Add(new Token { Type = TokenType.Modulo }); }
                else if (fileContent[i] == '(') 
                { 
                    tokens.Add(new Token{ Type = TokenType.Parenthesis, Value = "Start" }); 
                    parenthesisLevel++;
                    parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Parenthesis, Value = "End" };
                }
                else if(fileContent[i] == ')')
                {
                    tokens.Add(parenthesisParent[parenthesisLevel]);
                    parenthesisLevel--;
                }

                // String
                else if (fileContent[i] == '"') 
                {
                    string stringContent = "";
                    i++;
                    while (i < fileContent.Length - 1 && fileContent[i] != '"')
                    {
                        stringContent += fileContent[i];
                        i++;
                    }
                    if (fileContent[i] != '"')
                    {
                        syntaxError = true;
                        errorMessage = $"The string needs to be closed! At character {i-1}";
                    }
                    tokens.Add(new Token { Type = TokenType.String, Value = stringContent });
                }

                // Keywords
                else if (char.IsLetter(fileContent[i]) && char.IsUpper(fileContent[i]))
                {
                    // Return
                    if (fileContent[i] == 'R' && i + 6 < fileContent.Length && fileContent.Substring(i, 7) == "Return(")
                    {
                        tokens.Add(new Token { Type = TokenType.Return});
                        parenthesisLevel++;
                        parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Return, Value = "End" };
                        i += 6; 
                        continue;
                    }

                    // Print 
                    if (fileContent[i] == 'P' && i + 5 < fileContent.Length && fileContent.Substring(i, 6) == "Print(")
                    {
                        tokens.Add(new Token { Type = TokenType.Print, Value = "Start" });
                        parenthesisLevel++;
                        parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Print, Value = "End" };
                        i += 5; 
                        continue;
                    }
                    
                    // Length
                    if (fileContent[i] == 'L' && i + 6 < fileContent.Length && fileContent.Substring(i, 7) == "Length(")
                    {
                        tokens.Add(new Token { Type = TokenType.Length, Value = "Start" });
                        parenthesisLevel++;
                        parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Length, Value = "End" };
                        i += 6; 
                        continue;
                    }
                }

                // Variables
                else if (char.IsLetter(fileContent[i])) 
                {
                    string variableName = fileContent[i].ToString();
                    while (i < fileContent.Length - 1 && char.IsLetter(fileContent[i+1]))
                    {
                        variableName += fileContent[i+1];
                        i++;
                    }
                    tokens.Add(new Token { Type = TokenType.Variable, Value = variableName });
                }

                // Numbers
                else if (char.IsDigit(fileContent[i])) 
                {
                    string number = fileContent[i].ToString();

                    for (int k = i + 1; k < fileContent.Length; k++)
                    {
                        if (char.IsDigit(fileContent[k]))
                        {
                            number += fileContent[k].ToString();
                        }
                        else if (fileContent[k] == '.')
                        {
                            if (!number.Contains('.') && char.IsDigit(fileContent[k+1]))
                            {
                                number += fileContent.Substring(k, 2).ToString();
                                k += 1;
                            }
                            else 
                            {
                                syntaxError = true;
                                errorMessage = $"That's not a valid number! At character {k}";
                            }
                        }
                        else 
                        {
                            i = k-1;
                            break;
                        }
                    }
                    if (number.Contains('.'))
                    {
                        tokens.Add(new Token { Type = TokenType.Double, Value = number });
                    }
                    else 
                    {
                        tokens.Add(new Token { Type = TokenType.Int, Value = number });
                    }
                }                    
                
                else
                {
                    Console.WriteLine($"Unknown: {fileContent[i]} at {fileContent.Substring(0, i) + "\n\n"+ fileContent.Substring(i, fileContent.Length- 1 -i)}");
                }
            }


            if (syntaxError) 
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}
