#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuJingAnniversaryEntries
{
    internal bool AdvanceAnniversaryXuJingChildren() => _resolve().AdvanceAnniversaryXuJingChildren();
    internal bool AnniversaryXuJingBeforeDamageObserver(long id) => _resolve().AnniversaryXuJingBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryXuJingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryXuJingChoices(f);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? AnniversaryXuJingDamage(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AnniversaryXuJingDamage(context);
    internal bool AnniversaryXuJingDamageObserver(long id) => _resolve().AnniversaryXuJingDamageObserver(id);
    internal global::CardGame.Core.AnniversaryXuJingCaixiaIssuedEvent? AnniversaryXuJingDebt(int owner) => _resolve().AnniversaryXuJingDebt(owner);
    internal int[] AnniversaryXuJingDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().AnniversaryXuJingDiscardIndexes(batch, candidate);
    internal bool AnniversaryXuJingDyingObserver() => _resolve().AnniversaryXuJingDyingObserver();
    internal bool AnniversaryXuJingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryXuJingFirstChild(f, child);
    internal global::CardGame.Core.AnniversaryXuJingMarkedEvent? AnniversaryXuJingMark(int owner, string? instance = default) => _resolve().AnniversaryXuJingMark(owner, instance);
    internal bool AnniversaryXuJingMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().AnniversaryXuJingMovementProof(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryXuJingObserverRoot() => _resolve().AnniversaryXuJingObserverRoot();
    internal int[] AnniversaryXuJingRecipients(int owner, int card) => _resolve().AnniversaryXuJingRecipients(owner, card);
    internal bool AnniversaryXuJingRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().AnniversaryXuJingRecoveryParent(f, recovery);
    internal int AnniversaryXuJingRemaining(int owner) => _resolve().AnniversaryXuJingRemaining(owner);
    internal void AnswerAnniversaryXuJing(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryXuJing(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryXuJing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversaryXuJing(supplied, operation);
    internal bool CanOfferAnniversaryXuJing(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferAnniversaryXuJing(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryXuJingCount(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryXuJingCount(p, f);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryXuJingGift(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryXuJingGift(p, f);
    internal bool ContinueAnniversaryXuJing(long id) => _resolve().ContinueAnniversaryXuJing(id);
    internal bool DrainAnniversaryXuJing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainAnniversaryXuJing(f);
    internal void MoveAnniversaryXuJingEntity(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.CardLocation source, global::CardGame.Core.CardLocation destination) => _resolve().MoveAnniversaryXuJingEntity(supplied, source, destination);
    internal void PublishAnniversaryXuJing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryXuJing(f);
    internal void RecordAnniversaryXuJingActualUse(global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent fact) => _resolve().RecordAnniversaryXuJingActualUse(fact);
    internal bool RetainAnniversaryXuJingIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainAnniversaryXuJingIssued(f);
    internal void ReturnAnniversaryXuJingDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryXuJingDraw(f, outcome);
    internal bool ReturnAnniversaryXuJingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryXuJingMovement(f);
    internal int[]? SelectAnniversaryXuJingDiscards(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectAnniversaryXuJingDiscards(batch, candidate, features);
    internal bool HasAnniversaryXuJing { get => _resolve().HasAnniversaryXuJing; }
}
