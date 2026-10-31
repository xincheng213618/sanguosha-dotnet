#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunLuBanBoundaryMobileEntries
{
    internal bool AllowsBoundarySunLuBanDrawTwoRoleTarget(CardUseFrame use) => _resolve().AllowsBoundarySunLuBanDrawTwoRoleTarget(use);
    internal void AnswerBoundarySunLuBanChoice(PromptChoice selected) => _resolve().AnswerBoundarySunLuBanChoice(selected);
    internal SkillProgramStepOutcome BeginBoundarySunLuBanChoice(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginBoundarySunLuBanChoice(supplied, effect);
    internal IReadOnlyList<PromptChoice> BoundarySunLuBanChoices(ProgramSkillFrame frame) => _resolve().BoundarySunLuBanChoices(frame);
    internal bool BoundarySunLuBanTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().BoundarySunLuBanTargetWindow(query);
}
