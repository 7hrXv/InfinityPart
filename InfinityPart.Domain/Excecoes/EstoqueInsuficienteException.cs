using System;

namespace Infinitypart.Domain.Excecoes
{
    public class EstoqueInsuficienteException : Exception
    {
        public EstoqueInsuficienteException(string mensagem) : base(mensagem)
        {
        }
    }
}