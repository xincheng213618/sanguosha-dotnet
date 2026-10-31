#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponseObserversEntries
{
    internal void AssertPairedColorDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPairedColorDisposition(f);
    internal bool HasPairedColorDispositionBeforeDamageObserver(long id) => _resolve().HasPairedColorDispositionBeforeDamageObserver(id);
    internal bool HasPairedColorDispositionCardObserver(long parentId) => _resolve().HasPairedColorDispositionCardObserver(parentId);
    internal bool HasPairedColorDispositionDamageObserver(long id) => _resolve().HasPairedColorDispositionDamageObserver(id);
    internal bool HasPairedColorDispositionObserver(long id, bool before) => _resolve().HasPairedColorDispositionObserver(id, before);
    internal bool IsPairedColorDispositionChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsPairedColorDispositionChoice(f, decision);
    internal bool IsPairedColorDispositionDying() => _resolve().IsPairedColorDispositionDying();
    internal bool PairedColorDispositionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().PairedColorDispositionFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? PairedColorDispositionObserverRoot() => _resolve().PairedColorDispositionObserverRoot();
    internal bool PairedColorDispositionStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().PairedColorDispositionStructuralEdge(parent, child);
    internal global::CardGame.Core.CardLocation PairedColorPaidDestination(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramPairedColorDispositionReceipt r) => _resolve().PairedColorPaidDestination(f, r);
    internal string PairedColorPaidReason(global::CardGame.Core.ProgramPairedColorDispositionReceipt r) => _resolve().PairedColorPaidReason(r);
    internal bool TryAdvancePairedColorDispositionSubtree() => _resolve().TryAdvancePairedColorDispositionSubtree();
    internal bool ValidPairedColorDispositionReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPairedColorDispositionReceipt(f);
}
