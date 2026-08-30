using System;

namespace InfinityPart.Domain.Excecoes
{
    public class RegraNegocioException : Exception
    {
        public RegraNegocioException(string mensagem) : base(mensagem)
        {
        }
    }
}