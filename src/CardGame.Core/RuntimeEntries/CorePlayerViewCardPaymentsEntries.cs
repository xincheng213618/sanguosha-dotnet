#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePlayerViewCardPaymentsEntries
{
    private readonly Func<EngineOwners.CorePlayerViewCardPaymentsRuntime> _resolve;
    private CorePlayerViewCardPaymentsEntries(Func<EngineOwners.CorePlayerViewCardPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePlayerViewCardPaymentsEntries Create(Func<EngineOwners.CorePlayerViewCardPaymentsRuntime> resolve) => new(resolve);
}
