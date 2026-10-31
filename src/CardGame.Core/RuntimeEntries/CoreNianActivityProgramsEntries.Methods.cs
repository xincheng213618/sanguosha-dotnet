#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNianActivityProgramsEntries
{
    internal bool CanRunNianJingjueCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunNianJingjueCandidate(candidate, context);
    internal global::CardGame.Core.CardMovementBatchContext CaptureNianOwnedTransferBatch(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramSkillFrame? origin) => _resolve().CaptureNianOwnedTransferBatch(batch, origin);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectNianJingjueCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectNianJingjueCandidates(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectNianRecoveryCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectNianRecoveryCandidates(change);
    internal void CollectWindow_CollectNianJingjueCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectNianJingjueCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureNianOwnedTransferBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureNianOwnedTransferBatch(batch, completed);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateNianJingjueContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateNianJingjueContext(parent, candidate);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateNianRecoveryContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateNianRecoveryContext(change, candidate);
    internal bool DistanceCardUseExemption60NianShouyiUnlimitedCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption60NianShouyiUnlimitedCardDistance(q);
    internal bool ExcludeOrdinaryMovement_IsNianJingjueTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsNianJingjueTrigger(candidate, trigger);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteNianActivity(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteNianActivity(supplied, effect);
    internal bool IsNianRecoveryCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsNianRecoveryCandidate(candidate, context);
    internal bool IsNianRuizhiMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paidEffect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsNianRuizhiMovement(frame, paidEffect, pending);
    internal int[] MatchingNianJingjueMovements(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().MatchingNianJingjueMovements(batch, owner);
    internal int NianBaonuDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().NianBaonuDamageBonus(attack);
    internal bool NianDongmianDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().NianDongmianDirectedTargetProhibited(query);
    internal bool NianDongmianForbidsTarget(int actor, int target) => _resolve().NianDongmianForbidsTarget(actor, target);
    internal bool NianEnabled(global::CardGame.Core.CharacterState player, string id) => _resolve().NianEnabled(player, id);
    internal bool NianOwnActualTurn(int seat) => _resolve().NianOwnActualTurn(seat);
    internal bool NianPhaseWasActuallySkipped(int seat, global::CardGame.Core.SkillProgramTurnPhase phase) => _resolve().NianPhaseWasActuallySkipped(seat, phase);
    internal bool NianShouyiUnlimitedCardDistance(global::CardGame.Core.CharacterState player) => _resolve().NianShouyiUnlimitedCardDistance(player);
    internal bool? ProveNianJingjueMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveNianJingjueMovement(trigger, candidate, context);
    internal bool? ProveNianRecovery(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveNianRecovery(trigger, candidate, context);
    internal void RecordNianActualPhaseSkips(int owner, bool draw, bool play) => _resolve().RecordNianActualPhaseSkips(owner, draw, play);
    internal void ReturnNianOperationDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnNianOperationDraw(frame, outcome);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateNianJingjueContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateNianJingjueContext(frame, candidate);
}
