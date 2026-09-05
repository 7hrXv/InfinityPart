using InfinityPart.Domain.Interfaces;
using InfinityPart.Entidades;

namespace InfinityPart.Infrastructure.Repositories
{
    public class AuditoriaRepository : IAuditoriaRepository
    {
        private readonly InfinityPartDbContext _context;

        public AuditoriaRepository(InfinityPartDbContext context)
        {
            _context = context;
        }

        public void RegistrarLog(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

        public List<Auditoria> ObterUltimosLogs(int quantidade)
        {
            return _context.Auditorias
                .OrderByDescending(a => a.DataHora)
                .Take(quantidade)
                .ToList();
        }

        public List<Auditoria> ObterPorUsuario(string usuarioId)
        {
            return _context.Auditorias
                .Where(a => a.UsuarioId == usuarioId)
                .OrderByDescending(a => a.DataHora)
                .ToList();
        }

        public List<Auditoria> ObterPorData(
            DateTime dataInicio,
            DateTime dataFim)
        {
            return _context.Auditorias
                .Where(a => a.DataHora >= dataInicio &&
                            a.DataHora <= dataFim)
                .OrderByDescending(a => a.DataHora)
                .ToList();
        }

        public List<Auditoria> ObterApenasErros()
        {
            return _context.Auditorias
                .Where(a => a.Acao.Contains("erro") ||
                            a.Acao.Contains("Erro") ||
                            a.Acao.Contains("ERRO"))
                .OrderByDescending(a => a.DataHora)
                .ToList();
        }
    }
}