#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime))]
internal sealed partial class CoreGameEngineWholeActualTurnUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _resolve;
    private CoreGameEngineWholeActualTurnUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWholeActualTurnUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve) => new(resolve);
}
