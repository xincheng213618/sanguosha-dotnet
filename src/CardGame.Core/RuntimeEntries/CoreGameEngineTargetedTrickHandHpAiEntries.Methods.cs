#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetedTrickHandHpAiEntries
{
    internal double GetTargetedTrickHandHpPublicScore(int owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramEffect> effects, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().GetTargetedTrickHandHpPublicScore(owner, effects, context);
    internal double TargetedTrickPublicChoiceScore(int owner, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PromptChoice choice) => _resolve().TargetedTrickPublicChoiceScore(owner, use, choice);
    internal double UnequalHandHpPublicChoiceScore(int owner, int target, bool draw) => _resolve().UnequalHandHpPublicChoiceScore(owner, target, draw);
}
