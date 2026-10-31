#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime))]
internal sealed partial class CoreGameEngineCurrentRoundActualUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> _resolve;
    private CoreGameEngineCurrentRoundActualUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentRoundActualUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> resolve) => new(resolve);
}
