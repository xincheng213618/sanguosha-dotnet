#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoLeEntries
{
    internal bool AdvanceLeChengyanBorrowedAi() => _resolve().AdvanceLeChengyanBorrowedAi();
    internal bool AdvanceLeChengyanChildren() => _resolve().AdvanceLeChengyanChildren();
    internal void AnswerLeChengyanBorrowedVictim(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerLeChengyanBorrowedVictim(selected);
    internal void BeginLeChengyanBorrowedWithAuxiliary(global::CardGame.Core.CardUseFrame supplied) => _resolve().BeginLeChengyanBorrowedWithAuxiliary(supplied);
    internal void BeginLeChengyanSlash(global::CardGame.Core.CardUseFrame u) => _resolve().BeginLeChengyanSlash(u);
    internal bool CanOfferLeChengyan(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferLeChengyan(gate);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLeXidiOpening(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w) => _resolve().CaptureLeXidiOpening(w);
    internal bool ContinueAcceptedLeChengyan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedLeChengyan(f);
    internal void ContinueChengyanAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueChengyanAfterPublicReveal(f);
    internal void ContinueLeChengyan(long id) => _resolve().ContinueLeChengyan(id);
    internal void FinishLeChengyan(global::CardGame.Core.ProgramSkillFrame f, bool replaced) => _resolve().FinishLeChengyan(f, replaced);
    internal bool FinishLeChengyanAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishLeChengyanAttack(attack, out paused);
    internal bool FinishLeChengyanBorrowedNative(global::CardGame.Core.NativeOwners.BorrowedSwordHandle borrowed) => _resolve().FinishLeChengyanBorrowedNative(borrowed);
    internal bool HasLeChengyanBeforeDamageObserver(long id) => _resolve().HasLeChengyanBeforeDamageObserver(id);
    internal bool HasLeChengyanBorrowedPrompt() => _resolve().HasLeChengyanBorrowedPrompt();
    internal bool HasLeChengyanDamageObserver(long id) => _resolve().HasLeChengyanDamageObserver(id);
    internal bool HasLeXidiEntityMark(int cardId) => _resolve().HasLeXidiEntityMark(cardId);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeLeXidi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeLeXidi(supplied);
    internal bool IsLeChengyanDying() => _resolve().IsLeChengyanDying();
    internal bool IsLeChengyanEffectUse(long id) => _resolve().IsLeChengyanEffectUse(id);
    internal bool IsLeChengyanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLeChengyanMovement(f, e, p);
    internal bool IsLeChengyanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLeChengyanRecoveryParent(f, recovery);
    internal bool IsLeXidiHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsLeXidiHandLimitExempt(owner, cardId);
    internal bool KeepLeChengyanBorrowedMaterial(long id) => _resolve().KeepLeChengyanBorrowedMaterial(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LeChengyanBorrowedChoices(global::CardGame.Core.CardUseFrame u) => _resolve().LeChengyanBorrowedChoices(u);
    internal bool LeChengyanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LeChengyanFirstChild(f, child);
    internal bool LeChengyanObserverEdge(int n) => _resolve().LeChengyanObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? LeChengyanObserverRoot() => _resolve().LeChengyanObserverRoot();
    internal bool LeChengyanObserverSuffix(int index) => _resolve().LeChengyanObserverSuffix(index);
    internal global::CardGame.Core.Card LeChengyanOriginalCard(global::CardGame.Core.CardUseFrame u) => _resolve().LeChengyanOriginalCard(u);
    internal bool LeZhugeGuoFinalizedBeforeActivityTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().LeZhugeGuoFinalizedBeforeActivityTargetWindow(query);
    internal void ProjectLeXidiCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectLeXidiCardSkillMark(cardId, ref projection);
    internal global::CardGame.Core.SkillProgramStepOutcome ReorderLeXidi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReorderLeXidi(supplied);
    internal bool ResumeLeChengyan(long id) => _resolve().ResumeLeChengyan(id);
    internal void ReturnLeChengyanDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnLeChengyanDraw(f, d);
    internal bool ReturnLeChengyanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLeChengyanMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome StartLeChengyan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartLeChengyan(supplied);
    internal void StartLeChengyanNativeEffect(global::CardGame.Core.CardUseFrame u) => _resolve().StartLeChengyanNativeEffect(u);
    internal bool TryBypassLeChengyanNullification(long id, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.LegalActionKind action, int? targetCard, global::CardGame.Core.CardKind? required) => _resolve().TryBypassLeChengyanNullification(id, card, targets, action, targetCard, required);
    internal bool TryContinueLeChengyanBorrowed(long id) => _resolve().TryContinueLeChengyanBorrowed(id);
    internal bool TryResolveLeChengyanNativeEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().TryResolveLeChengyanNativeEffect(pending);
    internal bool TryStartLeChengyanEffect(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryStartLeChengyanEffect(window);
    internal bool ValidLeChengyan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidLeChengyan(f);
    internal bool ValidLeChengyanEffect(global::CardGame.Core.CardUseFrame u) => _resolve().ValidLeChengyanEffect(u);
    internal bool HasLeZhugeGuoCapability { get => _resolve().HasLeZhugeGuoCapability; }
}
