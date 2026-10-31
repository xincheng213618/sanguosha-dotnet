#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKenshangPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> _resolve;
    private CoreGameEngineKenshangPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineKenshangPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineKenshangPaymentsRuntime> resolve) => new(resolve);
}
