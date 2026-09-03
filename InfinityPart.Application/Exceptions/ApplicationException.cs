using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityPart.Application.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string mensagem)
        : base(mensagem)
    {
    }
}