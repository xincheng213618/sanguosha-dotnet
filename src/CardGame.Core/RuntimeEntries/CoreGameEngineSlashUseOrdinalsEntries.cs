#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed partial class CoreGameEngineSlashUseOrdinalsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _resolve;
    private CoreGameEngineSlashUseOrdinalsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashUseOrdinalsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> resolve) => new(resolve);
}
