#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSkillContextQueriesEntries
{
    internal PlayerSkillContext CreateSkillContext(CharacterState player, bool includeHandLimit = false) => _resolve().CreateSkillContext(player, includeHandLimit);
    internal bool HasProgramSkill(CharacterState player, string skillId) => _resolve().HasProgramSkill(player, skillId);
}
