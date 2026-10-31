#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversEntries
{
    internal bool SoleTargetProtectionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SoleTargetProtectionFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? SoleTargetProtectionObserverRoot() => _resolve().SoleTargetProtectionObserverRoot();
    internal bool TryAdvanceSoleTargetProtectionSubtree() => _resolve().TryAdvanceSoleTargetProtectionSubtree();
}
