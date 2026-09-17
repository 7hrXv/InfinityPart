using InfinittyPart.Domain.Entidades;

namespace InfinittyPart.Domain.Interfaces;

public interface IAdministradorRepository
{
    void Adicionar(Administrador administrador);

    void Atualizar(Administrador administrador);

    void Remover(int id);

    Administrador? ObterPorId(int id);

    Administrador? ObterPorCpf(string cpf);

    Administrador? ObterPorEmail(string email);

    /// <summary>
    /// Busca o administrador por qualquer identificador de login aceito:
    /// nome de usuário (Nome), e-mail ou CPF (com ou sem máscara).
    /// </summary>
    Administrador? ObterPorLogin(string identificador);

    List<Administrador> ObterTodos();
}
