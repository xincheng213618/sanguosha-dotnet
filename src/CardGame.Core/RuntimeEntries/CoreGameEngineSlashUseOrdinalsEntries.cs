#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime))]
internal sealed partial class CoreGameEngineSlashUseOrdinalsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> _resolve;
    private CoreGameEngineSlashUseOrdinalsEntries(Func<EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashUseOrdinalsEntries Create(Func<EngineOwners.CoreGameEngineSlashUseOrdinalsRuntime> resolve) => new(resolve);
}
