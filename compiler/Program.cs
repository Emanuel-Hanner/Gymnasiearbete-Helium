using System;
using System.Formats.Tar;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var lexer = new Lexer();
            var tokens = lexer.Analyze("./compiler/helium.he");
            PrintTokens(tokens);

            var parser = new Parser();
            var ast = parser.Parse(tokens);
            PrintAst(ast);

            var generator = new Generator();
            var assembly = generator.Generate(ast);
            File.WriteAllText("./compiler/output/assembly.asm", assembly);
            System.Console.WriteLine("\n\n------------ Generator Output: ------------\n\n" + assembly);
            System.Console.WriteLine("\n\n------------ Assembly Output: ------------\n\n");
        }


        static void PrintTokens(List<Token> tokens)
        {

            String tokenOutput = "\n\n------------ Token Output: ------------\n\n";

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
            Console.WriteLine(tokenOutput);
        }

        
        static void PrintAst(List<Node> ast)
        {
            String astOutput = "\n\n------------ AST Output: ------------\n";

            foreach (Node node in ast) 
            {
                // A "Switch Expression" - a handy and intuitive replacement for "Switch Statements"
                astOutput += node switch
                {
                    PrintNode printNode => $"{Indentation(printNode.ExecutionOrder)}Print:\n",
                    StringNode stringNode => $"{Indentation(stringNode.ExecutionOrder)}String: {stringNode.Value}\n",
                    PlusNode plusNode => $"{Indentation(plusNode.ExecutionOrder)}Plus\n", // removing these "\n" can give better styling
                    MinusNode minusNode => $"{Indentation(minusNode.ExecutionOrder)}Minus\n", // removing these "\n" can give better styling
                    IntNode intNode => $"{Indentation(intNode.ExecutionOrder)}Int: {intNode.Value}\n",
                    VariableNode variableNode => $"{Indentation(variableNode.ExecutionOrder)}Variable: {variableNode.VariableName}\n",
                    _ => throw new Exception("Unknown Node-type!"), // "_" (blank) represents the default/"else" option 
                };
            } 

            File.WriteAllText("./compiler/output/ast.txt", astOutput);
            Console.WriteLine(astOutput);
        }

        static string Indentation(int amount)
        {
            string indentation = "";

            if (amount == 0)
            {
                indentation += "\n";
            }

            return indentation + new string(' ', 2 * amount);
        }
    }
}
