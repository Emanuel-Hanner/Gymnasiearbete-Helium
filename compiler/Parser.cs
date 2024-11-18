using System;
using System.IO;

namespace Compiler
{

    /* This might turn into something, maybe Top-Down Parsing - Earley Parsing, (with Nodes)
    public abstract class Node
    {
        
    }

    
    // Node for an integer literal
    public class IntNode : Node
    {
        public int Value { get; }

        public IntNode(int value)
        {
            Value = value;
        }
    }

    // Node for a string literal
    public class StringNode : Node
    {
        public string Value { get; }

        public StringNode(string value)
        {
            Value = value;
        }
    }

    // Print statement with either an IntNode or StringNode
    public class PrintNode : Node
    {
        public Node Value { get; } // This can be either IntNode or StringNode

        public PrintNode(Node value)
        {
            Value = value;
        }
    }
    */
    
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
        public ActionType Action { get; set; }  
        public string? Type { get; set; }   // What variable to define
        public string? Value { get; set; }  // What value to assign
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



            // I only want to parse tokens While() they include Print-statements.
            
            // If Print statements can't be nested:
                // then the first 'Print: Start' will be the only before the first 'Print: End'.

            // If Print statements doesen't effect code outside the statement: 
                // then every executed/parsed Print-statementet can be removed from the list.

            // What about the semicolon following the Print statement? Will it allways be there?
                // ..Yes the next token will always be a semicolon.
                    // Subsecuently, should it be neccesary? 
                    // What is the meaning of linebreaks (in a compiled language)?


            // 0. While Loop.
            // 1. Find 'Print: Start'.
            // 2. Find 'Print: End'.
            // 3. Dynamic programming, buildig a parse tree Top-Down and Left-Right. ...is it even dynamic programming?!??
                // Rooted in definitions (Define) connected with modifications (i.e Add/Multiply for example).

                // A self calling method is neccesary,
                    // Input: 
                        // A: A span of tokens that might include tokens not rooted in definitions.
                        // B: A index (I don't remember what it's called) holding all previous defined tokens and sub-tokens
                    // Output: 
                        // A: Break if defined.
                        // B: A span of tokens that includes one layer less than before..
                    // Process:
                        // Iterate through the loop.
                            // Searching + puttin undefined in the front one layer at a time!??
                                // For example: Print("abc" + string * 3) ---> 
                                // Define string, Define 3, Multiply, add TO "abc".     
                                // 
                                //            Print(  )
                                //               |
                                //               |
                                //              add
                                //             /  \
                                //            /    \
                                //   Define "abc"   \
                                //                   \
                                //                 Multiply
                                //                   /    \
                                //                  /      \
                                //         Define string    \
                                //                           \
                                //                        Define 3
                                //
                                //
                                // Print(2 + 3 * 4); ---> Add(Mult(int(3), int(4)), int(2))
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //
                                //


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


                // Loop through Print(interval); 
                for (int i = startIndex+1; i < endIndex-startIndex; i++)
                {
                    // if is pure --> define
                    // else --> find reference (loop with interval (backtracking))
                    // then move on 
                    // spilt +&- and *&/ alternatively?
                
                    /*
                    if (tokens[i].Type == TokenType.Int)
                    {
                        System.Console.WriteLine("yes " + tokens[i].Value);
                    }   
                    else {
                        System.Console.WriteLine("no " + tokens[i].Value);
                    }
                    */
                }


                














                
                
                // tokens.RemoveRange(startIndex, endIndex-startIndex+2);
             
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

                Console.WriteLine(tokenOutput);
                
                break;
            }
        }
    }
}