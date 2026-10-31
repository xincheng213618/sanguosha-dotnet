#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime))]
internal sealed partial class CoreTurnTargetCardQuotaAllowancesEntries
{
    private readonly Func<EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> _resolve;
    private CoreTurnTargetCardQuotaAllowancesEntries(Func<EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTurnTargetCardQuotaAllowancesEntries Create(Func<EngineOwners.CoreTurnTargetCardQuotaAllowancesRuntime> resolve) => new(resolve);
}
