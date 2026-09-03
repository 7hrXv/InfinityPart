using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.Exceptions;

public class RecursoNaoEncontradoException : ApplicationException
{
    public RecursoNaoEncontradoException(string mensagem)
        : base(mensagem)
    {
    }
}