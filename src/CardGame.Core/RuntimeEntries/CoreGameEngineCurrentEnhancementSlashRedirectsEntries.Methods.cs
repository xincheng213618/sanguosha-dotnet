#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsEntries
{
    internal bool HasCurrentEnhancementVirtualProducer(CardUseFrame use) => _resolve().HasCurrentEnhancementVirtualProducer(use);
    internal bool IsCurrentEnhancementSlashRedirectFact(CardUseFrame use, CurrentEnhancementSlashRedirectedEvent fact, IReadOnlyList<int> before, IReadOnlyList<IGameEvent> history) => _resolve().IsCurrentEnhancementSlashRedirectFact(use, fact, before, history);
    internal void RecordCurrentEnhancementSlashRedirect(ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => _resolve().RecordCurrentEnhancementSlashRedirect(redirector, useId, previousSeat, newSeat);
}
