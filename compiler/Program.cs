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
            parser.Parse();
        }
        
    }
}
