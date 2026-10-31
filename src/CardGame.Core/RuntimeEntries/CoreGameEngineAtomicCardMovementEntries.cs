#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAtomicCardMovementEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime> _resolve;
    private CoreGameEngineAtomicCardMovementEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAtomicCardMovementEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAtomicCardMovementRuntime> resolve) => new(resolve);
}
