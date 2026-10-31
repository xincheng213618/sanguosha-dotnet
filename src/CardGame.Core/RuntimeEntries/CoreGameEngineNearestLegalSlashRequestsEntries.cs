#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime))]
internal sealed partial class CoreGameEngineNearestLegalSlashRequestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> _resolve;
    private CoreGameEngineNearestLegalSlashRequestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNearestLegalSlashRequestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNearestLegalSlashRequestsRuntime> resolve) => new(resolve);
}
