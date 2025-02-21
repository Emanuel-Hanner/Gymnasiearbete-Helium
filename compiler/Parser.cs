using System;
using System.Collections.Generic;
using System.IO;

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
        public Node Value { get; }

        public VariabelNode(Node value)
        {
            Value = value;
        }
    }


    public class Parser
    {
        private List<Token> tokens;
        private int currentTokenIndex;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentTokenIndex = 0;
        }

        public Node Parse()
        {
            return ParseStatement();
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

            Consume(TokenType.Print, "End");
            Consume(TokenType.Semicolon);

            return new PrintNode(value);
        }

        private Node ParseExpression()
        {
            if (Match(TokenType.String))
            {
                return new StringNode(Consume(TokenType.String).Value);
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