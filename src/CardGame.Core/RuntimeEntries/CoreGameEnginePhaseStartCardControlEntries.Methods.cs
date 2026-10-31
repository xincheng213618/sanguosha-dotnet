#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartCardControlEntries
{
    internal SkillProgramStepOutcome BeginPhaseStartCardControl(ProgramSkillFrame supplied, PhaseStartCardControlKind kind, string? grantedSkill) => _resolve().BeginPhaseStartCardControl(supplied, kind, grantedSkill);
    internal bool CanContinuePhaseStartCardControl(ProgramSkillFrame f) => _resolve().CanContinuePhaseStartCardControl(f);
    internal bool CanOfferPhaseStartCardControl(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferPhaseStartCardControl(candidate, trigger, context);
    internal void CompletePhaseStartCardControl(ProgramSkillFrame f) => _resolve().CompletePhaseStartCardControl(f);
    internal bool DrainPhaseStartCardControlChildren(ProgramSkillFrame f) => _resolve().DrainPhaseStartCardControlChildren(f);
    internal bool HasExactPhaseStartCardControl(ProgramSkillFrame f) => _resolve().HasExactPhaseStartCardControl(f);
    internal bool HasPhaseStartControlBoundary(int owner, ProgramSkillWindowContext? context, PhaseStartCardControlKind kind) => _resolve().HasPhaseStartControlBoundary(owner, context, kind);
    internal Card[] OtherPlayRestrictionCosts(int owner) => _resolve().OtherPlayRestrictionCosts(owner);
    internal IReadOnlyList<PromptChoice> PhaseStartCardControlChoices(ProgramSkillFrame f) => _resolve().PhaseStartCardControlChoices(f);
    internal CardConversionSource PhaseStartControlSource(ProgramSkillFrame f) => _resolve().PhaseStartControlSource(f);
    internal void PublishPhaseStartCardControlChoice(ProgramSkillFrame f) => _resolve().PublishPhaseStartCardControlChoice(f);
    internal void ResolvePhaseStartCardControlChoice(PromptChoice selected) => _resolve().ResolvePhaseStartCardControlChoice(selected);
    internal bool ResumePhaseStartCardControl(long id) => _resolve().ResumePhaseStartCardControl(id);
    internal bool ReturnPhaseStartCardControlMovement(ProgramSkillFrame frame) => _resolve().ReturnPhaseStartCardControlMovement(frame);
    internal PromptChoice SelectAiPhaseStartCardControl(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiPhaseStartCardControl(decision, f);
    internal IEnumerable<(Card Card, int Seat, bool Obtain)> StartingEquipmentControlChoices(int owner) => _resolve().StartingEquipmentControlChoices(owner);
}
