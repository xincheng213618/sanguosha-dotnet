#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDesignatedExtraTargetsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AddOneDistanceFreeCurrentUseTarget(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().AddOneDistanceFreeCurrentUseTarget(supplied);
    internal void AssertDesignatedExtraTargetDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertDesignatedExtraTargetDraft(frame, paused);
    internal void AssertRegisteredDesignatedExtraTargetDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertRegisteredDesignatedExtraTargetDraft(frame, plan);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildDesignatedExtraTargetChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use) => _resolve().BuildDesignatedExtraTargetChoices(frame, use);
    internal bool CanBeDistanceFreeDesignatedTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeDistanceFreeDesignatedTarget(use, target);
    internal bool CanOfferDesignatedExtraTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDesignatedExtraTarget(candidate, trigger, context);
    internal global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().CloneDesignatedExtraTargetAction(action, targets);
    internal bool ContinueDesignatedVirtualBasicEffect(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueDesignatedVirtualBasicEffect(id, continuation);
    internal bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().DesignatedExtraTargetCandidateMatches(candidate, context, window);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> DesignatedExtraTargetCandidates(global::CardGame.Core.CardUseFrame use) => _resolve().DesignatedExtraTargetCandidates(use);
    internal bool DesignatedExtraTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().DesignatedExtraTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal global::CardGame.Core.CardConversionSource DesignatedExtraTargetSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DesignatedExtraTargetSource(frame);
    internal bool HasDesignatedExtraTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasDesignatedExtraTargetTail(use);
    internal bool HasDiscardedDesignatedCardName(global::CardGame.Core.CardKind kind) => _resolve().HasDiscardedDesignatedCardName(kind);
    internal bool IsDesignatedExtraTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().IsDesignatedExtraTargetFact(use, fact, originalTargets);
    internal bool IsDesignatedExtraTargetRedirectFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetRedirectedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> actualTargets) => _resolve().IsDesignatedExtraTargetRedirectFact(use, fact, actualTargets);
    internal void RecordDesignatedExtraTargetRedirect(global::CardGame.Core.ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordDesignatedExtraTargetRedirect(redirector, useId, originalSeat, newSeat);
    internal global::CardGame.Core.CardUseFrame RequireDesignatedExtraTargetParent(global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireDesignatedExtraTargetParent(frame, out window);
    internal void ResolveDesignatedExtraTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDesignatedExtraTargetChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiDesignatedExtraTargetChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiDesignatedExtraTargetChoice(decision, frame);
    internal bool TryContinueDesignatedVirtualBasicUse(long id) => _resolve().TryContinueDesignatedVirtualBasicUse(id);
    internal bool TryGetDesignatedExtraTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetDesignatedExtraTargetUse(owner, context, out use, out window);
    internal bool TryGetDesignatedExtraTargetVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryGetDesignatedExtraTargetVirtualPrimaryReturn(use, parent, out targets);
    internal bool WasDesignatedExtraTargetOffered(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardConversionSource source) => _resolve().WasDesignatedExtraTargetOffered(use, source);
}
