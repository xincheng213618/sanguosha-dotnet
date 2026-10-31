#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryReplacementDamageRidesEntries
{
    internal bool IsPaidCardUseProgramDying() => _resolve().IsPaidCardUseProgramDying();
    internal bool IsRecoveryReplacementProgramDying(long? requiredPaidUseId = default) => _resolve().IsRecoveryReplacementProgramDying(requiredPaidUseId);
    internal bool RecoveryReplacementDamageObserverRidesOn(int index) => _resolve().RecoveryReplacementDamageObserverRidesOn(index);
    internal bool RecoveryReplacementFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().RecoveryReplacementFrameRidesOn(ride, beneath);
}
