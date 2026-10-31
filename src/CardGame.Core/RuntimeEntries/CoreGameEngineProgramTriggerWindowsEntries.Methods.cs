#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTriggerWindowsEntries
{
    internal global::CardGame.Core.NativeOwners.ProgramTriggerWindowHost? CurrentDrivenProgramTriggerWindow() => _resolve().CurrentDrivenProgramTriggerWindow();
    internal bool TryGetDrivenProgramTriggerCandidate(scoped out global::CardGame.Core.ProgramTriggerCandidate candidate, scoped out global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().TryGetDrivenProgramTriggerCandidate(out candidate, out context);
    internal bool TryResolveDrivenProgramTriggerGroupChoice(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.PromptChoice selected, string? action) => _resolve().TryResolveDrivenProgramTriggerGroupChoice(candidate, context, selected, action);
    internal bool TrySkipDrivenProgramCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TrySkipDrivenProgramCandidate(candidate);
}
