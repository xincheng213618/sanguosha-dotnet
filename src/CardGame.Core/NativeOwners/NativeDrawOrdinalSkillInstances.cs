#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawOrdinalSkillInstances(Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime) : INativeDrawOrdinalSkillInstances
{
    public bool HasRuntimeSkillInstance(CharacterState owner, string skillId, string skillInstanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(owner, skillId, skillInstanceId);
}
