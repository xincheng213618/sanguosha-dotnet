#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlObserversEntries
{
    internal ProgramSkillFrame? PrivateWhirlObserverRoot() => _resolve().PrivateWhirlObserverRoot();
    internal bool TryAdvancePrivateWhirlSubtree() => _resolve().TryAdvancePrivateWhirlSubtree();
    internal bool WhirlFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().WhirlFirstChild(root, child);
}
