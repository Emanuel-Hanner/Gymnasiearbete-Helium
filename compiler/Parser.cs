using System;
using System.IO;

namespace Compiler
{
    public class Parser
    {
        private List<Token> tokens;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public void Parse()
        {
            Console.WriteLine("Hello, world!");

            
            string tokenOutput = "";

            foreach (Token token in tokens)
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

                Console.WriteLine(tokenOutput);
            }
        }
    }
}