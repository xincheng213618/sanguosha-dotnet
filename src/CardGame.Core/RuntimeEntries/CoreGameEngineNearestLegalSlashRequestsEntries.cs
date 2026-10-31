#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime))]
internal sealed partial class CoreGameEngineNearestLegalSlashRequestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> _resolve;
    private CoreGameEngineNearestLegalSlashRequestsEntries(Func<EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNearestLegalSlashRequestsEntries Create(Func<EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> resolve) => new(resolve);
}
