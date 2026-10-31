#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDingFengMobileEntries
{
    internal RuleQueryEvaluation ApplyMobileFenxunDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyMobileFenxunDistance(source, target, original);
    internal SkillProgramStepOutcome GrantMobileFenxun(ProgramSkillFrame supplied) => _resolve().GrantMobileFenxun(supplied);
}
