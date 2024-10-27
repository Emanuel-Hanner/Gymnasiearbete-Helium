$$
\begin{align}
[Program] &\to [\text{statement}]^* \\
[statement] &\to 
\begin{cases}
identifier = [\text{expression}]; \\
Print([\text{expression}]); \\

\end{cases}\\

[expression] &\to
\begin{cases}
identifier? \\
integer? \\
string? \\

\end{cases}

\end{align}
$$