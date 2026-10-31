#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGhostActivityProgramsEntries
{
    internal SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().ExecuteGhostActivity(input, effect);
    internal void ResolveRedJudgmentFireTarget(PromptChoice choice) => _resolve().ResolveRedJudgmentFireTarget(choice);
}
