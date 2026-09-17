using InfinityPart.Application.DTOs.Auth;
using InfinityPart.Application.Interfaces;
using InfinityPart.Domain.Entidades;
using Microsoft.AspNetCore.Identity;

namespace InfinityPart.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioDto dto)
    {
        var usuarioExistente = await _userManager.FindByEmailAsync(dto.Email);

        if (usuarioExistente != null)
            throw new InvalidOperationException("Já existe um usuário com este e-mail.");

        var usuario = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomeCompleto = dto.NomeCompleto,
            CPF = dto.CPF,
            Telefone = dto.Telefone,
            DataCadastro = DateTime.UtcNow
        };

        var resultado = await _userManager.CreateAsync(
            usuario,
            dto.Senha);

        if (!resultado.Succeeded)
        {
            var erros = string.Join(
                "; ",
                resultado.Errors.Select(e => e.Description));

            throw new InvalidOperationException(erros);
        }

        return MapearParaDto(usuario);
    }

    public async Task<UsuarioDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _userManager.FindByEmailAsync(dto.Email);

        if (usuario == null)
            return null;

        var senhaCorreta = await _userManager.CheckPasswordAsync(
            usuario,
            dto.Senha);

        if (!senhaCorreta)
            return null;

        return MapearParaDto(usuario);
    }

    private static UsuarioDto MapearParaDto(ApplicationUser usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email ?? string.Empty,
            CPF = usuario.CPF,
            Telefone = usuario.Telefone,
            DataCadastro = usuario.DataCadastro
        };
    }
}