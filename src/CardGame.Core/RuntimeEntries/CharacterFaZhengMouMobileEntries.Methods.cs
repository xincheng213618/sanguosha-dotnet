#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFaZhengMouMobileEntries
{
    internal bool AdvanceMouFaZhengChildren() => _resolve().AdvanceMouFaZhengChildren();
    internal global::CardGame.Core.PromptChoice AiMouFaZheng(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouFaZheng(p, f);
    internal void AnswerMouFaZheng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouFaZheng(choice);
    internal bool CanMouFaZhengMark(int owner, int target) => _resolve().CanMouFaZhengMark(owner, target);
    internal bool CanOfferMouFaZheng(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMouFaZheng(c, t, context);
    internal bool CanStartMouFaZheng(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouFaZheng(owner);
    internal void CaptureMouFaZhengGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouFaZhengGainBatch(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectMouFaZhengGainCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouFaZhengGainCandidates(batch);
    internal void CollectWindow_CollectMouFaZhengGainCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouFaZhengGainCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouFaZhengGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouFaZhengGainBatch(batch, completed);
    internal bool ContinueAcceptedMouFaZheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouFaZheng(f);
    internal void ContinueMouFaZheng(long id) => _resolve().ContinueMouFaZheng(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouFaZhengGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame parent, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateMouFaZhengGainContext(parent, c);
    internal bool DrainMouFaZhengChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouFaZhengChildren(f);
    internal bool ExcludeOrdinaryMovement_IsMouFaZhengGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouFaZhengGainTrigger(candidate, trigger);
    internal void FinishMouFaZheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouFaZheng(f);
    internal bool HasMouFaZhengBeforeDamageObserver(long id) => _resolve().HasMouFaZhengBeforeDamageObserver(id);
    internal bool HasMouFaZhengDamageObserver(long id) => _resolve().HasMouFaZhengDamageObserver(id);
    internal bool IsLiveMouFaZhengSource(global::CardGame.Core.MouFaZhengMarkEvent mark) => _resolve().IsLiveMouFaZhengSource(mark);
    internal bool IsMouFaZhengDirectRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouFaZhengDirectRecovery(f, recovery);
    internal bool IsMouFaZhengDying() => _resolve().IsMouFaZhengDying();
    internal bool IsMouFaZhengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouFaZhengMovement(f, e, p);
    internal bool IsMouFaZhengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouFaZhengRecoveryParent(f, recovery);
    internal bool IsMouFaZhengSilverLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().IsMouFaZhengSilverLionChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouFaZhengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouFaZhengChoices(f);
    internal int MouFaZhengExtracted(long mark) => _resolve().MouFaZhengExtracted(mark);
    internal bool MouFaZhengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouFaZhengFirstChild(f, child);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouFaZhengMarkEvent> MouFaZhengMarks() => _resolve().MouFaZhengMarks();
    internal bool MouFaZhengObserverEdge(int n) => _resolve().MouFaZhengObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouFaZhengObserverRoot() => _resolve().MouFaZhengObserverRoot();
    internal bool MouFaZhengObserverSuffix(int n) => _resolve().MouFaZhengObserverSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouFaZhengPayments(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().MouFaZhengPayments(f, target);
    internal global::CardGame.Core.MouFaZhengGainQualifiedEvent? MouFaZhengQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MouFaZhengQualification(c, context);
    internal int MouFaZhengReceived(long mark) => _resolve().MouFaZhengReceived(mark);
    internal int MouFaZhengUsed(int owner, string skill, int fallback) => _resolve().MouFaZhengUsed(owner, skill, fallback);
    internal void NextMouFaZhengMark(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextMouFaZhengMark(f);
    internal void ObserveMouFaZhengLoss(global::CardGame.Core.ProgramSkillHpLostEvent lost) => _resolve().ObserveMouFaZhengLoss(lost);
    internal void PayMouFaZhengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, int target, string purpose) => _resolve().PayMouFaZhengMovement(f, card, from, target, purpose);
    internal bool? ProveMouFaZhengMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveMouFaZhengMovement(trigger, candidate, context);
    internal void PublishMouFaZheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouFaZheng(f);
    internal void RemoveMouFaZhengMark(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RemoveMouFaZhengMark(f);
    internal bool ResumeMouFaZheng(long id) => _resolve().ResumeMouFaZheng(id);
    internal bool ReturnMouFaZhengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouFaZhengMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouFaZheng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().StartMouFaZheng(supplied, op);
    internal void SynchronizeMouFaZhengMarks() => _resolve().SynchronizeMouFaZhengMarks();
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouFaZhengGainContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouFaZhengGainContext(frame, candidate);
    internal bool ValidMouFaZheng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouFaZheng(f);
    internal bool HasMouFaZhengCapability { get => _resolve().HasMouFaZhengCapability; }
}
