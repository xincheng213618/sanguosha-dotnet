#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileObserversEntries
{
    internal bool RiceFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().RiceFirstChild(root, child);
    internal ProgramSkillFrame? RiceObserverRoot() => _resolve().RiceObserverRoot();
    internal bool TryAdvanceRiceSubtree() => _resolve().TryAdvanceRiceSubtree();
}
