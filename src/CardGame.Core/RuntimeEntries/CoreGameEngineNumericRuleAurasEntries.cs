#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNumericRuleAurasEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNumericRuleAurasRuntime> _resolve;
    private CoreGameEngineNumericRuleAurasEntries(Func<EngineOwners.CoreGameEngineNumericRuleAurasRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNumericRuleAurasEntries Create(Func<EngineOwners.CoreGameEngineNumericRuleAurasRuntime> resolve) => new(resolve);
}
