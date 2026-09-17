using InfinittyPart.Domain.Entidades;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Infrastructure.Repositories;

public class AdministradorRepository : IAdministradorRepository
{
    private readonly InfinityPartDbContext _context;

    public AdministradorRepository(InfinityPartDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Administrador administrador)
    {
        _context.Administradores.Add(administrador);
        _context.SaveChanges();
    }

    public void Atualizar(Administrador administrador)
    {
        _context.Administradores.Update(administrador);
        _context.SaveChanges();
    }

    public void Remover(int id)
    {
        var administrador = _context.Administradores.Find(id);

        if (administrador != null)
        {
            _context.Administradores.Remove(administrador);
            _context.SaveChanges();
        }
    }

    public Administrador? ObterPorId(int id)
    {
        return _context.Administradores.Find(id);
    }

    public Administrador? ObterPorCpf(string cpf)
    {
        var somenteDigitos = SomenteDigitos(cpf);
        var comMascara = AplicarMascaraCpf(somenteDigitos);

        return _context.Administradores
            .FirstOrDefault(a => a.Cpf == cpf
                              || a.Cpf == somenteDigitos
                              || a.Cpf == comMascara);
    }

    public Administrador? ObterPorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var normalizado = email.Trim();

        return _context.Administradores
            .FirstOrDefault(a => a.Email == normalizado);
    }

    public Administrador? ObterPorLogin(string identificador)
    {
        if (string.IsNullOrWhiteSpace(identificador))
            return null;

        var valor = identificador.Trim();
        var digitos = SomenteDigitos(valor);

        // Só considera variações de CPF quando o valor informado tem 11 dígitos.
        var cpfSemMascara = digitos.Length == 11 ? digitos : valor;
        var cpfComMascara = digitos.Length == 11 ? AplicarMascaraCpf(digitos) : valor;

        return _context.Administradores
            .FirstOrDefault(a => a.Email == valor
                              || a.Nome == valor
                              || a.Cpf == valor
                              || a.Cpf == cpfSemMascara
                              || a.Cpf == cpfComMascara);
    }

    public List<Administrador> ObterTodos()
    {
        return _context.Administradores.ToList();
    }

    private static string SomenteDigitos(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        return new string(valor.Where(char.IsDigit).ToArray());
    }

    private static string AplicarMascaraCpf(string somenteDigitos)
    {
        if (somenteDigitos.Length != 11)
            return string.Empty;

        return $"{somenteDigitos[..3]}.{somenteDigitos[3..6]}.{somenteDigitos[6..9]}-{somenteDigitos[9..]}";
    }
}
