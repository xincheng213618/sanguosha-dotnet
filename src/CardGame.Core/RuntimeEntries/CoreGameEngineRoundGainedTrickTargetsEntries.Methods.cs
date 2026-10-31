#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedTrickTargetsEntries
{
    internal IReadOnlyList<PromptChoice> BuildRoundGainedTrickTargetChoices(ProgramSkillFrame frame, CardUseFrame use) => _resolve().BuildRoundGainedTrickTargetChoices(frame, use);
    internal bool CanOfferRoundGainedTrickTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferRoundGainedTrickTarget(candidate, trigger, context);
    internal bool HasRoundGainedTrickTargetCancellation(CardUseFrame use) => _resolve().HasRoundGainedTrickTargetCancellation(use);
    internal bool HasRoundGainedTrickTargetTail(CardUseFrame use) => _resolve().HasRoundGainedTrickTargetTail(use);
    internal bool IsRoundGainedTrickTargetFact(CardUseFrame use, RoundGainedTrickTargetResolvedEvent fact, IReadOnlyList<int> originalTargets) => _resolve().IsRoundGainedTrickTargetFact(use, fact, originalTargets);
    internal SkillProgramStepOutcome OfferRoundGainedTrickTarget(ProgramSkillFrame supplied) => _resolve().OfferRoundGainedTrickTarget(supplied);
    internal RoundGainedUseQualification RequireRoundGainedTrickTargetParent(ProgramSkillFrame frame, scoped out CardUseFrame use) => _resolve().RequireRoundGainedTrickTargetParent(frame, out use);
    internal void ResolveRoundGainedTrickTargetChoice(PromptChoice selected) => _resolve().ResolveRoundGainedTrickTargetChoice(selected);
    internal IReadOnlyList<int> RoundGainedOriginalTargets(CardUseFrame use) => _resolve().RoundGainedOriginalTargets(use);
    internal bool RoundGainedTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().RoundGainedTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal PromptChoice SelectAiRoundGainedTrickTargetChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiRoundGainedTrickTargetChoice(decision, frame);
}
