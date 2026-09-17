using InfinityPart.Application.DTOs.Administradores;
using InfinityPart.Application.DTOs.Autenticacao;
using InfinityPart.Application.Exceptions;
using InfinityPart.Application.Interfaces;
using InfinittyPart.Domain.Interfaces;

namespace InfinityPart.Application.Services;

public class AutenticacaoService : IAutenticacaoService
{
    /// <summary>Tamanho mínimo aceito para a senha.</summary>
    public const int TamanhoMinimoSenha = 8;

    private const string MensagemCredenciaisInvalidas =
        "Usuário ou senha inválidos.";

    private readonly IAdministradorRepository _administradorRepository;
    private readonly ISenhaHasher _senhaHasher;

    public AutenticacaoService(
        IAdministradorRepository administradorRepository,
        ISenhaHasher senhaHasher)
    {
        _administradorRepository = administradorRepository;
        _senhaHasher = senhaHasher;
    }

    public LoginResultadoDto Autenticar(LoginDto dto)
    {
        if (dto == null)
            return LoginResultadoDto.Falha(MensagemCredenciaisInvalidas);

        var identificador = dto.Identificador?.Trim() ?? string.Empty;

        if (identificador.Length == 0 || string.IsNullOrEmpty(dto.Senha))
            return LoginResultadoDto.Falha("Informe usuário e senha.");

        var administrador = _administradorRepository.ObterPorLogin(identificador);

        // Mensagem genérica para não revelar se o usuário existe.
        if (administrador == null)
            return LoginResultadoDto.Falha(MensagemCredenciaisInvalidas);

        if (!administrador.Ativo)
            return LoginResultadoDto.Falha("Este usuário está inativo. Procure o responsável pelo sistema.");

        if (!administrador.PossuiSenhaDefinida)
        {
            return new LoginResultadoDto
            {
                Autenticado = false,
                SenhaNaoDefinida = true,
                Mensagem = "Este usuário ainda não possui senha definida. Defina a primeira senha antes de entrar."
            };
        }

        var verificacao = _senhaHasher.Verificar(administrador.SenhaHash, dto.Senha);

        if (verificacao == ResultadoVerificacaoSenha.Invalida)
            return LoginResultadoDto.Falha(MensagemCredenciaisInvalidas);

        // Atualiza o hash caso tenha sido gerado com parâmetros antigos.
        if (verificacao == ResultadoVerificacaoSenha.ValidaRequerNovoHash)
            administrador.SenhaHash = _senhaHasher.GerarHash(dto.Senha);

        administrador.RegistrarAcesso();
        _administradorRepository.Atualizar(administrador);

        return LoginResultadoDto.Sucesso(new AdministradorDto
        {
            Id = administrador.Id,
            Nome = administrador.Nome,
            Email = administrador.Email,
            Cpf = administrador.Cpf,
            Telefone = administrador.Telefone,
            DataCadastro = administrador.DataCadastro,
            Ativo = administrador.Ativo,
            UltimoAcesso = administrador.UltimoAcesso
        });
    }

    public void DefinirSenha(DefinirSenhaDto dto)
    {
        if (dto == null)
            throw new ValidacaoException("Dados inválidos.");

        var administrador = _administradorRepository.ObterPorId(dto.AdministradorId)
            ?? throw new RecursoNaoEncontradoException("Administrador não encontrado.");

        ValidarSenha(dto.NovaSenha);

        // Se já existe senha, é obrigatório confirmar a senha atual.
        if (administrador.PossuiSenhaDefinida)
        {
            if (string.IsNullOrEmpty(dto.SenhaAtual))
                throw new ValidacaoException("Informe a senha atual.");

            var verificacao = _senhaHasher.Verificar(administrador.SenhaHash, dto.SenhaAtual);

            if (verificacao == ResultadoVerificacaoSenha.Invalida)
                throw new ValidacaoException("A senha atual está incorreta.");
        }

        administrador.SenhaHash = _senhaHasher.GerarHash(dto.NovaSenha);

        _administradorRepository.Atualizar(administrador);
    }

    /// <summary>
    /// Regra única de validação de senha, reaproveitada no cadastro e na troca.
    /// </summary>
    public static void ValidarSenha(string? senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ValidacaoException("A senha é obrigatória.");

        if (senha.Length < TamanhoMinimoSenha)
            throw new ValidacaoException($"A senha deve ter ao menos {TamanhoMinimoSenha} caracteres.");

        if (!senha.Any(char.IsLetter) || !senha.Any(char.IsDigit))
            throw new ValidacaoException("A senha deve conter letras e números.");
    }
}
