using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityPart.Application.DTOs.Auth;

namespace InfinityPart.Application.Interfaces;

public interface IAuthService
{
    Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto);

    Task<UsuarioDto?> LoginAsync(LoginDto dto);
}