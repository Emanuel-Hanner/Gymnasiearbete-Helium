using System;
using System.IO;

namespace Compiler
{
    public enum TokenType 
    {
        Return,
        Print,
        String,
        Int,
        Double,
        Semi
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
                    tokens.Add(new Token { Type = TokenType.Semi, Value = null });
                }
                else if (fileContent[i] == '"') // STRINGS
                {
                    
                }
                else if (char.IsLetter(fileContent[i])) // KEYWORDS
                {
                    // RETURN
                    if (fileContent[i] == 'r' && i + 5 < fileContent.Length && fileContent.Substring(i, 6) == "return")
                    {
                        tokens.Add(new Token { Type = TokenType.Return});
                        i += 5; 
                    }

                    // PRINT 
                    if (fileContent[i] == 'p' && i + 4 < fileContent.Length && fileContent.Substring(i, 5) == "print")
                    {
                        tokens.Add(new Token { Type = TokenType.Print });
                        i += 4; 
                    }
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
                foreach (var token in tokens)
                {
                    Console.WriteLine($"Token Type: {token.Type}, Value: {token.Value}");
                }
            }
        }
    }
}
