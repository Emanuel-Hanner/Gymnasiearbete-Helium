using System;
using System.IO;

namespace Compiler
{
    public enum TokenType 
    {
        Divide,
        Double,
        Equals,
        Int,
        Minus,
        Modulo,
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


    class Program
    {
        static bool syntaxError = false;
        static string errorMessage = "";



        static void Main(string[] args)
        {
            string fileContent = File.ReadAllText("helium.he");

            var tokens = new List<Token>();

            for (int i = 0; i < fileContent.Length; i++)
            {
                if (char.IsWhiteSpace(fileContent[i]) || fileContent[i] == '\n') // FORMATING
                {
                    continue;
                }
                else if (fileContent[i] == ';') // SEMICOLONS 
                {
                    tokens.Add(new Token { Type = TokenType.Semicolon, Value = null });
                }
                else if (fileContent[i] == '=') // EQUALS
                {
                    tokens.Add(new Token { Type = TokenType.Equals });
                }
                else if (fileContent[i] == '+') // PLUS
                {
                    tokens.Add(new Token { Type = TokenType.Plus });
                }
                else if (fileContent[i] == '-') // MINUS
                {
                    tokens.Add(new Token { Type = TokenType.Minus });
                }
                else if (fileContent[i] == '/') // DIVIDE
                {
                    tokens.Add(new Token { Type = TokenType.Divide });
                }
                else if (fileContent[i] == '*') // TIMES
                {
                    tokens.Add(new Token { Type = TokenType.Times });
                }
                else if (fileContent[i] == '%') // MODULO
                {
                    tokens.Add(new Token { Type = TokenType.Modulo });
                }
                else if (fileContent[i] == '"') // STRINGS
                {
                    string stringContent = "";
                    i++;
                    while (i < fileContent.Length - 1 && fileContent[i] != '"')
                    {
                        stringContent += fileContent[i];
                        i++;
                    }
                    tokens.Add(new Token { Type = TokenType.String, Value = stringContent });
                }
                else if (char.IsLetter(fileContent[i])) // KEYWORDS & VARIABLES
                {
                    // RETURN
                    if (fileContent[i] == 'r' && i + 5 < fileContent.Length && fileContent.Substring(i, 6) == "return")
                    {
                        tokens.Add(new Token { Type = TokenType.Return});
                        i += 5; 
                        continue;
                    }

                    // PRINT 
                    if (fileContent[i] == 'p' && i + 4 < fileContent.Length && fileContent.Substring(i, 5) == "print")
                    {
                        tokens.Add(new Token { Type = TokenType.Print });
                        i += 4; 
                        continue;
                    }

                    string variableName = fileContent[i].ToString();
                    while (i < fileContent.Length - 1 && char.IsLetter(fileContent[i+1]))
                    {
                        variableName += fileContent[i+1];
                        i++;
                    }
                    tokens.Add(new Token { Type = TokenType.Variable, Value = variableName });
                }
                else if (char.IsDigit(fileContent[i])) // NUMBERS 
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

                foreach (var token in tokens)
                {
                    if (token.Value != null)
                    {
                        tokenOutput += $"{token.Type} {token.Value}\n";
                    }
                    else
                    {
                        tokenOutput += $"{token.Type}\n";
                    }
                }

                File.WriteAllText("tokens.txt", tokenOutput);
                Console.WriteLine(tokenOutput);
            }
        }
    }
}
