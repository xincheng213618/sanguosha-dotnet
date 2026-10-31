#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandDamageRidesEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? GetPaidHandRepaymentRoot(long damageWindowId) => _resolve().GetPaidHandRepaymentRoot(damageWindowId);
    internal bool IsPaidHandRepaymentProgramDying() => _resolve().IsPaidHandRepaymentProgramDying();
    internal bool IsPaidHandRepaymentRescueRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
    internal bool ParticipantHandRescueObserverRide(global::CardGame.Core.ResolutionFrame frame, global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.CardUseFrame rescue) => _resolve().ParticipantHandRescueObserverRide(frame, parent, rescue);
}
