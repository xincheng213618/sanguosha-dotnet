#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNianActivityProgramsEntries
{
    internal bool CanRunNianJingjueCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanRunNianJingjueCandidate(candidate, context);
    internal CardMovementBatchContext CaptureNianOwnedTransferBatch(CardMovementBatchContext batch, ProgramSkillFrame? origin) => _resolve().CaptureNianOwnedTransferBatch(batch, origin);
    internal IEnumerable<ProgramTriggerCandidate> CollectNianJingjueCandidates(CardMovementBatchContext batch) => _resolve().CollectNianJingjueCandidates(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectNianRecoveryCandidates(HpChangeContext change) => _resolve().CollectNianRecoveryCandidates(change);
    internal void CollectWindow_CollectNianJingjueCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectNianJingjueCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureNianOwnedTransferBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNianOwnedTransferBatch(batch, completed);
    internal ProgramSkillWindowContext CreateNianJingjueContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => _resolve().CreateNianJingjueContext(parent, candidate);
    internal ProgramSkillWindowContext CreateNianRecoveryContext(HpChangeContext change, ProgramTriggerCandidate candidate) => _resolve().CreateNianRecoveryContext(change, candidate);
    internal bool DistanceCardUseExemption60NianShouyiUnlimitedCardDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption60NianShouyiUnlimitedCardDistance(q);
    internal bool ExcludeOrdinaryMovement_IsNianJingjueTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsNianJingjueTrigger(candidate, trigger);
    internal SkillProgramStepOutcome ExecuteNianActivity(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteNianActivity(supplied, effect);
    internal bool IsNianRecoveryCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsNianRecoveryCandidate(candidate, context);
    internal int[] MatchingNianJingjueMovements(CardMovementBatchContext batch, int owner) => _resolve().MatchingNianJingjueMovements(batch, owner);
    internal int NianBaonuDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().NianBaonuDamageBonus(attack);
    internal bool NianDongmianDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().NianDongmianDirectedTargetProhibited(query);
    internal bool NianDongmianForbidsTarget(int actor, int target) => _resolve().NianDongmianForbidsTarget(actor, target);
    internal bool NianEnabled(CharacterState player, string id) => _resolve().NianEnabled(player, id);
    internal bool NianOwnActualTurn(int seat) => _resolve().NianOwnActualTurn(seat);
    internal bool NianPhaseWasActuallySkipped(int seat, SkillProgramTurnPhase phase) => _resolve().NianPhaseWasActuallySkipped(seat, phase);
    internal bool NianShouyiUnlimitedCardDistance(CharacterState player) => _resolve().NianShouyiUnlimitedCardDistance(player);
    internal bool? ProveNianJingjueMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveNianJingjueMovement(trigger, candidate, context);
    internal bool? ProveNianRecovery(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveNianRecovery(trigger, candidate, context);
    internal void RecordNianActualPhaseSkips(int owner, bool draw, bool play) => _resolve().RecordNianActualPhaseSkips(owner, draw, play);
    internal void ReturnNianOperationDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnNianOperationDraw(frame, outcome);
    internal ProgramSkillWindowContext? TryCreateNianJingjueContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateNianJingjueContext(frame, candidate);
}
