#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreAlternativePhaseCostProgramsEntries
{
    internal SkillProgramStepOutcome BeginAlternativePhaseCost(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker) => _resolve().BeginAlternativePhaseCost(frame, zones, marker);
    internal IReadOnlyList<PromptChoice> BuildAlternativePhaseCostChoices(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BuildAlternativePhaseCostChoices(frame, effect);
    internal bool CanOfferAlternativePhaseCost(CharacterState owner, SkillProgramTrigger trigger, ProgramTriggerCandidate candidate) => _resolve().CanOfferAlternativePhaseCost(owner, trigger, candidate);
    internal void CompleteActualJudgmentStarting(ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteActualJudgmentStarting(frame);
    internal bool IsAlternativePhaseCostChoiceLegal(PromptChoice choice) => _resolve().IsAlternativePhaseCostChoiceLegal(choice);
    internal bool IsConfirmedActualStartingSubstitution(ProgramSkillWindowContext context) => _resolve().IsConfirmedActualStartingSubstitution(context);
    internal void RecordAlternativePhaseSkip(ProgramSkillFrame frame, IReadOnlyList<SkillProgramTurnPhase> phases) => _resolve().RecordAlternativePhaseSkip(frame, phases);
    internal void ResolveAlternativePhaseCost(PromptChoice choice) => _resolve().ResolveAlternativePhaseCost(choice);
    internal void ResolveDeclinedOwnedFieldMove(PromptChoice choice) => _resolve().ResolveDeclinedOwnedFieldMove(choice);
    internal bool ResumeAlternativePhaseCost(long frameId) => _resolve().ResumeAlternativePhaseCost(frameId);
    internal PromptChoice SelectAiAlternativePhaseCost(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiAlternativePhaseCost(decision, frame);
}
