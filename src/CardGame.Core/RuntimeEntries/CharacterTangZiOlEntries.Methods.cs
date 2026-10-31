#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangZiOlEntries
{
    internal bool MatchesTangZiWoundedGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramHpSkillGrant rule) => _resolve().MatchesTangZiWoundedGrant(owner, rule);
}
