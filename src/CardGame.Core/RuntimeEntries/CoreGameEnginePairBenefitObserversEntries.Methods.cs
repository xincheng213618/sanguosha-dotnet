#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairBenefitObserversEntries
{
    internal void CaptureFixedRecipientDraw(long frameId, int targetSeat, CardMoveReason reason, long sequenceBefore, int actualCount) => _resolve().CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
    internal void CaptureFixedRecipientRecovery(long frameId, int owner, int target, int amount) => _resolve().CaptureFixedRecipientRecovery(frameId, owner, target, amount);
    internal bool IsPairBenefitProgramDying() => _resolve().IsPairBenefitProgramDying();
    internal bool PairBenefitFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().PairBenefitFirstChild(f, child);
    internal ProgramSkillFrame? PairBenefitObserverRoot() => _resolve().PairBenefitObserverRoot();
}
