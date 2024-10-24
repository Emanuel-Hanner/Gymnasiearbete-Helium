; // THIS IS FOR OUR EDUCATIONAL PURPOSES ONLY, // ;
; //   I NEED TO LEARN 'ASSEMBLY' SOMEHOW T-T   // ;
 

section .bss
    input resb 128             ; Reserve 128 bytes for the input string
    doubled_input resb 257     ; Reserve 256 bytes for the doubled string + 1 byte for the newline

section .data
    prompt db "Enter a string: ", 0  ; Prompt message
    prompt_len equ $ - prompt

section .text
    global _start

_start:
    ; Step 1: Print the prompt message
    mov rax, 1            ; sys_write
    mov rdi, 1            ; file descriptor (stdout)
    mov rsi, prompt       ; pointer to the prompt message
    mov rdx, prompt_len   ; length of the prompt message
    syscall

    ; Step 2: Read the user's input
    mov rax, 0            ; sys_read
    mov rdi, 0            ; file descriptor (stdin)
    mov rsi, input        ; pointer to the input buffer
    mov rdx, 128          ; maximum number of bytes to read
    syscall

    ; Step 3: Calculate the length of the input
    ; Subtract the newline character (0x0A) and any additional bytes
    mov rcx, rax          ; Move the number of bytes read to rcx
    dec rcx               ; Decrement rcx to account for the newline character

    ; Step 4: Copy input to doubled_input twice
    mov rdi, doubled_input ; Destination: doubled_input
    mov rsi, input         ; Source: input
    mov rdx, rcx           ; Save the original length of input

    ; Copy first time
copy_first_input:
    movsb                 ; Move byte from [rsi] to [rdi]
    loop copy_first_input ; Repeat RCX times

    ; Copy second time
    mov rsi, input        ; Reset RSI to the start of input
    mov rcx, rdx          ; Reset RCX to the original input length
copy_second_input:
    movsb                 ; Move byte from [rsi] to [rdi]
    loop copy_second_input ; Repeat RCX times

    ; Step 5: Append a newline character
    mov byte [rdi], 0x0A  ; Append a newline character to the end of the doubled string

    ; Step 6: Output the doubled string
    mov rax, 1            ; sys_write
    mov rdi, 1            ; file descriptor (stdout)
    mov rsi, doubled_input ; pointer to the doubled string
    add rdx, rdx          ; Double the original input length (since we repeated the input twice)
    inc rdx               ; Account for the newline character
    syscall

    ; Exit the program
    mov rax, 60           ; sys_exit
    xor rdi, rdi          ; exit code 0
    syscall
