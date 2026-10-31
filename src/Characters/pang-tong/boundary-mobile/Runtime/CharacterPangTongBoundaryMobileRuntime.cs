#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CharacterPangTongBoundaryMobileRuntime
{
    private readonly IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    internal CharacterPangTongBoundaryMobileRuntime(IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations)
    {
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
    }
}
