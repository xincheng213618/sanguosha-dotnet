#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime))]
internal sealed partial class CoreGameEngineConditionalDiscardDuelReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> _resolve;
    private CoreGameEngineConditionalDiscardDuelReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConditionalDiscardDuelReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConditionalDiscardDuelReturnsRuntime> resolve) => new(resolve);
}
