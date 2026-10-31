#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNumericRuleAurasEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNumericRuleAurasRuntime> _resolve;
    private CoreGameEngineNumericRuleAurasEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNumericRuleAurasRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNumericRuleAurasEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNumericRuleAurasRuntime> resolve) => new(resolve);
}
