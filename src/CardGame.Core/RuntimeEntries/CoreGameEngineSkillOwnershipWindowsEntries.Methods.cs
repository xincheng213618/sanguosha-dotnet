#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillOwnershipWindowsEntries
{
    internal bool TryBeginAdvancedSkillsChanged(long parentFrameId) => _resolve().TryBeginAdvancedSkillsChanged(parentFrameId);
}
