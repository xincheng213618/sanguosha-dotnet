#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoLeEntries
{
    internal bool AdvanceLeChengyanBorrowedAi() => _resolve().AdvanceLeChengyanBorrowedAi();
    internal bool AdvanceLeChengyanChildren() => _resolve().AdvanceLeChengyanChildren();
    internal void AnswerLeChengyanBorrowedVictim(PromptChoice selected) => _resolve().AnswerLeChengyanBorrowedVictim(selected);
    internal void BeginLeChengyanBorrowedWithAuxiliary(CardUseFrame supplied) => _resolve().BeginLeChengyanBorrowedWithAuxiliary(supplied);
    internal void BeginLeChengyanSlash(CardUseFrame u) => _resolve().BeginLeChengyanSlash(u);
    internal bool CanOfferLeChengyan(ProgramTriggerLegalityContext gate) => _resolve().CanOfferLeChengyan(gate);
    internal ProgramLifecycleTriggerWindowFrame CaptureLeXidiOpening(ProgramLifecycleTriggerWindowFrame w) => _resolve().CaptureLeXidiOpening(w);
    internal bool ContinueAcceptedLeChengyan(ProgramSkillFrame f) => _resolve().ContinueAcceptedLeChengyan(f);
    internal void ContinueChengyanAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueChengyanAfterPublicReveal(f);
    internal void ContinueLeChengyan(long id) => _resolve().ContinueLeChengyan(id);
    internal void FinishLeChengyan(ProgramSkillFrame f, bool replaced) => _resolve().FinishLeChengyan(f, replaced);
    internal bool FinishLeChengyanAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishLeChengyanAttack(attack, out paused);
    internal bool FinishLeChengyanBorrowedNative(NativeOwners.BorrowedSwordHandle borrowed) => _resolve().FinishLeChengyanBorrowedNative(borrowed);
    internal bool HasLeChengyanBorrowedPrompt() => _resolve().HasLeChengyanBorrowedPrompt();
    internal bool HasLeXidiEntityMark(int cardId) => _resolve().HasLeXidiEntityMark(cardId);
    internal SkillProgramStepOutcome InitializeLeXidi(ProgramSkillFrame supplied) => _resolve().InitializeLeXidi(supplied);
    internal bool IsLeChengyanDying() => _resolve().IsLeChengyanDying();
    internal bool IsLeChengyanEffectUse(long id) => _resolve().IsLeChengyanEffectUse(id);
    internal bool IsLeChengyanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsLeChengyanMovement(f, e, p);
    internal bool IsLeChengyanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLeChengyanRecoveryParent(f, recovery);
    internal bool IsLeXidiHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsLeXidiHandLimitExempt(owner, cardId);
    internal bool KeepLeChengyanBorrowedMaterial(long id) => _resolve().KeepLeChengyanBorrowedMaterial(id);
    internal IReadOnlyList<PromptChoice> LeChengyanBorrowedChoices(CardUseFrame u) => _resolve().LeChengyanBorrowedChoices(u);
    internal bool LeChengyanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LeChengyanFirstChild(f, child);
    internal bool LeChengyanObserverEdge(int n) => _resolve().LeChengyanObserverEdge(n);
    internal ProgramSkillFrame? LeChengyanObserverRoot() => _resolve().LeChengyanObserverRoot();
    internal bool LeChengyanObserverSuffix(int index) => _resolve().LeChengyanObserverSuffix(index);
    internal Card LeChengyanOriginalCard(CardUseFrame u) => _resolve().LeChengyanOriginalCard(u);
    internal bool LeZhugeGuoFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().LeZhugeGuoFinalizedBeforeActivityTargetWindow(query);
    internal void ProjectLeXidiCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectLeXidiCardSkillMark(cardId, ref projection);
    internal SkillProgramStepOutcome ReorderLeXidi(ProgramSkillFrame supplied) => _resolve().ReorderLeXidi(supplied);
    internal bool ResumeLeChengyan(long id) => _resolve().ResumeLeChengyan(id);
    internal void ReturnLeChengyanDraw(ProgramSkillFrame f, NativeDrawOutcome d) => _resolve().ReturnLeChengyanDraw(f, d);
    internal bool ReturnLeChengyanMovement(ProgramSkillFrame f) => _resolve().ReturnLeChengyanMovement(f);
    internal SkillProgramStepOutcome StartLeChengyan(ProgramSkillFrame supplied) => _resolve().StartLeChengyan(supplied);
    internal void StartLeChengyanNativeEffect(CardUseFrame u) => _resolve().StartLeChengyanNativeEffect(u);
    internal bool TryBypassLeChengyanNullification(long id, Card card, IReadOnlyList<int> targets, LegalActionKind action, int? targetCard, CardKind? required) => _resolve().TryBypassLeChengyanNullification(id, card, targets, action, targetCard, required);
    internal bool TryContinueLeChengyanBorrowed(long id) => _resolve().TryContinueLeChengyanBorrowed(id);
    internal bool TryResolveLeChengyanNativeEffect(NullificationWindowFrame pending) => _resolve().TryResolveLeChengyanNativeEffect(pending);
    internal bool TryStartLeChengyanEffect(ProgramCardTriggerWindowFrame window) => _resolve().TryStartLeChengyanEffect(window);
    internal bool ValidLeChengyan(ProgramSkillFrame f) => _resolve().ValidLeChengyan(f);
    internal bool ValidLeChengyanEffect(CardUseFrame u) => _resolve().ValidLeChengyanEffect(u);
    internal bool HasLeZhugeGuoCapability { get => _resolve().HasLeZhugeGuoCapability; }
}
