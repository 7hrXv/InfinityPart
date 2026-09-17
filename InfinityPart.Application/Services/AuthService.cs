using InfinityPart.Application.DTOs.Auth;
using InfinityPart.Application.Interfaces;

namespace InfinityPart.Application.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;

    public AuthService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        return await _identityService.RegistrarAsync(dto);
    }

    public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
    {
        return await _identityService.LoginAsync(dto);
    }
}