section .data
    msg db "Hello, World!", 0xA     ; Define the string with a newline character at the end
    msg_len equ $ - msg             ; Calculate the length of the string
    msga db "test", 0xA
    msga_len equ $ - msga

section .text
    global _start

_start:
    ; System call to write to stdout (print)
    mov rax, 1                      ; Syscall number for sys_write in x86-64 (Linux)
    mov rdi, 1                      ; File descriptor 1 (stdout)
    mov rsi, msg                    ; Address of the string to print
    mov rdx, msg_len                ; Length of the string
    syscall
    mov rax, 1                      ; Syscall number for sys_write in x86-64 (Linux)
    mov rdi, 1                      ; File descriptor 1 (stdout)
    mov rsi, msga                    ; Address of the string to print
    mov rdx, msga_len                ; Length of the string
    syscall                         ; Call the kernel

    ; Exit the program
    mov rax, 60                     ; Syscall number for sys_exit in x86-64 (Linux)
    xor rdi, rdi                    ; Exit code 0
    syscall                         ; Call the kernel
