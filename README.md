# Gymnasiearbete Compiler
 Emanuel & Hampus Gymnasiearbete.



SETUP INSTRUCTIONS FOR REFERENCE:

Open Control Panel  >  Programs  > Turn Windows features on or off  >
    Enable “Virtual Machine Platform”,
    Enable “Windows Hypervisor Platform”,
    Enable “Windows Subsystem for Linux”,
    Restart if prompted.

Open PowerShell as Admin
    Install Ubuntu by running: wsl --install -d Ubuntu-22.04  

Initialize Ubuntu & install NASM.
    Pick a username.
    Make a password, confirm.
    Run:
        sudo apt update
        sudo apt upgrade 
        sudo apt install nasm

Install VS Code.

Install .NET SDK
    sudo apt update
    sudo apt install -y wget apt-transport-https software-properties-common
    wget https://packages.microsoft.com/keys/microsoft.asc
    sudo apt-key add microsoft.asc
    sudo add-apt-repository "$(wget -qO- https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list)"
    sudo apt update
    sudo apt install -y dotnet-sdk-8.0

[Optional] Customize Shell by running: code ~/.bashrc
    Add “PS1='\[\033[0;31m\]\W\[\033[0m\] \[\033[0;31m\]\$\[\033[0m\] '” to the end of the .bashrc file. 
    Run source ~/.bashrc to apply the changes to your current terminal session. 

Download GitHub Desktop and set up the repository or create a new C# Project with: dotnet new console -o Name

Use “dotnet build Compiler.csproj && dotnet run” to build and run the project.

Use “nasm -f elf64 example.asm -o example.o && ld example.o -o example && ./example && rm example.o && rm example” to run example.asm.

Use “echo $?” to catch the exit code.