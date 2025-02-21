    using System;

    namespace Compiler
    {
        class Program
        {
            static void Main(string[] args)
            {
                var lexer = new Lexer();
                lexer.Analyze("./compiler/helium.he");
                
                var parser = new Parser(lexer.tokens);
                var ast = parser.Parse();

                // Prints the Tokens & AST
                PrintTokens(lexer.tokens);
                PrintAst(ast, 0);
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


            static void PrintAst(Node node, int indentLevel)
            {
                Console.WriteLine("\n\n------------ AST Output: ------------\n\n");

                string indent = new string(' ', indentLevel * 2);

                if (node is PrintNode printNode)
                {
                    Console.WriteLine($"{indent}Print:");
                    PrintAst(printNode.Value, indentLevel + 1);
                }
                else if (node is StringNode stringNode)
                {
                    Console.WriteLine($"{indent}String: {stringNode.Value}");
                }
                else
                {
                    Console.WriteLine($"{indent}Unknown node type");
                }
            }

            
        }
    }
