using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Compiler
{

    public abstract class Node { }

    public class StringNode : Node
    {
        public string Value { get; }

        public StringNode(string value)
        {
            Value = value;
        }
    }

    public class PrintNode : Node
    {
        public Node Value { get; }

        public PrintNode(Node value)
        {
            Value = value;
        }
    }

    public class VariabelNode : Node
    {
        public string VariableName { get; }
        public Node Value { get; }

        public VariabelNode(String name, Node value)
        {
            VariableName = name;
            Value = value;
        }
    }


    public class Parser
    {
        private int currentTokenIndex;

        public List<(int, Node)> ast = new List<(int, Node)>();
  


        public List<(int, Node)> Parse(List<Token> tokens)
        {
            currentTokenIndex = 0;

            while (currentTokenIndex <= tokens.Count)
            {
                var (indentation, nodeType) = MatchStatement(tokens, currentTokenIndex);
                ast.Add((indentation, nodeType));
            }
            

            return ast;
        }

        private (int, Node) MatchStatement(List<Token> tokens, int currentTokenIndex)
        {
            if (Match(TokenType.Print, "Start"))
            {
                return (1, ParsePrintStatement());
            }
            else if (Match(TokenType.Variable))
            {
                return (1, ParseVariableStatement(tokens, currentTokenIndex));
            }

            throw new Exception("Unexpected token");
        }


        private Node ParseStatement()
        {
            if (Match(TokenType.Print, "Start"))
            {
                return ParsePrintStatement();
            }

            throw new Exception("Unexpected token");
        }

        private Node ParsePrintStatement()
        {
            Consume(TokenType.Print, "Start");
       

            var value = ParseExpression();

            if (Match(TokenType.Print, "End"))
            {
                Consume(TokenType.Print, "End");
            }
            else 
            {
                throw new Exception("Unexpected token");
            }

            if (Match(TokenType.Semicolon))
            {
                 Consume(TokenType.Semicolon);
            }
            else 
            {
                throw new Exception("Unexpected token");
            }

            return new PrintNode(value);
        }

        private Node ParseVariableStatement(List<Token> tokens, int currentTokenIndex)
        {
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
        }

        private Node ParseExpression()
        {
            if (Match(TokenType.String))
            {
                return new StringNode(Consume(TokenType.String).Value);
            }
            else if (Match(TokenType.Variable))
            {
                return new VariabelNode(Consume(TokenType.Variable).Value);
            }

            throw new Exception("Unexpected token in expression");
        }

        private Token Consume(TokenType type, string? value = null)
        {
            if (Match(type, value))
            {
                return tokens[currentTokenIndex++];
            }

            throw new Exception($"Expected token {type} with value {value}, but found {tokens[currentTokenIndex].Type} with value {tokens[currentTokenIndex].Value}");
        }

        private bool Match(TokenType type, string? value = null)
        {
            return currentTokenIndex < tokens.Count && tokens[currentTokenIndex].Type == type && (value == null || value == tokens[currentTokenIndex].Value);
        }
    }
}