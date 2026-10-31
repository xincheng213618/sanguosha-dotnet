#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramHandExchangesRuntime))]
internal sealed partial class CoreGameEngineProgramHandExchangesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramHandExchangesRuntime> _resolve;
    private CoreGameEngineProgramHandExchangesEntries(Func<EngineOwners.CoreGameEngineProgramHandExchangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramHandExchangesEntries Create(Func<EngineOwners.CoreGameEngineProgramHandExchangesRuntime> resolve) => new(resolve);
}
