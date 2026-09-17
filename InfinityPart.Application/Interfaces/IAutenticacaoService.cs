using InfinityPart.Application.DTOs.Autenticacao;

namespace InfinityPart.Application.Interfaces;

public interface IAutenticacaoService
{
    LoginResultadoDto Autenticar(LoginDto dto);

    void DefinirSenha(DefinirSenhaDto dto);
}
