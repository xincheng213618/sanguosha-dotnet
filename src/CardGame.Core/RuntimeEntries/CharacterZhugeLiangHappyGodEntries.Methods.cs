#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangHappyGodEntries
{
    internal void GrantHappyDawu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GrantHappyDawu(supplied);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> HappyDawuNormalDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HappyDawuNormalDamage(attack);
}
