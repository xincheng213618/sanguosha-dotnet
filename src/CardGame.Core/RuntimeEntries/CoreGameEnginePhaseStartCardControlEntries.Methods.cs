#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartCardControlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPhaseStartCardControl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.PhaseStartCardControlKind kind, string? grantedSkill) => _resolve().BeginPhaseStartCardControl(supplied, kind, grantedSkill);
    internal bool CanContinuePhaseStartCardControl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePhaseStartCardControl(f);
    internal bool CanOfferPhaseStartCardControl(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferPhaseStartCardControl(candidate, trigger, context);
    internal void CompletePhaseStartCardControl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletePhaseStartCardControl(f);
    internal bool DrainPhaseStartCardControlChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainPhaseStartCardControlChildren(f);
    internal bool HasExactPhaseStartCardControl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasExactPhaseStartCardControl(f);
    internal bool HasPhaseStartControlBoundary(int owner, global::CardGame.Core.ProgramSkillWindowContext? context, global::CardGame.Core.PhaseStartCardControlKind kind) => _resolve().HasPhaseStartControlBoundary(owner, context, kind);
    internal global::CardGame.Core.Card[] OtherPlayRestrictionCosts(int owner) => _resolve().OtherPlayRestrictionCosts(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PhaseStartCardControlChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PhaseStartCardControlChoices(f);
    internal global::CardGame.Core.CardConversionSource PhaseStartControlSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PhaseStartControlSource(f);
    internal void PublishPhaseStartCardControlChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPhaseStartCardControlChoice(f);
    internal void ResolvePhaseStartCardControlChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolvePhaseStartCardControlChoice(selected);
    internal bool ResumePhaseStartCardControl(long id) => _resolve().ResumePhaseStartCardControl(id);
    internal bool ReturnPhaseStartCardControlMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPhaseStartCardControlMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiPhaseStartCardControl(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiPhaseStartCardControl(decision, f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, int Seat, bool Obtain)> StartingEquipmentControlChoices(int owner) => _resolve().StartingEquipmentControlChoices(owner);
}
