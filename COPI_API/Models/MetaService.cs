using Microsoft.EntityFrameworkCore;

namespace COPI_API.Models
{
    public class MetaService
    {
        private readonly AppDbContext _context;
        public MetaService(AppDbContext context)
        {
            _context = context;
        }

        // Atualiza o progresso das ações estratégicas de uma meta
        public async Task AtualizarProgressoAcoesAsync(int acaoId)
        {
            var acao = await _context.AcoesEstrategicas
                .Include(a => a.Tarefas)
                .FirstOrDefaultAsync(a => a.Id == acaoId);
            if (acao == null)
                return;

            // Calcula progresso da ação estratégica com base nas tarefas batidas
            int total = acao.Tarefas?.Count ?? 0;
            int batidas = acao.Tarefas?.Count(t => t.Batida) ?? 0;
            acao.Progresso = (total > 0) ? (int)Math.Round((batidas * 100.0) / total) : 0;

            // Se progresso 100%, marca como concluída e batida
            if (acao.Progresso == 100)
            {
                acao.Status = "Concluída";
                acao.Batida = true;
                acao.Concluido = true;
            }
            else
            {
                // Remove status de concluída e batida se não for mais 100%
                if (acao.Status == "Concluída")
                    acao.Status = "Em andamento";
                acao.Batida = false;
                acao.Concluido = false;
            }

            _context.AcoesEstrategicas.Update(acao);
            await _context.SaveChangesAsync();

            // Atualiza a meta acima
            await AtualizarProgressoMetaAsync(acao.MetaId);
        }

        // Atualiza o progresso da meta com base nas ações estratégicas
        public async Task AtualizarProgressoMetaAsync(int metaId)
        {
            var meta = await _context.Metas
                .Include(m => m.AcoesEstrategicas)
                    .ThenInclude(a => a.Tarefas)
                .FirstOrDefaultAsync(m => m.Id == metaId);
            if (meta == null)
                return;
            int total = meta.AcoesEstrategicas?.Count ?? 0;
            int batidas = meta.AcoesEstrategicas?.Count(a => a.Progresso == 100 || a.Batida) ?? 0;
            meta.Progresso = (total > 0) ? (int)Math.Round((batidas * 100.0) / total) : 0;

            // Se progresso 100%, marca como concluída
            if (meta.Progresso == 100)
            {
                meta.Status = "Concluída";
            }
            // Se não for mais 100%, remove concluído
            else if (meta.Status == "Concluída")
            {
                meta.Status = "Em andamento";
            }

            _context.Metas.Update(meta);
            await _context.SaveChangesAsync();
        }

        // Atualiza tudo em cascata a partir de uma ação estratégica
        public async Task AtualizarCascataPorAcaoAsync(int acaoId)
        {
            await AtualizarProgressoAcoesAsync(acaoId);
        }

        // Atualiza tudo em cascata a partir de uma tarefa
        public async Task AtualizarCascataPorTarefaAsync(int tarefaId)
        {
            var tarefa = await _context.Tarefas.FindAsync(tarefaId);
            if (tarefa == null) return;
            await AtualizarProgressoAcoesAsync(tarefa.AcaoEstrategicaId);
        }
    }
}
