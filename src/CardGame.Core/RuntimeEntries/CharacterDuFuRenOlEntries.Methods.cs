#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuFuRenOlEntries
{
    internal bool AdvanceDuFuRenSubtree() => _resolve().AdvanceDuFuRenSubtree();
    internal global::CardGame.Core.PromptChoice AiDuFuRen(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiDuFuRen(p, f);
    internal void AnswerDuFuRen(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerDuFuRen(choice);
    internal void ApplyDuFuRenChainedDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ApplyDuFuRenChainedDamageBonus(attack);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DuFuRenBlackIssuedEvent> AvailableDuFuRenBlack(int target) => _resolve().AvailableDuFuRenBlack(target);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDuFuRen(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginDuFuRen(supplied, op);
    internal global::CardGame.Core.CardMovementBatchContext CaptureDuFuRenGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureDuFuRenGainBatch(batch);
    internal void CaptureDuFuRenOwnedOrigin(global::CardGame.Core.CardMovementRecord m) => _resolve().CaptureDuFuRenOwnedOrigin(m);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectDuFuRenYiseCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectDuFuRenYiseCandidates(batch);
    internal void CollectWindow_CollectDuFuRenYiseCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectDuFuRenYiseCandidates(batch, result);
    internal void CompleteDuFuRen(global::CardGame.Core.ProgramSkillFrame f, bool applied) => _resolve().CompleteDuFuRen(f, applied);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureDuFuRenGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuFuRenGainBatch(batch, completed);
    internal void ConfirmDuFuRenPlayPhase(global::CardGame.Core.IGameEvent payload) => _resolve().ConfirmDuFuRenPlayPhase(payload);
    internal void ConsumeDuFuRenActualDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeDuFuRenActualDamage(payload);
    internal void ConsumeDuFuRenPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => _resolve().ConsumeDuFuRenPhase(kind, ownerSeat);
    internal void ContinueDuFuRen(long id) => _resolve().ContinueDuFuRen(id);
    internal bool ContinuePaidDuFuRen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidDuFuRen(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateDuFuRenYiseContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateDuFuRenYiseContext(frame, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DuFuRenGain>? CurrentDuFuRenGains(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentDuFuRenGains(c, context);
    internal bool DrainDuFuRenChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainDuFuRenChildren(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DuFuRenChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuFuRenChoices(f);
    internal int DuFuRenDamageCursor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().DuFuRenDamageCursor(attack);
    internal global::CardGame.Core.DuFuRenDamageReceipt? DuFuRenDamageState(long id) => _resolve().DuFuRenDamageState(id);
    internal bool DuFuRenFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DuFuRenFirstChild(root, child);
    internal bool DuFuRenGrantConsumed(long id, global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind) => _resolve().DuFuRenGrantConsumed(id, kind);
    internal global::CardGame.Core.ProgramSkillFrame? DuFuRenObserverRoot() => _resolve().DuFuRenObserverRoot();
    internal int DuFuRenPhaseBonus(int ownerSeat, global::CardGame.Core.SkillRuleQuery query) => _resolve().DuFuRenPhaseBonus(ownerSeat, query);
    internal int DuFuRenYiseDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().DuFuRenYiseDamageBonus(attack);
    internal bool ExcludeOrdinaryMovement_IsDuFuRenYiseTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsDuFuRenYiseTrigger(candidate, trigger);
    internal bool HasDuFuRenDamageObserver(long id) => _resolve().HasDuFuRenDamageObserver(id);
    internal bool IsDuFuRenAwaitedMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsDuFuRenAwaitedMovement(f, e, p);
    internal bool IsDuFuRenGiftContext(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsDuFuRenGiftContext(c, context);
    internal bool IsDuFuRenRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDuFuRenRecoveryParent(f, recovery);
    internal void PrepareDuFuRenChainAdvance(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().PrepareDuFuRenChainAdvance(attack);
    internal bool? ProveDuFuRenMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveDuFuRenMovement(trigger, candidate, context);
    internal void PublishDuFuRenChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDuFuRenChoice(f);
    internal bool QualifyDuFuRen(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyDuFuRen(c, t, context);
    internal void RecordDuFuRenOwnedTransferProducer(global::CardGame.Core.CardMovementRecord m, int recipientSeat, long producerId) => _resolve().RecordDuFuRenOwnedTransferProducer(m, recipientSeat, producerId);
    internal bool ResumeDuFuRen(long id) => _resolve().ResumeDuFuRen(id);
    internal bool ReturnDuFuRenMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDuFuRenMovement(f);
    internal void SetDuFuRenChainDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => _resolve().SetDuFuRenChainDamage(attack, amount);
    internal void SetDuFuRenDamageState(long id, global::CardGame.Core.DuFuRenDamageReceipt? receipt) => _resolve().SetDuFuRenDamageState(id, receipt);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateDuFuRenYiseContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateDuFuRenYiseContext(frame, candidate);
    internal bool TryReturnDuFuRenPaidPreparation(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnDuFuRenPaidPreparation(frame);
    internal bool HasDuFuRenShunshi { get => _resolve().HasDuFuRenShunshi; }
    internal bool HasDuFuRenYise { get => _resolve().HasDuFuRenYise; }
}
