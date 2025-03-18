section .data
    msg0 db "Hello, World!", 0xA    ; String 0 with newline
    msg0_len equ $ - msg0            ; Length of msg0

section .text
    global _start

_start:
    ; Print string (msg0)
    mov rsi, msg0                    ; Pointer to the message
    mov rdx, msg0_len                ; Length of the message
    call print_string

    ; Exit program
    mov rax, 60                     ; Syscall: exit
    xor rdi, rdi                    ; Exit code 0
    syscall                         ; Invoke kernel


print_string:
    mov rax, 1                      ; Syscall: write
    mov rdi, 1                      ; File descriptor: stdout
    syscall                         ; Invoke kernel
    ret                             ; Return to caller
