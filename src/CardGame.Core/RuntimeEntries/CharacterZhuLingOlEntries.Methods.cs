#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuLingOlEntries
{
    internal void ResolveZhanyiCategoryChoice(PromptChoice selected) => _resolve().ResolveZhanyiCategoryChoice(selected);
    internal void ResolveZhanyiPunishCardChoice(PromptChoice selected) => _resolve().ResolveZhanyiPunishCardChoice(selected);
    internal void ResolveZhanyiPunishTargetChoice(PromptChoice selected) => _resolve().ResolveZhanyiPunishTargetChoice(selected);
    internal IReadOnlyList<Card> ZhanyiOwnedCategoryCards(int ownerSeat, SkillProgramCardCategory category) => _resolve().ZhanyiOwnedCategoryCards(ownerSeat, category);
    internal SkillProgramStepOutcome ZhanyiProgramChooseCategory(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhanyiProgramChooseCategory(frame, effect);
    internal SkillProgramStepOutcome ZhanyiProgramEquipmentPunish(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZhanyiProgramEquipmentPunish(frame, effect);
}
