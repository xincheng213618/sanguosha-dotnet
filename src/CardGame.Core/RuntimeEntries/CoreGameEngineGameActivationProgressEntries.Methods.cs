#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameActivationProgressEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot>? GetSkillPublicRuleStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().GetSkillPublicRuleStates(owner, skillId);
}
