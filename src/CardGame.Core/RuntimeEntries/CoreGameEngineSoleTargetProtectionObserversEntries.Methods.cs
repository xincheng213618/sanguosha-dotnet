#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversEntries
{
    internal bool SoleTargetProtectionFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SoleTargetProtectionFirstChild(f, child);
    internal ProgramSkillFrame? SoleTargetProtectionObserverRoot() => _resolve().SoleTargetProtectionObserverRoot();
    internal bool TryAdvanceSoleTargetProtectionSubtree() => _resolve().TryAdvanceSoleTargetProtectionSubtree();
}
