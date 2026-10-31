#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineSkillOwnershipWindowsRuntime
{
    private readonly ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations _skillOwnershipWindowOperations;
    internal CoreGameEngineSkillOwnershipWindowsRuntime(ICoreSkillOwnershipWindowsSkillOwnershipWindowOperations skillOwnershipWindowOperations)
    {
        this._skillOwnershipWindowOperations = skillOwnershipWindowOperations;
    }
}
