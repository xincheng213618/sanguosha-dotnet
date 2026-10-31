#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashesEntries
{
    internal bool AllowsPayHpInspectHandThenDiscardOrSlashActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsPayHpInspectHandThenDiscardOrSlashActivation(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginInspectedHandSlash(supplied);
    internal void FinishInspectedHandSlash(global::CardGame.Core.ProgramSkillFrame f, bool issued) => _resolve().FinishInspectedHandSlash(f, issued);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> InspectedHandChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().InspectedHandChoices(f);
    internal global::System.Collections.Generic.Dictionary<string, string> InspectedHandParameters(global::CardGame.Core.ProgramSkillFrame f, string option) => _resolve().InspectedHandParameters(f, option);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> InspectedHandPrivateCards(int viewer) => _resolve().InspectedHandPrivateCards(viewer);
    internal global::CardGame.Core.CardConversionSource InspectedHandSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().InspectedHandSource(f);
    internal void PublishInspectedHandChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishInspectedHandChoice(f);
    internal void ResolveInspectedHandChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveInspectedHandChoice(choice);
    internal bool ResumeInspectedHandSlash(long id) => _resolve().ResumeInspectedHandSlash(id);
    internal bool ReturnInspectedHandMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnInspectedHandMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiInspectedHand(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiInspectedHand(decision, f);
}
