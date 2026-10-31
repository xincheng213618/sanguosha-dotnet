#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiEntries
{
    internal double GetTargetedTrickHandHpPublicScore(int owner, IReadOnlyList<SkillProgramEffect> effects, ProgramSkillWindowContext? context) => _resolve().GetTargetedTrickHandHpPublicScore(owner, effects, context);
    internal double TargetedTrickPublicChoiceScore(int owner, CardUseFrame use, PromptChoice choice) => _resolve().TargetedTrickPublicChoiceScore(owner, use, choice);
    internal double UnequalHandHpPublicChoiceScore(int owner, int target, bool draw) => _resolve().UnequalHandHpPublicChoiceScore(owner, target, draw);
}
