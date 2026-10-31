#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastObserversEntries
{
    internal void AssertPairedHandRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPairedHandRecast(f);
    internal bool HasPairedHandRecastDamageObserver(long windowId) => _resolve().HasPairedHandRecastDamageObserver(windowId);
    internal bool IsPairedHandRecastChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsPairedHandRecastChoice(f, decision);
    internal bool IsPairedHandRecastDying() => _resolve().IsPairedHandRecastDying();
    internal bool PairedHandRecastFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().PairedHandRecastFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? PairedHandRecastObserverRoot() => _resolve().PairedHandRecastObserverRoot();
    internal bool TryAdvancePairedHandRecastSubtree() => _resolve().TryAdvancePairedHandRecastSubtree();
    internal bool ValidPairedHandRecastReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPairedHandRecastReceipt(f);
}
