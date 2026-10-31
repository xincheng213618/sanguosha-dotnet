#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnShownCardBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> _resolve;
    private CoreGameEngineCurrentTurnShownCardBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnShownCardBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> resolve) => new(resolve);
}
