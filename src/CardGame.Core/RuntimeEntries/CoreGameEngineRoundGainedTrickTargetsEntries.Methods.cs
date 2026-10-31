#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedTrickTargetsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildRoundGainedTrickTargetChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use) => _resolve().BuildRoundGainedTrickTargetChoices(frame, use);
    internal bool CanOfferRoundGainedTrickTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferRoundGainedTrickTarget(candidate, trigger, context);
    internal bool HasRoundGainedTrickTargetCancellation(global::CardGame.Core.CardUseFrame use) => _resolve().HasRoundGainedTrickTargetCancellation(use);
    internal bool HasRoundGainedTrickTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasRoundGainedTrickTargetTail(use);
    internal bool IsRoundGainedTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundGainedTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().IsRoundGainedTrickTargetFact(use, fact, originalTargets);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferRoundGainedTrickTarget(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().OfferRoundGainedTrickTarget(supplied);
    internal global::CardGame.Core.RoundGainedUseQualification RequireRoundGainedTrickTargetParent(global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().RequireRoundGainedTrickTargetParent(frame, out use);
    internal void ResolveRoundGainedTrickTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveRoundGainedTrickTargetChoice(selected);
    internal global::System.Collections.Generic.IReadOnlyList<int> RoundGainedOriginalTargets(global::CardGame.Core.CardUseFrame use) => _resolve().RoundGainedOriginalTargets(use);
    internal bool RoundGainedTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().RoundGainedTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal global::CardGame.Core.PromptChoice SelectAiRoundGainedTrickTargetChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiRoundGainedTrickTargetChoice(decision, frame);
}
