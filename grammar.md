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
        term\ [(\text{``}+" | \text{``}-")\ Term]^*\\
    \end{cases}\\

    [Term] &\to
    \begin{cases}
        \text{Factor} \ [(\text{``}*" \ | \ \text{``}/" \ | \ \text{``}\%") \ \text{Factor}]^*
    \end{cases}


\end{align}
$$