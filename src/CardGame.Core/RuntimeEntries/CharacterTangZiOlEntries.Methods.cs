#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangZiOlEntries
{
    internal bool MatchesTangZiWoundedGrant(CharacterState owner, SkillProgramHpSkillGrant rule) => _resolve().MatchesTangZiWoundedGrant(owner, rule);
}
