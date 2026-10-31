#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePlayerViewCardPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewCardPaymentsRuntime> _resolve;
    private CorePlayerViewCardPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewCardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePlayerViewCardPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewCardPaymentsRuntime> resolve) => new(resolve);
}
