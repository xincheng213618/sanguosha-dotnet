#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUsePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _resolve;
    private CoreGameEngineCardUsePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUsePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> resolve) => new(resolve);
}
