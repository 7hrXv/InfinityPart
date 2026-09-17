namespace InfinittyPart.Domain.Interfaces;

public enum ResultadoVerificacaoSenha
{
    Invalida = 0,
    Valida = 1,

    /// <summary>
    /// Senha correta, porém gerada com parâmetros antigos de hash.
    /// O hash deve ser regravado com os parâmetros atuais.
    /// </summary>
    ValidaRequerNovoHash = 2
}

/// <summary>
/// Abstração de hash de senha. A implementação concreta fica na Infrastructure
/// para que Domain e Application não dependam de detalhes de criptografia.
/// </summary>
public interface ISenhaHasher
{
    string GerarHash(string senha);

    ResultadoVerificacaoSenha Verificar(string senhaHash, string senhaInformada);
}
