#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianEntries
{
    internal bool CanContinueSimultaneousPindian(ProgramSkillFrame f) => _resolve().CanContinueSimultaneousPindian(f);
    internal void CleanupSimultaneousPindianCards(ProgramSkillFrame f) => _resolve().CleanupSimultaneousPindianCards(f);
    internal bool DrainSimultaneousChildren(ProgramSkillFrame f) => _resolve().DrainSimultaneousChildren(f);
    internal void FinishSimultaneousPindian(ProgramSkillFrame f) => _resolve().FinishSimultaneousPindian(f);
    internal bool IsSimultaneousReceiptSource(ProgramSkillFrame f) => _resolve().IsSimultaneousReceiptSource(f);
    internal void PublishSimultaneousChoice(ProgramSkillFrame f) => _resolve().PublishSimultaneousChoice(f);
    internal void ResolveSimultaneousPindianChoice(PromptChoice choice) => _resolve().ResolveSimultaneousPindianChoice(choice);
    internal bool ResumeSimultaneousPindian(long id) => _resolve().ResumeSimultaneousPindian(id);
    internal bool ReturnSimultaneousMovement(ProgramSkillFrame f) => _resolve().ReturnSimultaneousMovement(f);
    internal PromptChoice SelectAiSimultaneousChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiSimultaneousChoice(p, f);
    internal IReadOnlyList<PromptChoice> SimultaneousChoices(ProgramSkillFrame f) => _resolve().SimultaneousChoices(f);
    internal int SimultaneousChooser(ProgramSkillFrame f) => _resolve().SimultaneousChooser(f);
    internal PindianResult SimultaneousResult(ProgramSkillFrame f) => _resolve().SimultaneousResult(f);
    internal CardConversionSource SimultaneousSource(ProgramSkillFrame f) => _resolve().SimultaneousSource(f);
    internal SkillProgramStepOutcome StartSimultaneousPindian(ProgramSkillFrame supplied) => _resolve().StartSimultaneousPindian(supplied);
}
