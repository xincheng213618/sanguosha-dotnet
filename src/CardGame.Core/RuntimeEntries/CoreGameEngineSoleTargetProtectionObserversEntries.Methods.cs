#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionObserversEntries
{
    internal bool HasSoleTargetProtectionCardObserver(long useId) => _resolve().HasSoleTargetProtectionCardObserver(useId);
    internal bool IsSoleTargetProtectionDying() => _resolve().IsSoleTargetProtectionDying();
    internal bool IsSoleTargetQuarantinedAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsSoleTargetQuarantinedAttackConsistent(attack, processing);
    internal bool SoleTargetProtectionFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SoleTargetProtectionFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? SoleTargetProtectionObserverRoot() => _resolve().SoleTargetProtectionObserverRoot();
    internal bool TryAdvanceSoleTargetProtectionSubtree() => _resolve().TryAdvanceSoleTargetProtectionSubtree();
}
