using System;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var lexer = new Lexer();
            var tokens = lexer.Analyze("./compiler/helium.he");
            
            var parser = new Parser();
            var ast = parser.Parse(tokens);

            // Prints the Tokens & AST
            PrintTokens(tokens);
            PrintAst(ast);
            
        }


        static void PrintTokens(List<Token> tokens)
        {

            String tokenOutput = "";

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
            }

            File.WriteAllText("./compiler/output/tokens.txt", tokenOutput);
            Console.WriteLine("\n\n------------ Token Output: ------------\n\n" + tokenOutput);
        }

        
        static void PrintAst(List<Node> ast)
        {
            Console.WriteLine("\n\n------------ AST Output: ------------\n");

            foreach (Node node in ast) {

                if (node is PrintNode printNode)
                {
                    Console.WriteLine($"{Indentation(printNode.ExecutionOrder)}Print:");
                }
                else if (node is StringNode stringNode)
                {
                    Console.WriteLine($"{Indentation(stringNode.ExecutionOrder)}String: {stringNode.Value}");
                }
                else
                {
                    Console.WriteLine($"Unknown node type");
                }
            } 
        }

        static string Indentation(int amount)
        {
            return new string(' ', 2 * amount);
        }
    }
}
