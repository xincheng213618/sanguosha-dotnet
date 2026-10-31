#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFaZhengMouMobileEntries
{
    internal bool AdvanceMouFaZhengChildren() => _resolve().AdvanceMouFaZhengChildren();
    internal PromptChoice AiMouFaZheng(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouFaZheng(p, f);
    internal void AnswerMouFaZheng(PromptChoice choice) => _resolve().AnswerMouFaZheng(choice);
    internal bool CanMouFaZhengMark(int owner, int target) => _resolve().CanMouFaZhengMark(owner, target);
    internal bool CanOfferMouFaZheng(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferMouFaZheng(c, t, context);
    internal bool CanStartMouFaZheng(CharacterState owner) => _resolve().CanStartMouFaZheng(owner);
    internal void CaptureMouFaZhengGainBatch(CardMovementBatchContext batch) => _resolve().CaptureMouFaZhengGainBatch(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouFaZhengGainCandidates(CardMovementBatchContext batch) => _resolve().CollectMouFaZhengGainCandidates(batch);
    internal void CollectWindow_CollectMouFaZhengGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouFaZhengGainCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouFaZhengGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouFaZhengGainBatch(batch, completed);
    internal bool ContinueAcceptedMouFaZheng(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouFaZheng(f);
    internal void ContinueMouFaZheng(long id) => _resolve().ContinueMouFaZheng(id);
    internal ProgramSkillWindowContext CreateMouFaZhengGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate c) => _resolve().CreateMouFaZhengGainContext(parent, c);
    internal bool DrainMouFaZhengChildren(ProgramSkillFrame f) => _resolve().DrainMouFaZhengChildren(f);
    internal bool ExcludeOrdinaryMovement_IsMouFaZhengGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouFaZhengGainTrigger(candidate, trigger);
    internal void FinishMouFaZheng(ProgramSkillFrame f) => _resolve().FinishMouFaZheng(f);
    internal bool IsLiveMouFaZhengSource(MouFaZhengMarkEvent mark) => _resolve().IsLiveMouFaZhengSource(mark);
    internal bool IsMouFaZhengDirectRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouFaZhengDirectRecovery(f, recovery);
    internal bool IsMouFaZhengDying() => _resolve().IsMouFaZhengDying();
    internal bool IsMouFaZhengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouFaZhengMovement(f, e, p);
    internal bool IsMouFaZhengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouFaZhengRecoveryParent(f, recovery);
    internal bool IsMouFaZhengSilverLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().IsMouFaZhengSilverLionChild(f, child);
    internal IReadOnlyList<PromptChoice> MouFaZhengChoices(ProgramSkillFrame f) => _resolve().MouFaZhengChoices(f);
    internal int MouFaZhengExtracted(long mark) => _resolve().MouFaZhengExtracted(mark);
    internal bool MouFaZhengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouFaZhengFirstChild(f, child);
    internal IEnumerable<MouFaZhengMarkEvent> MouFaZhengMarks() => _resolve().MouFaZhengMarks();
    internal bool MouFaZhengObserverEdge(int n) => _resolve().MouFaZhengObserverEdge(n);
    internal ProgramSkillFrame? MouFaZhengObserverRoot() => _resolve().MouFaZhengObserverRoot();
    internal bool MouFaZhengObserverSuffix(int n) => _resolve().MouFaZhengObserverSuffix(n);
    internal IReadOnlyList<PromptChoice> MouFaZhengPayments(ProgramSkillFrame f, int target) => _resolve().MouFaZhengPayments(f, target);
    internal MouFaZhengGainQualifiedEvent? MouFaZhengQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().MouFaZhengQualification(c, context);
    internal int MouFaZhengReceived(long mark) => _resolve().MouFaZhengReceived(mark);
    internal int MouFaZhengUsed(int owner, string skill, int fallback) => _resolve().MouFaZhengUsed(owner, skill, fallback);
    internal void NextMouFaZhengMark(ProgramSkillFrame f) => _resolve().NextMouFaZhengMark(f);
    internal void ObserveMouFaZhengLoss(ProgramSkillHpLostEvent lost) => _resolve().ObserveMouFaZhengLoss(lost);
    internal void PayMouFaZhengMovement(ProgramSkillFrame f, Card card, CardLocation from, int target, string purpose) => _resolve().PayMouFaZhengMovement(f, card, from, target, purpose);
    internal bool? ProveMouFaZhengMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveMouFaZhengMovement(trigger, candidate, context);
    internal void PublishMouFaZheng(ProgramSkillFrame f) => _resolve().PublishMouFaZheng(f);
    internal void RemoveMouFaZhengMark(ProgramSkillFrame f) => _resolve().RemoveMouFaZhengMark(f);
    internal bool ResumeMouFaZheng(long id) => _resolve().ResumeMouFaZheng(id);
    internal bool ReturnMouFaZhengMovement(ProgramSkillFrame f) => _resolve().ReturnMouFaZhengMovement(f);
    internal SkillProgramStepOutcome StartMouFaZheng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().StartMouFaZheng(supplied, op);
    internal void SynchronizeMouFaZhengMarks() => _resolve().SynchronizeMouFaZhengMarks();
    internal ProgramSkillWindowContext? TryCreateMouFaZhengGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouFaZhengGainContext(frame, candidate);
    internal bool ValidMouFaZheng(ProgramSkillFrame f) => _resolve().ValidMouFaZheng(f);
    internal bool HasMouFaZhengCapability { get => _resolve().HasMouFaZhengCapability; }
}
