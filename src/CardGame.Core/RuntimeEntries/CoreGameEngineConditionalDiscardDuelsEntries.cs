#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelsRuntime> resolve) => new(resolve);
}
