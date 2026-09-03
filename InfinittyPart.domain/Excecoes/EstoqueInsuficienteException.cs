using System;

namespace InfinityPart.Domain.Exceptions;

public class EstoqueInsuficienteException : RegraNegocioException
{
    public EstoqueInsuficienteException(string mensagem)
        : base(mensagem)
    {
    }
}