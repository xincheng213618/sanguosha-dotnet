#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime))]
internal sealed partial class CoreGameEngineEndingHistoricalUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> _resolve;
    private CoreGameEngineEndingHistoricalUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndingHistoricalUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndingHistoricalUsesRuntime> resolve) => new(resolve);
}
