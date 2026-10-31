#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime))]
internal sealed partial class CoreGameEnginePairedColorResponseObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> _resolve;
    private CoreGameEnginePairedColorResponseObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedColorResponseObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponseObserversRuntime> resolve) => new(resolve);
}
