using InfinityPart.Domain.Entidades;
using InfinityPart.Entidades;
using System;
using System.Collections.Generic;

namespace InfinityPart.Domain.Interfaces
{
    // Interface responsável por definir as regras do histórico/logs da InfinityPart
    public interface IAuditoriaRepository
    {
        // 1. Salva um novo registro no sistema (ex: "Cliente comprou uma Placa de Vídeo")
        void RegistrarLog(Auditoria auditoria);

        // 2. Traz os últimos X registros salvos
        List<Auditoria> ObterUltimosLogs(int quantidade);

        // 3. Busca todo o histórico de ações de um usuário específico (pelo ID do usuário)
        List<Auditoria> ObterPorUsuario(string usuarioId);

        // 4. Busca registros entre duas datas (útil para relatórios mensais ou diários)
        List<Auditoria> ObterPorData(DateTime dataInicio, DateTime dataFim);

        // 5. Busca apenas logs que registraram erros/falhas no sistema
        List<Auditoria> ObterApenasErros();
    }
}