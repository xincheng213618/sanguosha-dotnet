#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed partial class CoreGameEngineSkillOwnershipWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _resolve;
    private CoreGameEngineSkillOwnershipWindowsEntries(Func<EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillOwnershipWindowsEntries Create(Func<EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> resolve) => new(resolve);
}
