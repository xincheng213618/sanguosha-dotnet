#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangHappyGodEntries
{
    internal void GrantHappyDawu(ProgramSkillFrame supplied) => _resolve().GrantHappyDawu(supplied);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyDawuNormalDamage(GameEngine.IDamageAttempt attack) => _resolve().HappyDawuNormalDamage(attack);
}
