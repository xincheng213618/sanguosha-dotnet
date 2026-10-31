#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAlternativePhaseCostProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAlternativePhaseCost(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().BeginAlternativePhaseCost(frame, zones, marker);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildAlternativePhaseCostChoices(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BuildAlternativePhaseCostChoices(frame, effect);
    internal bool CanOfferAlternativePhaseCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CanOfferAlternativePhaseCost(owner, trigger, candidate);
    internal void CompleteActualJudgmentStarting(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteActualJudgmentStarting(frame);
    internal bool IsAlternativePhaseCostChoiceLegal(global::CardGame.Core.PromptChoice choice) => _resolve().IsAlternativePhaseCostChoiceLegal(choice);
    internal bool IsConfirmedActualStartingSubstitution(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsConfirmedActualStartingSubstitution(context);
    internal void RecordAlternativePhaseSkip(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramTurnPhase> phases) => _resolve().RecordAlternativePhaseSkip(frame, phases);
    internal void ResolveAlternativePhaseCost(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveAlternativePhaseCost(choice);
    internal void ResolveDeclinedOwnedFieldMove(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDeclinedOwnedFieldMove(choice);
    internal bool ResumeAlternativePhaseCost(long frameId) => _resolve().ResumeAlternativePhaseCost(frameId);
    internal global::CardGame.Core.PromptChoice SelectAiAlternativePhaseCost(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiAlternativePhaseCost(decision, frame);
}
