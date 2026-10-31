#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSkillContextQueriesEntries
{
    internal global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false) => _resolve().CreateSkillContext(player, includeHandLimit);
    internal bool HasProgramSkill(global::CardGame.Core.CharacterState player, string skillId) => _resolve().HasProgramSkill(player, skillId);
}
