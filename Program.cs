using System;
using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "helium.he";            
            string fileContent = File.ReadAllText(fileName);

            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i] == '\n')
                {
                    Console.WriteLine(i);
                }                
                Console.WriteLine(fileContent[i]);
            }

            string displayContent = fileContent.Replace("\n", "\\n");

            Console.WriteLine("File Content:");
            Console.WriteLine(fileContent);
            Console.WriteLine(displayContent);


        }
    }
}
