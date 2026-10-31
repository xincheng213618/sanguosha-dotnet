#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedHandRecastObserversEntries
{
    internal bool IsPairedHandRecastDying() => _resolve().IsPairedHandRecastDying();
    internal bool PairedHandRecastFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().PairedHandRecastFirstChild(f, child);
    internal ProgramSkillFrame? PairedHandRecastObserverRoot() => _resolve().PairedHandRecastObserverRoot();
    internal bool TryAdvancePairedHandRecastSubtree() => _resolve().TryAdvancePairedHandRecastSubtree();
    internal bool ValidPairedHandRecastReceipt(ProgramSkillFrame f) => _resolve().ValidPairedHandRecastReceipt(f);
}
