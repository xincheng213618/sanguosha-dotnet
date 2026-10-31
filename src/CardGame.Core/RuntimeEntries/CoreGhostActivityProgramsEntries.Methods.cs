#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGhostActivityProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteGhostActivity(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteGhostActivity(input, effect);
    internal bool IsValidRedJudgmentFireSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, bool awaiting) => _resolve().IsValidRedJudgmentFireSelection(frame, effect, awaiting);
    internal void ResolveRedJudgmentFireTarget(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveRedJudgmentFireTarget(choice);
}
