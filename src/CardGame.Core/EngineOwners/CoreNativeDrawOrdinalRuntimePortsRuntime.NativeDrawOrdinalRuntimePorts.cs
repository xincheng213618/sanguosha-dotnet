#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDrawOrdinalRuntimePortsRuntime
{
    bool INativeDrawOrdinalSkillInstances.HasRuntimeSkillInstance(CharacterState owner, string skillId, string skillInstanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, skillId, skillInstanceId);
}
