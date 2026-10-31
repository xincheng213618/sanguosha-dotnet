#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryReplacementDamageRidesEntries
{
    internal bool IsPaidCardUseProgramDying() => _resolve().IsPaidCardUseProgramDying();
    internal bool IsRecoveryReplacementProgramDying(long? requiredPaidUseId = default) => _resolve().IsRecoveryReplacementProgramDying(requiredPaidUseId);
    internal bool MatchRecoveryReplacementProgramDying() => _resolve().MatchRecoveryReplacementProgramDying();
    internal bool RecoveryReplacementDamageObserverRidesOn(int index) => _resolve().RecoveryReplacementDamageObserverRidesOn(index);
    internal bool RecoveryReplacementFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().RecoveryReplacementFrameRidesOn(ride, beneath);
}
