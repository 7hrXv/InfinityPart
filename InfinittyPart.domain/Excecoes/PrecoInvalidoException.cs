using System;

namespace InfinityPart.Domain.Exceptions;

public class PrecoInvalidoException : RegraNegocioException
{
    public PrecoInvalidoException(string mensagem)
        : base(mensagem)
    {
    }
}