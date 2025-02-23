$$
\begin{align}
    [Program] &\to [\text{statement}]^* \\
    [Statement] &\to 

    \begin{cases}
        identifier = [\text{expression}]; \\
        print([\text{expression}]); \\
    \end{cases}\\

    [Expression] &\to
    \begin{cases}
        term\ [(\text{``}+" | \text{``}-")\ term]^*\\
    \end{cases}\\

    [Term] &\to
    \begin{cases}
        \text{factor} \ [(\text{``}*" \ | \ \text{``}/" \ | \ \text{``}\%") \ \text{factor}]^*
    \end{cases}


\end{align}
$$