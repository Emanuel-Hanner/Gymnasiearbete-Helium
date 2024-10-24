using System;
using System.IO;

namespace Compiler
{
    public enum ActionType
    {
        Add,
        Define,
        Divide,
        Multiply,
        Print,
        Subtract
    }

    public class Actions
    {
        public ActionType Type { get; set; }
        public string? Value { get; set; }
    }


    public class Parser
    {
        private List<Token> tokens;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public void Parse()
        {
            // Console.WriteLine("Parse Start Here:");



            // I only want to parse While() the tokens include Print-statements.
            
            // If Print statements can't be nested:
                // then the first 'Print: Start' will be the only before the first 'Print: End'.

            // If Print statements doesen't effect code outside the statement: 
                // then every executed/parsed Print-statementet can be removed from the list.

            // What about the semicolon following the Print statement? Will it allways be there?


            // 0. While Loop.
            // 1. Find 'Print: Start'.
            // 2. Find 'Print: End'.
            // 3. Dynamic programming, buildig a parse tree Top-Down and Left-Right.
                // Rooted in definitions (Define) connected with modifications (i.e Add/Multiply for example).


            while (tokens.Any(token => token.Type == TokenType.Print)) // LINQ Logic (Goated)
            {
                int startIndex = -1;
                int endIndex = -1;


                for (int i = 0; i < tokens.Count; i++)
                {
                    if (startIndex == -1 && tokens[i].Type == TokenType.Print)
                    {
                        Console.WriteLine(i);
                        startIndex = i;
                    }
                    else if (endIndex == -1 && tokens[i].Type == TokenType.Print)
                    {
                        Console.WriteLine(i);
                        endIndex = i;
                        break;
                    }
                }

                
                // Dynamic Programming T-T














                
                for (int i = 0; i < tokens.Count; i++)
                {

                    string tokenOutput = "";

                    if (tokens[i].Value != null)
                    {
                        tokenOutput += $"{tokens[i].Type}: {tokens[i].Value}\n";
                    }
                    else if (tokens[i].Type == TokenType.Semicolon)
                    {
                        tokenOutput += $"{tokens[i].Type}\n\n";
                    }
                    else 
                    {
                        tokenOutput += $"{tokens[i].Type}\n";
                    }

                    Console.Write(tokenOutput);
                }
                
                tokens.RemoveRange(startIndex, endIndex-startIndex+1);
                break;
            }
        }
    }
}