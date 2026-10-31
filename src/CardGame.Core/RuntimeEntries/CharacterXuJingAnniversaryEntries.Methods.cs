#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuJingAnniversaryEntries
{
    internal bool AdvanceAnniversaryXuJingChildren() => _resolve().AdvanceAnniversaryXuJingChildren();
    internal IReadOnlyList<PromptChoice> AnniversaryXuJingChoices(ProgramSkillFrame f) => _resolve().AnniversaryXuJingChoices(f);
    internal GameEngine.IDamageAttempt? AnniversaryXuJingDamage(ProgramSkillWindowContext context) => _resolve().AnniversaryXuJingDamage(context);
    internal AnniversaryXuJingCaixiaIssuedEvent? AnniversaryXuJingDebt(int owner) => _resolve().AnniversaryXuJingDebt(owner);
    internal int[] AnniversaryXuJingDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().AnniversaryXuJingDiscardIndexes(batch, candidate);
    internal bool AnniversaryXuJingDyingObserver() => _resolve().AnniversaryXuJingDyingObserver();
    internal bool AnniversaryXuJingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnniversaryXuJingFirstChild(f, child);
    internal AnniversaryXuJingMarkedEvent? AnniversaryXuJingMark(int owner, string? instance = default) => _resolve().AnniversaryXuJingMark(owner, instance);
    internal bool AnniversaryXuJingMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().AnniversaryXuJingMovementProof(f, effect, pending);
    internal ProgramSkillFrame? AnniversaryXuJingObserverRoot() => _resolve().AnniversaryXuJingObserverRoot();
    internal int[] AnniversaryXuJingRecipients(int owner, int card) => _resolve().AnniversaryXuJingRecipients(owner, card);
    internal bool AnniversaryXuJingRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().AnniversaryXuJingRecoveryParent(f, recovery);
    internal int AnniversaryXuJingRemaining(int owner) => _resolve().AnniversaryXuJingRemaining(owner);
    internal void AnswerAnniversaryXuJing(PromptChoice choice) => _resolve().AnswerAnniversaryXuJing(choice);
    internal SkillProgramStepOutcome BeginAnniversaryXuJing(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXuJing(supplied, operation);
    internal bool CanOfferAnniversaryXuJing(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryXuJing(candidate, trigger, context);
    internal PromptChoice ChooseAnniversaryXuJingCount(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryXuJingCount(p, f);
    internal PromptChoice ChooseAnniversaryXuJingGift(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryXuJingGift(p, f);
    internal bool ContinueAnniversaryXuJing(long id) => _resolve().ContinueAnniversaryXuJing(id);
    internal bool DrainAnniversaryXuJing(ProgramSkillFrame f) => _resolve().DrainAnniversaryXuJing(f);
    internal void MoveAnniversaryXuJingEntity(ProgramSkillFrame supplied, CardLocation source, CardLocation destination) => _resolve().MoveAnniversaryXuJingEntity(supplied, source, destination);
    internal void PublishAnniversaryXuJing(ProgramSkillFrame f) => _resolve().PublishAnniversaryXuJing(f);
    internal void RecordAnniversaryXuJingActualUse(CurrentTurnCardUseKindsRecordedEvent fact) => _resolve().RecordAnniversaryXuJingActualUse(fact);
    internal bool RetainAnniversaryXuJingIssued(ProgramSkillFrame f) => _resolve().RetainAnniversaryXuJingIssued(f);
    internal void ReturnAnniversaryXuJingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryXuJingDraw(f, outcome);
    internal bool ReturnAnniversaryXuJingMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryXuJingMovement(f);
    internal int[]? SelectAnniversaryXuJingDiscards(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _resolve().SelectAnniversaryXuJingDiscards(batch, candidate, features);
    internal bool HasAnniversaryXuJing { get => _resolve().HasAnniversaryXuJing; }
}
