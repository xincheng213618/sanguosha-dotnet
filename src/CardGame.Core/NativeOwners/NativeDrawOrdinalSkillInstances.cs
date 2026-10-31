#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawOrdinalSkillInstances(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : INativeDrawOrdinalSkillInstances
{
    public bool HasRuntimeSkillInstance(CharacterState owner, string skillId, string skillInstanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(owner, skillId, skillInstanceId);
}
