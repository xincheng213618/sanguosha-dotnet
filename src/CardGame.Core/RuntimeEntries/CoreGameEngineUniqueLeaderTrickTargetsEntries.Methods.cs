#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsEntries
{
    internal IReadOnlyList<PromptChoice> BuildUniqueLeaderTrickTargetChoices(ProgramSkillFrame frame, CardUseFrame use, SkillProgramEffectOp operation) => _resolve().BuildUniqueLeaderTrickTargetChoices(frame, use, operation);
    internal bool CanBeUniqueLeaderTrickTarget(CardUseFrame use, CharacterState target) => _resolve().CanBeUniqueLeaderTrickTarget(use, target);
    internal bool CanOfferUniqueLeaderTrickTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferUniqueLeaderTrickTarget(candidate, trigger, context);
    internal void CaptureUniqueLeaderTrickQualification(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().CaptureUniqueLeaderTrickQualification(action, window);
    internal bool HasUniqueLeaderTrickTargetTail(CardUseFrame use) => _resolve().HasUniqueLeaderTrickTargetTail(use);
    internal bool IsUniqueLeaderTrickTargetFact(CardUseFrame use, UniqueLeaderTrickTargetResolvedEvent fact, IReadOnlyList<int> originalTargets) => _resolve().IsUniqueLeaderTrickTargetFact(use, fact, originalTargets);
    internal bool LargestHandTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().LargestHandTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool LargestHpTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().LargestHpTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal SkillProgramStepOutcome OfferUniqueLeaderTrickTarget(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().OfferUniqueLeaderTrickTarget(supplied, operation);
    internal CardUseFrame RequireUniqueLeaderTrickParent(ProgramSkillFrame frame, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireUniqueLeaderTrickParent(frame, out window);
    internal void ResolveUniqueLeaderTrickTargetChoice(PromptChoice selected) => _resolve().ResolveUniqueLeaderTrickTargetChoice(selected);
    internal PromptChoice SelectAiUniqueLeaderTrickTargetChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiUniqueLeaderTrickTargetChoice(decision, frame);
    internal bool TryGetUniqueLeaderTrickUse(ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetUniqueLeaderTrickUse(context, out use, out window);
    internal bool UniqueLeaderTrickEligible(CardUseFrame use, int owner, SkillProgramEffectOp operation) => _resolve().UniqueLeaderTrickEligible(use, owner, operation);
    internal UniqueLeaderTrickQualificationEvent? UniqueLeaderTrickQualification(CardUseFrame use) => _resolve().UniqueLeaderTrickQualification(use);
    internal IReadOnlyList<IReadOnlyList<int>> UniqueLeaderTrickTargetCandidates(CardUseFrame use, int owner, SkillProgramEffectOp operation) => _resolve().UniqueLeaderTrickTargetCandidates(use, owner, operation);
    internal bool TracksUniqueLeaderTrickTargets { get => _resolve().TracksUniqueLeaderTrickTargets; }
}
