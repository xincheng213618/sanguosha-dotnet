#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSkillOwnershipWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _resolve;
    private CoreGameEngineSkillOwnershipWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSkillOwnershipWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> resolve) => new(resolve);
}
