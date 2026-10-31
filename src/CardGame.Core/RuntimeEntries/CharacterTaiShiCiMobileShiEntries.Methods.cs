#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaiShiCiMobileShiEntries
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> ShiTaiShiCiPublicStates(CharacterState owner, string skillId) => _resolve().ShiTaiShiCiPublicStates(owner, skillId);
    internal int ShiTzZhanlieDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().ShiTzZhanlieDamageBonus(attack);
}
