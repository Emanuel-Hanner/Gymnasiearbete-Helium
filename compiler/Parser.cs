using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Compiler
{

    public abstract class Node { }

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



    public class Parser
    {
        public List<Node> ast = new List<Node>();

        private int currentTokenIndex;
        private int executionOrder;


        // The Parse-function called from Program.cs
        public List<Node> Parse(List<Token> tokens)
        {
            currentTokenIndex = 0; 
            executionOrder = 0;

            int totalTokens = tokens.Count;

            while (currentTokenIndex <= totalTokens) { ast.AddRange(ParseStatement(tokens, currentTokenIndex)); }
        
            return ast;
        }


        // Deconstructs the Program into defined Statements 
        private List<Node> ParseStatement(List<Token> tokens, int position)
        {
            Token currentToken = tokens[position];

            if (MatchToken(currentToken, TokenType.Print, "Start"))
            {
                return [new PrintNode(executionOrder), .. ParsePrint(tokens, position)]; // ".." = the spread operator - a Collection Expression introduced with C#12
            }
            else if (MatchToken(currentToken, TokenType.Variable) && currentToken.Value != null)
            {
                return [new VariabelNode(executionOrder, currentToken.Value), .. ParseVariable(tokens, position)];
            }  
            else
            {
                throw new Exception("Unexpected token: A Statement is required");
            }
        }


        // Parses Print Statments 
        private List<Node> ParsePrint(List<Token> tokens, int position)
        {
            

            /*
            Consume(TokenType.Print, "Start");
       

            var value = ParseExpression();

            if (MatchToken(TokenType.Print, "End"))
            {
                Consume(TokenType.Print, "End");
            }
            else 
            {
                throw new Exception("Unexpected token");
            }

            if (MatchToken(TokenType.Semicolon))
            {
                 Consume(TokenType.Semicolon);
            }
            else 
            {
                throw new Exception("Unexpected token");
            }
            
            */
            currentTokenIndex += 100;
            return new List<Node>{new PrintNode(1)};
        }

        private List<Node> ParseVariable(List<Token> tokens, int position)
        {
            /*
            if (tokens[currentTokenIndex].Value == null)
            {
                throw new Exception("Variable name can not be null");
            }
            else   
            {
                var variable = Consume(TokenType.Variable);
                Consume(TokenType.Equals);
                var value = ParseExpression();
                Consume(TokenType.Semicolon);
            
                return new VariabelNode(tokens[currentTokenIndex].Value, value);
            }
            */
            currentTokenIndex += 100;
            return new List<Node>{new PrintNode(1)};
        }

        private List<Node> ParseExpression()
        {
            /*
            if (MatchToken(TokenType.String))
            {
                return new StringNode(Consume(TokenType.String).Value);
            }
            else if (MatchToken(TokenType.Variable))
            {
                return new VariabelNode(Consume(TokenType.Variable).Value);
            }

            throw new Exception("Unexpected token in expression");
            */

            return new List<Node>();
        }

        private void Consume(TokenType type, string? value = null)
        {
            /*
            if (MatchToken(type, value))
            {
                return tokens[currentTokenIndex++];
            }
           

            throw new Exception($"Expected token {type} with value {value}, but found {tokens[currentTokenIndex].Type} with value {tokens[currentTokenIndex].Value}");
            */
        }


        // Checks if tokenToMatch is of the requested TokenType and Value
        private bool MatchToken(Token tokenToMatch, TokenType type, string? value = null)
        {
            return tokenToMatch.Type == type && (value == null || value == tokenToMatch.Value);
        }
    }
}