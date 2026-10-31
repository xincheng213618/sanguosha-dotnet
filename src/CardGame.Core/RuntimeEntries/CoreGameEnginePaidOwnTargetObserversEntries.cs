#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime))]
internal sealed partial class CoreGameEnginePaidOwnTargetObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> _resolve;
    private CoreGameEnginePaidOwnTargetObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidOwnTargetObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidOwnTargetObserversRuntime> resolve) => new(resolve);
}
