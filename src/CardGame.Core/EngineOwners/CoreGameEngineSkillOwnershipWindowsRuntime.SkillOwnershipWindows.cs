#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSkillOwnershipWindowsRuntime
{
    internal bool TryBeginAdvancedSkillsChanged(long parentFrameId) => _skillOwnershipWindowOperations.Get().Begin(parentFrameId);
}
