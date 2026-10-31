#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed partial class CoreGameEngineEndingHistoricalUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _resolve;
    private CoreGameEngineEndingHistoricalUsesEntries(Func<EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingHistoricalUsesEntries Create(Func<EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> resolve) => new(resolve);
}
