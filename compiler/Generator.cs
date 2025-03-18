using System.ComponentModel;
using Compiler;

public class Generator()
{
    string sectionData = "section .data\n";
    string sectionText = "\n\nsection .text\n    global _start";
    string sectionStart  = "\n\n_start:\n";

    string sectionEnd = "\n\n    ; Exit program\n    mov rax, 60                     ; Syscall: exit\n    xor rdi, rdi                    ; Exit code 0\n    syscall                         ; Invoke kernel\n";

    string sectionMethods = "\n\n";
    
    public string Generate(List<Node> ast)
    {
        int index = 0;

        if (ast[0] is PrintNode)
        {
            if (ast[1] is StringNode)
            {
                StringNode stringNode = (StringNode)ast[1];
                
                sectionData += $"    msg{index} db \"{stringNode.Value}\", 0xA    ; String {index} with newline\n    msg{index}_len equ $ - msg{0}            ; Length of msg{0}";

                sectionStart += $"    ; Print string (msg{index})\n    mov rsi, msg{index}                    ; Pointer to the message\n    mov rdx, msg{index}_len                ; Length of the message\n    call print_string";

                sectionMethods += "print_string:\n    mov rax, 1                      ; Syscall: write\n    mov rdi, 1                      ; File descriptor: stdout\n    syscall                         ; Invoke kernel\n    ret                             ; Return to caller\n";
    
            }
            else if (ast[1] is IntNode)
            {
                System.Console.WriteLine("no");
            }
            else if (ast[1] is VariableNode)
            {
                System.Console.WriteLine("maybe");
            }
        }
        else if (ast[0] is VariableNode)
        {
            System.Console.WriteLine("no");
        }
        else 
        {
            throw new Exception("Unknown Node-type!");
        }
        
        
        
        


        return sectionData + sectionText + sectionStart + sectionEnd + sectionMethods;
    }

    
    
   
}



