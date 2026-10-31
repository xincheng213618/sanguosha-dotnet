#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameActivationProgressEntries
{
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot>? GetSkillPublicRuleStates(CharacterState owner, string skillId) => _resolve().GetSkillPublicRuleStates(owner, skillId);
}
