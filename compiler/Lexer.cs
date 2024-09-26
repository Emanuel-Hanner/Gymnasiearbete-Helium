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


    class Program
    {
        static bool syntaxError = false;
        static string errorMessage = "";



        static void Main(string[] args)
        {
            string fileContent = File.ReadAllText("./compiler/helium.he");

            var tokens = new List<Token>();

            int parenthesisLevel = 0; 
            
            Dictionary<int, Token> parenthesisParent = new Dictionary<int, Token>();

            for (int i = 0; i < fileContent.Length; i++)
            {

    // SIGNS
    // FORMATING
                if (char.IsWhiteSpace(fileContent[i]) || fileContent[i] == '\n') { continue;}
    // SEMICOLONS
                else if (fileContent[i] == ';') { tokens.Add(new Token { Type = TokenType.Semicolon }); }
    // EQUALS  
                else if (fileContent[i] == '=') { tokens.Add(new Token { Type = TokenType.Equals }); }
    // PLUS
                else if (fileContent[i] == '+') { tokens.Add(new Token { Type = TokenType.Plus }); }
    // MINUS
                else if (fileContent[i] == '-') { tokens.Add(new Token { Type = TokenType.Minus }); }
    // DIVIDE
                else if (fileContent[i] == '/') { tokens.Add(new Token { Type = TokenType.Divide }); }
    // TIMES
                else if (fileContent[i] == '*') { tokens.Add(new Token { Type = TokenType.Times }); }
    // MODULO
                else if (fileContent[i] == '%') { tokens.Add(new Token { Type = TokenType.Modulo }); }

    // PARENTHESIS
                else if (fileContent[i] == '(') 
                { 
                    tokens.Add(new Token{ Type = TokenType.Parenthesis, Value = "Start" }); 
                    parenthesisLevel++;
                    parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Parenthesis, Value = "End" };
                }

    // STRINGS  
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

// KEYWORDS
                else if (char.IsLetter(fileContent[i]) && char.IsUpper(fileContent[i]))
                {
    // RETURN
                    if (fileContent[i] == 'R' && i + 6 < fileContent.Length && fileContent.Substring(i, 7) == "Return(")
                    {
                        tokens.Add(new Token { Type = TokenType.Return});
                        parenthesisLevel++;
                        parenthesisParent[parenthesisLevel] = new Token { Type = TokenType.Return, Value = "End" };
                        i += 6; 
                        continue;
                    }

    // PRINT 
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

// VARIABLES
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
                

                else if(fileContent[i] == ')')
                {
                    tokens.Add(parenthesisParent[parenthesisLevel]);
                    parenthesisLevel--;
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
                    Console.WriteLine($"Unknown: {fileContent[i]} at {fileContent.Substring(0, i) + "\n\n"+ fileContent.Substring(i, fileContent.Length- 1 -i)}");
                    
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
