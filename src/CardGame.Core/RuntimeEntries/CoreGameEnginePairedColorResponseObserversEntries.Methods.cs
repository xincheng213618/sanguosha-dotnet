#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponseObserversEntries
{
    internal bool IsPairedColorDispositionChoice(ProgramSkillFrame f, PendingDecision decision) => _resolve().IsPairedColorDispositionChoice(f, decision);
    internal bool IsPairedColorDispositionDying() => _resolve().IsPairedColorDispositionDying();
    internal bool PairedColorDispositionFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().PairedColorDispositionFirstChild(f, child);
    internal ProgramSkillFrame? PairedColorDispositionObserverRoot() => _resolve().PairedColorDispositionObserverRoot();
    internal bool PairedColorDispositionStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().PairedColorDispositionStructuralEdge(parent, child);
    internal CardLocation PairedColorPaidDestination(ProgramSkillFrame f, ProgramPairedColorDispositionReceipt r) => _resolve().PairedColorPaidDestination(f, r);
    internal string PairedColorPaidReason(ProgramPairedColorDispositionReceipt r) => _resolve().PairedColorPaidReason(r);
    internal bool TryAdvancePairedColorDispositionSubtree() => _resolve().TryAdvancePairedColorDispositionSubtree();
    internal bool ValidPairedColorDispositionReceipt(ProgramSkillFrame f) => _resolve().ValidPairedColorDispositionReceipt(f);
}
