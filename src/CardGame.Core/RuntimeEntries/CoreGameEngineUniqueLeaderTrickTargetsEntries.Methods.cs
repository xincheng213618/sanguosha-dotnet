#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsEntries
{
    internal void AssertUniqueLeaderTrickTargetDraft(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertUniqueLeaderTrickTargetDraft(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildUniqueLeaderTrickTargetChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BuildUniqueLeaderTrickTargetChoices(frame, use, operation);
    internal bool CanBeUniqueLeaderTrickTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target) => _resolve().CanBeUniqueLeaderTrickTarget(use, target);
    internal bool CanOfferUniqueLeaderTrickTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferUniqueLeaderTrickTarget(candidate, trigger, context);
    internal void CaptureUniqueLeaderTrickQualification(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().CaptureUniqueLeaderTrickQualification(action, window);
    internal bool HasUniqueLeaderTrickTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasUniqueLeaderTrickTargetTail(use);
    internal bool IsUniqueLeaderTrickTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.UniqueLeaderTrickTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().IsUniqueLeaderTrickTargetFact(use, fact, originalTargets);
    internal bool LargestHandTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().LargestHandTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool LargestHpTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().LargestHpTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferUniqueLeaderTrickTarget(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().OfferUniqueLeaderTrickTarget(supplied, operation);
    internal global::CardGame.Core.CardUseFrame RequireUniqueLeaderTrickParent(global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireUniqueLeaderTrickParent(frame, out window);
    internal void ResolveUniqueLeaderTrickTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveUniqueLeaderTrickTargetChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiUniqueLeaderTrickTargetChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiUniqueLeaderTrickTargetChoice(decision, frame);
    internal bool TryGetUniqueLeaderTrickUse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetUniqueLeaderTrickUse(context, out use, out window);
    internal bool UniqueLeaderTrickEligible(global::CardGame.Core.CardUseFrame use, int owner, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().UniqueLeaderTrickEligible(use, owner, operation);
    internal global::CardGame.Core.UniqueLeaderTrickQualificationEvent? UniqueLeaderTrickQualification(global::CardGame.Core.CardUseFrame use) => _resolve().UniqueLeaderTrickQualification(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> UniqueLeaderTrickTargetCandidates(global::CardGame.Core.CardUseFrame use, int owner, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().UniqueLeaderTrickTargetCandidates(use, owner, operation);
    internal bool TracksUniqueLeaderTrickTargets { get => _resolve().TracksUniqueLeaderTrickTargets; }
}
