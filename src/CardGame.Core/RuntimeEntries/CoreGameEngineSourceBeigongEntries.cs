#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSourceBeigongRuntime))]
internal sealed partial class CoreGameEngineSourceBeigongEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSourceBeigongRuntime> _resolve;
    private CoreGameEngineSourceBeigongEntries(Func<EngineOwners.CoreGameEngineSourceBeigongRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceBeigongEntries Create(Func<EngineOwners.CoreGameEngineSourceBeigongRuntime> resolve) => new(resolve);
}
