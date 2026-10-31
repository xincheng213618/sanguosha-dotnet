#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTurnTargetCardQuotaAllowancesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> _resolve;
    private CoreTurnTargetCardQuotaAllowancesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTurnTargetCardQuotaAllowancesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> resolve) => new(resolve);
}
