#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseLifecycleEntries
{
    internal void IssueCardDamageAdjustment(long useId, int amount, int? targetSeat = null, IReadOnlyList<CardKind>? cardKinds = null, SkillProgramDamageLayer damageLayer = SkillProgramDamageLayer.AdditionalDamage) => _resolve().IssueCardDamageAdjustment(useId, amount, targetSeat, cardKinds, damageLayer);
    internal int CardDamageAdjustmentBonus(GameEngine.IDamageAttempt attack, bool cardDamageOnly = false) => _resolve().CardDamageAdjustmentBonus(attack, cardDamageOnly);
    internal void BeginSimpleCardUse(long frameId, ProgramSimpleCardContinuation continuation) => _resolve().BeginSimpleCardUse(frameId, continuation);
    internal void ContinueCommittedTrickUse(ProgramCardTriggerWindowFrame frame) => _resolve().ContinueCommittedTrickUse(frame);
    internal void ContinueSimpleCardUse(long frameId, ProgramSimpleCardContinuation continuation) => _resolve().ContinueSimpleCardUse(frameId, continuation);
    internal CardUseFrame? LifecycleCardUse(long frameId) => _resolve().LifecycleCardUse(frameId);
    internal bool TryBeginCommittedCardUse(long frameId, ProgramCardContinuation continuation, ProgramTrickContinuation? trick = default, ProgramSimpleCardContinuation? simple = default) => _resolve().TryBeginCommittedCardUse(frameId, continuation, trick, simple);
    internal bool TryMarkFinalizedSimpleProgramsStarted(long frameId) => _resolve().TryMarkFinalizedSimpleProgramsStarted(frameId);
    internal bool TryMarkFinalizedTrickProgramsStarted(long frameId) => _resolve().TryMarkFinalizedTrickProgramsStarted(frameId);
    internal bool TryMarkProgramUseAccepted(long frameId) => _resolve().TryMarkProgramUseAccepted(frameId);
    internal bool TryMarkProgramUseCommitted(long frameId) => _resolve().TryMarkProgramUseCommitted(frameId);
    internal bool TryMarkTargetsAdjusted(long frameId) => _resolve().TryMarkTargetsAdjusted(frameId);
    internal bool TryMarkUnlimitedUse(long frameId) => _resolve().TryMarkUnlimitedUse(frameId);
    internal bool TryMarkYingboRepeated(long frameId) => _resolve().TryMarkYingboRepeated(frameId);
    internal bool TryMarkYingboUnrespondable(long frameId) => _resolve().TryMarkYingboUnrespondable(frameId);
    internal void UpdateLifecycleCardUse(long frameId, Func<CardUseFrame, CardUseFrame> update) => _resolve().UpdateLifecycleCardUse(frameId, update);
}
