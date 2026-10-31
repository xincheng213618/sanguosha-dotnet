#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramLostHpChainedPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> _resolve;
    private CoreProgramLostHpChainedPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramLostHpChainedPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLostHpChainedPaymentsRuntime> resolve) => new(resolve);
}
