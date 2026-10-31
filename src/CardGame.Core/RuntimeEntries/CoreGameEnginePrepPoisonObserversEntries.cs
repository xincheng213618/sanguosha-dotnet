#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonObserversRuntime))]
internal sealed partial class CoreGameEnginePrepPoisonObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> _resolve;
    private CoreGameEnginePrepPoisonObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepPoisonObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepPoisonObserversRuntime> resolve) => new(resolve);
}
