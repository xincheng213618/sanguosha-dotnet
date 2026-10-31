#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRicePileObserversEntries
{
    internal bool RiceFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().RiceFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? RiceObserverRoot() => _resolve().RiceObserverRoot();
    internal bool TryAdvanceRiceSubtree() => _resolve().TryAdvanceRiceSubtree();
}
