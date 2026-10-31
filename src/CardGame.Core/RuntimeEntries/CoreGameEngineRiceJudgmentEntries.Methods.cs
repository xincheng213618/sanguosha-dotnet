#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRiceJudgmentEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRiceJudgmentReplacement(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginRiceJudgmentReplacement(supplied, effect);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> RiceJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().RiceJudgmentReplacementCards(owner, trigger);
}
