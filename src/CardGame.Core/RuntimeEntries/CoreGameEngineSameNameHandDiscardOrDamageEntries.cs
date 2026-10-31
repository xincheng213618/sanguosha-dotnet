#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> _resolve;
    private CoreGameEngineSameNameHandDiscardOrDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameNameHandDiscardOrDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> resolve) => new(resolve);
}
