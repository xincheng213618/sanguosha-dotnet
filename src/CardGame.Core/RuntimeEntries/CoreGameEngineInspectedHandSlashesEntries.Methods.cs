#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashesEntries
{
    internal bool AllowsPayHpInspectHandThenDiscardOrSlashActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsPayHpInspectHandThenDiscardOrSlashActivation(gate);
    internal SkillProgramStepOutcome BeginInspectedHandSlash(ProgramSkillFrame supplied) => _resolve().BeginInspectedHandSlash(supplied);
    internal void FinishInspectedHandSlash(ProgramSkillFrame f, bool issued) => _resolve().FinishInspectedHandSlash(f, issued);
    internal IReadOnlyList<PromptChoice> InspectedHandChoices(ProgramSkillFrame f) => _resolve().InspectedHandChoices(f);
    internal Dictionary<string, string> InspectedHandParameters(ProgramSkillFrame f, string option) => _resolve().InspectedHandParameters(f, option);
    internal IEnumerable<CardSnapshot> InspectedHandPrivateCards(int viewer) => _resolve().InspectedHandPrivateCards(viewer);
    internal CardConversionSource InspectedHandSource(ProgramSkillFrame f) => _resolve().InspectedHandSource(f);
    internal void PublishInspectedHandChoice(ProgramSkillFrame f) => _resolve().PublishInspectedHandChoice(f);
    internal void ResolveInspectedHandChoice(PromptChoice choice) => _resolve().ResolveInspectedHandChoice(choice);
    internal bool ResumeInspectedHandSlash(long id) => _resolve().ResumeInspectedHandSlash(id);
    internal bool ReturnInspectedHandMovement(ProgramSkillFrame f) => _resolve().ReturnInspectedHandMovement(f);
    internal PromptChoice SelectAiInspectedHand(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiInspectedHand(decision, f);
}
