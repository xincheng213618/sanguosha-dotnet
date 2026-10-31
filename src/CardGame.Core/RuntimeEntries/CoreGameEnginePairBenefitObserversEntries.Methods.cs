#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairBenefitObserversEntries
{
    internal void CaptureFixedRecipientDraw(long frameId, int targetSeat, global::CardGame.Core.CardMoveReason reason, long sequenceBefore, int actualCount) => _resolve().CaptureFixedRecipientDraw(frameId, targetSeat, reason, sequenceBefore, actualCount);
    internal void CaptureFixedRecipientRecovery(long frameId, int owner, int target, int amount) => _resolve().CaptureFixedRecipientRecovery(frameId, owner, target, amount);
    internal bool HasPairBenefitDamageObserver(long damageWindowId) => _resolve().HasPairBenefitDamageObserver(damageWindowId);
    internal bool IsPairBenefitProgramDying() => _resolve().IsPairBenefitProgramDying();
    internal bool PairBenefitFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().PairBenefitFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? PairBenefitObserverRoot() => _resolve().PairBenefitObserverRoot();
}
