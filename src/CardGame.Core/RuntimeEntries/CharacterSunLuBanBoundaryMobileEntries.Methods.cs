#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunLuBanBoundaryMobileEntries
{
    internal bool AllowsBoundarySunLuBanDrawTwoRoleTarget(global::CardGame.Core.CardUseFrame use) => _resolve().AllowsBoundarySunLuBanDrawTwoRoleTarget(use);
    internal void AnswerBoundarySunLuBanChoice(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerBoundarySunLuBanChoice(selected);
    internal void AssertBoundarySunLuBanChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertBoundarySunLuBanChoice(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundarySunLuBanChoice(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginBoundarySunLuBanChoice(supplied, effect);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundarySunLuBanChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BoundarySunLuBanChoices(frame);
    internal bool BoundarySunLuBanTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().BoundarySunLuBanTargetWindow(query);
    internal bool IsBoundarySunLuBanChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().IsBoundarySunLuBanChoiceResult(frame, binding);
}
