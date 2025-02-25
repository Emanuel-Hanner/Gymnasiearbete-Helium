using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;


namespace Compiler
{
    public abstract class Node { } // An "abstract class" - a class of classes / container for subclasses

    public class StringNode : Node
    {
        public int ExecutionOrder { get; }
        public string Value { get; }

        public StringNode(int exceptionOrder, string value)
        {
            ExecutionOrder = exceptionOrder;
            Value = value;
        }
    }

 
    public class PrintNode : Node
    {
        public int ExecutionOrder { get; }

        public PrintNode(int exceptionOrder)
        {
            ExecutionOrder = exceptionOrder;
        }
    }


    public class VariabelNode : Node
    {
        public int ExecutionOrder { get; }
        public string VariableName { get; }

        public VariabelNode(int exceptionOrder, String name)
        {
            ExecutionOrder = exceptionOrder;
            VariableName = name;
        }
    }

    public class PlusNode : Node
    {
        public int ExecutionOrder { get; }

        public PlusNode(int exceptionOrder)
        {
            ExecutionOrder = exceptionOrder;
        }
    }

    public class MinusNode : Node
    {
        public int ExecutionOrder { get; }

        public MinusNode(int exceptionOrder)
        {
            ExecutionOrder = exceptionOrder;
        }
    }

    public class IntNode : Node
    {
        public int ExecutionOrder { get; }
        public int Value { get; }

        public IntNode(int exceptionOrder, int value)
        {
            ExecutionOrder = exceptionOrder;
            Value = value;
        }
    }



    public class Parser
    {
        public List<Token> tokens;
        public List<Node> ast = new List<Node>();

        private int executionOrder;


        // The Parse-function called from Program.cs
        public List<Node> Parse(List<Token> importedTokens)
        {
            tokens = importedTokens;

            while (tokens.Count > 0) { ast.AddRange(ParseStatement(0)); }
        
            return ast;
        }


        // Deconstructs the Program into defined Statements 
        private List<Node> ParseStatement(int localOrder)
        {
            Token currentToken = tokens[0];

            if (Match(currentToken, TokenType.Print, "Start"))
            {  
                Consume();
                return [new PrintNode(localOrder), .. ParsePrint(localOrder += 1)]; 
                // ".." = the spread operator. It allows for easy merging of collections - a Collection Expression introduced with C#12 
            }
            else if (Match(currentToken, TokenType.Variable) && currentToken.Value != null)
            {
                return [new VariabelNode(localOrder, currentToken.Value), .. ParseVariable(localOrder += 1)];
            }  
            else
            {
                throw new Exception("Unexpected token: A Statement is required");
            }
        }


        // Parses Print Expressions 
        private List<Node> ParsePrint(int localOrder)
        {
            List<Node> printTree = new List<Node>();
            Token token = tokens[0];

            // Takes the first term
            if (Match(token, TokenType.String))
            {  
                printTree.Add(new StringNode(localOrder, token.Value));
                Consume(); 
            }
            else if (Match(token, TokenType.Int))
            {
                printTree.Add(new IntNode(localOrder, int.Parse(token.Value)));
                Consume(); 
            }
            else 
            {
                throw new Exception("Unexpected token in expression: Must be String or Integer");
            }
            
            // Takes pairs of +/- and Terms untill the PrintStatement ends
            while (tokens.Count > 0 && !(Match(tokens[0], TokenType.Print, "End") && Match(tokens[1], TokenType.Semicolon)))
            { 
                printTree.AddRange(ParseExpression(localOrder)); 
            }
            Consume(/*Print: End*/);
            Consume(/*Semicolon*/);
            
            return printTree;
        }

        private List<Node> ParseVariable(int order)
        {
            return new List<Node>{new PrintNode(1)};
        }


        private List<Node> ParseExpression(int localOrder)
        {
            List<Node> expressionTree = new List<Node>();
            Token token = tokens[0];

            if (token.Type == TokenType.Plus)
            {
                expressionTree.Add(new PlusNode(localOrder));
                Consume();
                token = tokens[0];

                if (token.Type == TokenType.String)
                {
                    expressionTree.Add(new StringNode(localOrder, token.Value));
                    Consume();
                }
                else if (token.Type == TokenType.Int)
                {
                    expressionTree.Add(new PlusNode(localOrder));
                    Consume();
                }
                else 
                {
                    throw new Exception("Unknown Term!");
                }   
            }
            else if (token.Type == TokenType.Minus)
            {
                expressionTree.Add(new MinusNode(localOrder));
                Consume();
                token = tokens[0];

                if (token.Type == TokenType.String)
                {
                    expressionTree.Add(new StringNode(localOrder, token.Value));
                    Consume();
                }
                else if (token.Type == TokenType.Int)
                {
                    expressionTree.Add(new IntNode(localOrder, int.Parse(token.Value)));
                    Consume();
                }
                else 
                {
                    throw new Exception("Unknown Term!");
                }   
            }
            else 
            {
                throw new Exception("Unknown Term Operator!");
            }

            return expressionTree;
        }

        private List<Node> ContinueParsingExpression(int localOrder)
        {
            List<Node> result = new List<Node>();
            Console.WriteLine("Hello " + localOrder);
            Consume();
            return result;
        }

        private void Consume()
        {
            if (tokens.Count > 0) 
            {
                tokens.RemoveAt(0);
            }
            else 
            {
                throw new Exception("No more tokens indexed");
            }
        }


        // Checks if tokenToMatch is of the requested TokenType and Value
        private bool Match(Token tokenToMatch, TokenType type, string? value = null)
        {
            return tokenToMatch.Type == type && (value == null || value == tokenToMatch.Value);
        }
    }
}
