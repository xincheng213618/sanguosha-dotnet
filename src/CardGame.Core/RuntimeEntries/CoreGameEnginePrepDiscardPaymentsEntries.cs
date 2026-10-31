#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> _resolve;
    private CoreGameEnginePrepDiscardPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrepDiscardPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrepDiscardPaymentsRuntime> resolve) => new(resolve);
}
