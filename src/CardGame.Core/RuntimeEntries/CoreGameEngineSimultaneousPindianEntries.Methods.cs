#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianEntries
{
    internal void AssertSimultaneousPindian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSimultaneousPindian(f);
    internal bool CanContinueSimultaneousPindian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueSimultaneousPindian(f);
    internal void CleanupSimultaneousPindianCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CleanupSimultaneousPindianCards(f);
    internal bool DrainSimultaneousChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSimultaneousChildren(f);
    internal void FinishSimultaneousPindian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSimultaneousPindian(f);
    internal bool IsSimultaneousReceiptSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsSimultaneousReceiptSource(f);
    internal void PublishSimultaneousChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSimultaneousChoice(f);
    internal void ResolveSimultaneousPindianChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSimultaneousPindianChoice(choice);
    internal bool ResumeSimultaneousPindian(long id) => _resolve().ResumeSimultaneousPindian(id);
    internal bool ReturnSimultaneousMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSimultaneousMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiSimultaneousChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSimultaneousChoice(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SimultaneousChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimultaneousChoices(f);
    internal int SimultaneousChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimultaneousChooser(f);
    internal global::CardGame.Core.PindianResult SimultaneousResult(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimultaneousResult(f);
    internal global::CardGame.Core.CardConversionSource SimultaneousSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SimultaneousSource(f);
    internal global::CardGame.Core.SkillProgramStepOutcome StartSimultaneousPindian(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartSimultaneousPindian(supplied);
}
