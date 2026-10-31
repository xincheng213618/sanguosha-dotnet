#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardEndingPromisesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _resolve;
    private CoreGameEnginePrepDiscardEndingPromisesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardEndingPromisesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> resolve) => new(resolve);
}
