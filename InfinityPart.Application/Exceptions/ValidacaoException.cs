using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.Exceptions;

public class ValidacaoException : ApplicationException
{
    public ValidacaoException(string mensagem)
        : base(mensagem)
    {
    }
}