section .data
    msg db "Hello, World!", 0xA     ; String 1 with newline
    msg_len equ $ - msg             ; Length of msg
    msga db "test", 0xA             ; String 2 with newline
    msga_len equ $ - msga           ; Length of msga

    num1 dq 10                      ; First number (64-bit integer)
    num2 dq 20                      ; Second number (64-bit integer)
    result_str db "Sum: ", 0        ; Prefix for the sum output
    buffer db 0, 0, 0, 0, 0, 0, 0, 0, 0, 0  ; Buffer for storing the result as a string
    newline db 0xA                  ; Newline character

section .text
    global _start

_start:
    ; Print first string (msg)
    mov rsi, msg
    mov rdx, msg_len
    call print_string

    ; Print second string (msga)
    mov rsi, msga
    mov rdx, msga_len
    call print_string

    ; Load num1 and num2 into registers and add them
    mov rax, [num1]                 ; Load num1 into rax
    add rax, [num2]                 ; Add num2 to rax (rax = num1 + num2)

    ; Convert the sum in rax to a string
    mov rsi, buffer                 ; Point to buffer for the string
    call int_to_ascii               ; Convert rax to ASCII string

    ; Print the result string prefix
    mov rsi, result_str             ; "Sum: "
    mov rdx, 6                      ; Length of "Sum: "
    call print_string

    ; Print the converted sum from buffer
    mov rsi, buffer                 ; Address of the converted string
    mov rdx, rax                    ; rax contains the string length
    call print_string

    ; Print newline
    mov rsi, newline
    mov rdx, 1
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

int_to_ascii:
    ; Converts the integer in rax to ASCII in the buffer pointed to by rsi
    mov rcx, 0                      ; Counter for digits
    mov rbx, 10                     ; Divisor for base 10

convert_loop:
    xor rdx, rdx                    ; Clear rdx (to hold remainder)
    div rbx                         ; Divide rax by 10, rdx gets remainder
    add dl, '0'                     ; Convert remainder to ASCII
    dec rsi                         ; Move buffer pointer backwards
    mov [rsi], dl                   ; Store ASCII character
    inc rcx                         ; Increment digit counter
    test rax, rax                   ; Check if quotient is 0
    jnz convert_loop                ; Continue if not 0

    mov rax, rcx                    ; Set rax to the number of digits
    ret                             ; Return to caller
