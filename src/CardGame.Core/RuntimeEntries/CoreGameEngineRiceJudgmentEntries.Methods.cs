#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRiceJudgmentEntries
{
    internal SkillProgramStepOutcome BeginRiceJudgmentReplacement(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginRiceJudgmentReplacement(supplied, effect);
    internal IReadOnlyList<Card> RiceJudgmentReplacementCards(CharacterState owner, SkillProgramTrigger trigger) => _resolve().RiceJudgmentReplacementCards(owner, trigger);
}
