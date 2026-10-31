#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTriggerWindowsEntries
{
    internal NativeOwners.ProgramTriggerWindowHost? CurrentDrivenProgramTriggerWindow() => _resolve().CurrentDrivenProgramTriggerWindow();
    internal bool TryGetDrivenProgramTriggerCandidate(scoped out ProgramTriggerCandidate candidate, scoped out ProgramSkillWindowContext context) => _resolve().TryGetDrivenProgramTriggerCandidate(out candidate, out context);
    internal bool TryResolveDrivenProgramTriggerGroupChoice(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, PromptChoice selected, string? action) => _resolve().TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);
    internal bool TrySkipDrivenProgramCandidate(ProgramTriggerCandidate candidate) => _resolve().TrySkipDrivenProgramCandidate(candidate);
}
