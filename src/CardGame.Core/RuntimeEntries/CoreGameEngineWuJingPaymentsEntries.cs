#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWuJingPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineWuJingPaymentsRuntime> _resolve;
    private CoreGameEngineWuJingPaymentsEntries(Func<EngineOwners.CoreGameEngineWuJingPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWuJingPaymentsEntries Create(Func<EngineOwners.CoreGameEngineWuJingPaymentsRuntime> resolve) => new(resolve);
}
