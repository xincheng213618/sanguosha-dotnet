#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuLingOlEntries
{
    internal void ResolveZhanyiCategoryChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhanyiCategoryChoice(selected);
    internal void ResolveZhanyiPunishCardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhanyiPunishCardChoice(selected);
    internal void ResolveZhanyiPunishTargetChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhanyiPunishTargetChoice(selected);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> ZhanyiOwnedCategoryCards(int ownerSeat, global::CardGame.Core.SkillProgramCardCategory category) => _resolve().ZhanyiOwnedCategoryCards(ownerSeat, category);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhanyiProgramChooseCategory(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhanyiProgramChooseCategory(frame, effect);
    internal global::CardGame.Core.SkillProgramStepOutcome ZhanyiProgramEquipmentPunish(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZhanyiProgramEquipmentPunish(frame, effect);
}
