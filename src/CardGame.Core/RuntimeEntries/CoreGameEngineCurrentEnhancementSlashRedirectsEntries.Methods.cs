#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentEnhancementSlashRedirectsEntries
{
    internal bool HasCurrentEnhancementVirtualProducer(global::CardGame.Core.CardUseFrame use) => _resolve().HasCurrentEnhancementVirtualProducer(use);
    internal bool IsCurrentEnhancementSlashRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CurrentEnhancementSlashRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().IsCurrentEnhancementSlashRedirectFact(use, fact, before, history);
    internal void RecordCurrentEnhancementSlashRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int previousSeat, int newSeat) => _resolve().RecordCurrentEnhancementSlashRedirect(redirector, useId, previousSeat, newSeat);
}
