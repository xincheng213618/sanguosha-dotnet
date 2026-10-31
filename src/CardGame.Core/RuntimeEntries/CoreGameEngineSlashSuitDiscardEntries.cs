#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashSuitDiscardRuntime))]
internal sealed partial class CoreGameEngineSlashSuitDiscardEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> _resolve;
    private CoreGameEngineSlashSuitDiscardEntries(Func<EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashSuitDiscardEntries Create(Func<EngineOwners.CoreGameEngineSlashSuitDiscardRuntime> resolve) => new(resolve);
}
