#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineResponseEntityExchangeRuntime))]
internal sealed partial class CoreGameEngineResponseEntityExchangeEntries
{
    private readonly Func<EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> _resolve;
    private CoreGameEngineResponseEntityExchangeEntries(Func<EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineResponseEntityExchangeEntries Create(Func<EngineOwners.CoreGameEngineResponseEntityExchangeRuntime> resolve) => new(resolve);
}
