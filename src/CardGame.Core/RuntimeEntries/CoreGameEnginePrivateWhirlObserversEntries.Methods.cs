#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? PrivateWhirlObserverRoot() => _resolve().PrivateWhirlObserverRoot();
    internal bool TryAdvancePrivateWhirlSubtree() => _resolve().TryAdvancePrivateWhirlSubtree();
    internal bool WhirlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().WhirlFirstChild(root, child);
}
