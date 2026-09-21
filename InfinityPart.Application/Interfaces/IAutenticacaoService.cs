using InfinityPart.Application.DTOs.Autenticacao;

namespace InfinityPart.Application.Interfaces;

public interface IAutenticacaoService
{
    LoginResultadoDto Autenticar(LoginDto dto);

    LoginResultadoDto AutenticarCliente(LoginDto dto);

    void DefinirSenha(DefinirSenhaDto dto);
}