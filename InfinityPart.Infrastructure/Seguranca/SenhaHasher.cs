using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace InfinityPart.Infrastructure.Seguranca;

/// <summary>
/// Implementação de ISenhaHasher usando o PasswordHasher do ASP.NET Core Identity
/// (PBKDF2 / HMAC-SHA512, 100.000 iterações, salt aleatório por senha).
/// O hash resultante é uma string Base64 que já carrega salt e parâmetros,
/// portanto nenhuma senha é armazenada em texto puro.
/// </summary>
public class SenhaHasher : ISenhaHasher
{
    private readonly PasswordHasher<Administrador> _hasher = new();

    public string GerarHash(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("A senha não pode ser vazia.", nameof(senha));

        return _hasher.HashPassword(new Administrador(), senha);
    }

    public ResultadoVerificacaoSenha Verificar(string senhaHash, string senhaInformada)
    {
        if (string.IsNullOrWhiteSpace(senhaHash) || string.IsNullOrEmpty(senhaInformada))
            return ResultadoVerificacaoSenha.Invalida;

        var resultado = _hasher.VerifyHashedPassword(new Administrador(), senhaHash, senhaInformada);

        return resultado switch
        {
            PasswordVerificationResult.Success => ResultadoVerificacaoSenha.Valida,
            PasswordVerificationResult.SuccessRehashNeeded => ResultadoVerificacaoSenha.ValidaRequerNovoHash,
            _ => ResultadoVerificacaoSenha.Invalida
        };
    }
}
