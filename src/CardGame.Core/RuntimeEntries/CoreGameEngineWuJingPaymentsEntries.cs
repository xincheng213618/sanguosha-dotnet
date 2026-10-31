#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingPaymentsRuntime> _resolve;
    private CoreGameEngineWuJingPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWuJingPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWuJingPaymentsRuntime> resolve) => new(resolve);
}
