#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaiShiCiMobileShiEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().ShiTaiShiCiPublicStates(owner, skillId);
    internal int ShiTzZhanlieDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShiTzZhanlieDamageBonus(attack);
}
