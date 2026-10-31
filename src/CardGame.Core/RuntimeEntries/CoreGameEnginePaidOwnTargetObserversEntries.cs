#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidOwnTargetObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _resolve;
    private CoreGameEnginePaidOwnTargetObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidOwnTargetObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve) => new(resolve);
}
