#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWholeActualTurnUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> _resolve;
    private CoreGameEngineWholeActualTurnUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWholeActualTurnUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWholeActualTurnUsesRuntime> resolve) => new(resolve);
}
