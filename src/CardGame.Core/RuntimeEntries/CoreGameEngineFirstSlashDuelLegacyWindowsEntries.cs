#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime))]
internal sealed partial class CoreGameEngineFirstSlashDuelLegacyWindowsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelLegacyWindowsEntries(Func<EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelLegacyWindowsEntries Create(Func<EngineOwners.CoreGameEngineFirstSlashDuelLegacyWindowsRuntime> resolve) => new(resolve);
}
