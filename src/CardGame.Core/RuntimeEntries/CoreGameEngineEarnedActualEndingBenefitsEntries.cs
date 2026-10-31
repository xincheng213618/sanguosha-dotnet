#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEarnedActualEndingBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> _resolve;
    private CoreGameEngineEarnedActualEndingBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEarnedActualEndingBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> resolve) => new(resolve);
}
