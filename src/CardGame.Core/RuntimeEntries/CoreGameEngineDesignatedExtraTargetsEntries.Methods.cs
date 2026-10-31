#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDesignatedExtraTargetsEntries
{
    internal SkillProgramStepOutcome AddOneDistanceFreeCurrentUseTarget(ProgramSkillFrame supplied) => _resolve().AddOneDistanceFreeCurrentUseTarget(supplied);
    internal void AssertDesignatedExtraTargetDraft(ProgramSkillFrame frame, SkillProgramEffect paused) => _resolve().AssertDesignatedExtraTargetDraft(frame, paused);
    internal IReadOnlyList<PromptChoice> BuildDesignatedExtraTargetChoices(ProgramSkillFrame frame, CardUseFrame use) => _resolve().BuildDesignatedExtraTargetChoices(frame, use);
    internal bool CanBeDistanceFreeDesignatedTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeDistanceFreeDesignatedTarget(use, target);
    internal bool CanOfferDesignatedExtraTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDesignatedExtraTarget(candidate, trigger, context);
    internal CardActionContext CloneDesignatedExtraTargetAction(CardActionContext action, IReadOnlyList<int> targets) => _resolve().CloneDesignatedExtraTargetAction(action, targets);
    internal bool ContinueDesignatedVirtualBasicEffect(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueDesignatedVirtualBasicEffect(id, continuation);
    internal bool DesignatedExtraTargetCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramCardTriggerWindowFrame window) => _resolve().DesignatedExtraTargetCandidateMatches(candidate, context, window);
    internal IReadOnlyList<IReadOnlyList<int>> DesignatedExtraTargetCandidates(CardUseFrame use) => _resolve().DesignatedExtraTargetCandidates(use);
    internal bool DesignatedExtraTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().DesignatedExtraTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal CardConversionSource DesignatedExtraTargetSource(ProgramSkillFrame frame) => _resolve().DesignatedExtraTargetSource(frame);
    internal bool HasDesignatedExtraTargetTail(CardUseFrame use) => _resolve().HasDesignatedExtraTargetTail(use);
    internal bool HasDiscardedDesignatedCardName(CardKind kind) => _resolve().HasDiscardedDesignatedCardName(kind);
    internal bool IsDesignatedExtraTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> originalTargets) => _resolve().IsDesignatedExtraTargetFact(use, fact, originalTargets);
    internal bool IsDesignatedExtraTargetRedirectFact(CardUseFrame use, DesignatedExtraTargetRedirectedEvent fact, IReadOnlyList<int> actualTargets) => _resolve().IsDesignatedExtraTargetRedirectFact(use, fact, actualTargets);
    internal void RecordDesignatedExtraTargetRedirect(ProgramSkillFrame redirector, long useId, int originalSeat, int newSeat) => _resolve().RecordDesignatedExtraTargetRedirect(redirector, useId, originalSeat, newSeat);
    internal CardUseFrame RequireDesignatedExtraTargetParent(ProgramSkillFrame frame, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireDesignatedExtraTargetParent(frame, out window);
    internal void ResolveDesignatedExtraTargetChoice(PromptChoice selected) => _resolve().ResolveDesignatedExtraTargetChoice(selected);
    internal PromptChoice SelectAiDesignatedExtraTargetChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiDesignatedExtraTargetChoice(decision, frame);
    internal bool TryContinueDesignatedVirtualBasicUse(long id) => _resolve().TryContinueDesignatedVirtualBasicUse(id);
    internal bool TryGetDesignatedExtraTargetUse(int owner, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetDesignatedExtraTargetUse(owner, context, out use, out window);
    internal bool TryGetDesignatedExtraTargetVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> targets) => _resolve().TryGetDesignatedExtraTargetVirtualPrimaryReturn(use, parent, out targets);
    internal bool WasDesignatedExtraTargetOffered(CardUseFrame use, CardConversionSource source) => _resolve().WasDesignatedExtraTargetOffered(use, source);
}
