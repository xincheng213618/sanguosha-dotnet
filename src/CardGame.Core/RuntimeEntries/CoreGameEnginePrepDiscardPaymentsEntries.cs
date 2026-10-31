#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime))]
internal sealed partial class CoreGameEnginePrepDiscardPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> _resolve;
    private CoreGameEnginePrepDiscardPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve) => new(resolve);
}
