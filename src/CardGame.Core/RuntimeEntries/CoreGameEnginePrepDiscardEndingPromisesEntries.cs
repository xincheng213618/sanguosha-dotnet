#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardEndingPromisesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> _resolve;
    private CoreGameEnginePrepDiscardEndingPromisesEntries(Func<EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardEndingPromisesEntries Create(Func<EngineOwners.CoreGameEnginePrepDiscardEndingPromisesRuntime> resolve) => new(resolve);
}
