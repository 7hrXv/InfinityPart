using System;

namespace Infinitypart.Domain.Excecoes
{
    public class PrecoInvalidoException : Exception
    {
        public PrecoInvalidoException(string mensagem) : base(mensagem)
        {
        }
    }
}