#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelLegacyWindowsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelLegacyWindowsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelLegacyWindowsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> resolve) => new(resolve);
}
