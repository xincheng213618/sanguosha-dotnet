#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandDamageRidesEntries
{
    internal ProgramSkillFrame? GetPaidHandRepaymentRoot(long damageWindowId) => _resolve().GetPaidHandRepaymentRoot(damageWindowId);
    internal bool IsPaidHandRepaymentProgramDying() => _resolve().IsPaidHandRepaymentProgramDying();
    internal bool IsPaidHandRepaymentRescueRide(int dyingIndex, DyingFrame dying) => _resolve().IsPaidHandRepaymentRescueRide(dyingIndex, dying);
    internal bool ParticipantHandRescueObserverRide(ResolutionFrame frame, ResolutionFrame parent, CardUseFrame rescue) => _resolve().ParticipantHandRescueObserverRide(frame, parent, rescue);
}
