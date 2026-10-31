#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuFuRenOlEntries
{
    internal bool AdvanceDuFuRenSubtree() => _resolve().AdvanceDuFuRenSubtree();
    internal PromptChoice AiDuFuRen(PendingDecision p, ProgramSkillFrame f) => _resolve().AiDuFuRen(p, f);
    internal void AnswerDuFuRen(PromptChoice choice) => _resolve().AnswerDuFuRen(choice);
    internal void ApplyDuFuRenChainedDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ApplyDuFuRenChainedDamageBonus(attack);
    internal IReadOnlyList<DuFuRenBlackIssuedEvent> AvailableDuFuRenBlack(int target) => _resolve().AvailableDuFuRenBlack(target);
    internal SkillProgramStepOutcome BeginDuFuRen(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginDuFuRen(supplied, op);
    internal CardMovementBatchContext CaptureDuFuRenGainBatch(CardMovementBatchContext batch) => _resolve().CaptureDuFuRenGainBatch(batch);
    internal void CaptureDuFuRenOwnedOrigin(CardMovementRecord m) => _resolve().CaptureDuFuRenOwnedOrigin(m);
    internal IEnumerable<ProgramTriggerCandidate> CollectDuFuRenYiseCandidates(CardMovementBatchContext batch) => _resolve().CollectDuFuRenYiseCandidates(batch);
    internal void CollectWindow_CollectDuFuRenYiseCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectDuFuRenYiseCandidates(batch, result);
    internal void CompleteDuFuRen(ProgramSkillFrame f, bool applied) => _resolve().CompleteDuFuRen(f, applied);
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuFuRenGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuFuRenGainBatch(batch, completed);
    internal void ConfirmDuFuRenPlayPhase(IGameEvent payload) => _resolve().ConfirmDuFuRenPlayPhase(payload);
    internal void ConsumeDuFuRenActualDamage(IGameEvent payload) => _resolve().ConsumeDuFuRenActualDamage(payload);
    internal void ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind kind, int ownerSeat) => _resolve().ConsumeDuFuRenPhase(kind, ownerSeat);
    internal void ContinueDuFuRen(long id) => _resolve().ContinueDuFuRen(id);
    internal bool ContinuePaidDuFuRen(ProgramSkillFrame f) => _resolve().ContinuePaidDuFuRen(f);
    internal ProgramSkillWindowContext CreateDuFuRenYiseContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateDuFuRenYiseContext(frame, c);
    internal IReadOnlyList<DuFuRenGain>? CurrentDuFuRenGains(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentDuFuRenGains(c, context);
    internal bool DrainDuFuRenChildren(ProgramSkillFrame f) => _resolve().DrainDuFuRenChildren(f);
    internal IReadOnlyList<PromptChoice> DuFuRenChoices(ProgramSkillFrame f) => _resolve().DuFuRenChoices(f);
    internal int DuFuRenDamageCursor(GameEngine.IDamageAttempt attack) => _resolve().DuFuRenDamageCursor(attack);
    internal DuFuRenDamageReceipt? DuFuRenDamageState(long id) => _resolve().DuFuRenDamageState(id);
    internal bool DuFuRenFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DuFuRenFirstChild(root, child);
    internal bool DuFuRenGrantConsumed(long id, ActualDiscardRecoveryPhaseKind kind) => _resolve().DuFuRenGrantConsumed(id, kind);
    internal ProgramSkillFrame? DuFuRenObserverRoot() => _resolve().DuFuRenObserverRoot();
    internal int DuFuRenPhaseBonus(int ownerSeat, SkillRuleQuery query) => _resolve().DuFuRenPhaseBonus(ownerSeat, query);
    internal int DuFuRenYiseDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().DuFuRenYiseDamageBonus(attack);
    internal bool ExcludeOrdinaryMovement_IsDuFuRenYiseTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsDuFuRenYiseTrigger(candidate, trigger);
    internal bool IsDuFuRenGiftContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsDuFuRenGiftContext(c, context);
    internal bool IsDuFuRenRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsDuFuRenRecoveryParent(f, recovery);
    internal void PrepareDuFuRenChainAdvance(GameEngine.IDamageAttempt attack) => _resolve().PrepareDuFuRenChainAdvance(attack);
    internal bool? ProveDuFuRenMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveDuFuRenMovement(trigger, candidate, context);
    internal void PublishDuFuRenChoice(ProgramSkillFrame f) => _resolve().PublishDuFuRenChoice(f);
    internal bool QualifyDuFuRen(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyDuFuRen(c, t, context);
    internal void RecordDuFuRenOwnedTransferProducer(CardMovementRecord m, int recipientSeat, long producerId) => _resolve().RecordDuFuRenOwnedTransferProducer(m, recipientSeat, producerId);
    internal bool ResumeDuFuRen(long id) => _resolve().ResumeDuFuRen(id);
    internal bool ReturnDuFuRenMovement(ProgramSkillFrame f) => _resolve().ReturnDuFuRenMovement(f);
    internal void SetDuFuRenChainDamage(GameEngine.IDamageAttempt attack, int amount) => _resolve().SetDuFuRenChainDamage(attack, amount);
    internal void SetDuFuRenDamageState(long id, DuFuRenDamageReceipt? receipt) => _resolve().SetDuFuRenDamageState(id, receipt);
    internal ProgramSkillWindowContext? TryCreateDuFuRenYiseContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateDuFuRenYiseContext(frame, candidate);
    internal bool TryReturnDuFuRenPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _resolve().TryReturnDuFuRenPaidPreparation(frame);
    internal bool HasDuFuRenShunshi { get => _resolve().HasDuFuRenShunshi; }
    internal bool HasDuFuRenYise { get => _resolve().HasDuFuRenYise; }
}
