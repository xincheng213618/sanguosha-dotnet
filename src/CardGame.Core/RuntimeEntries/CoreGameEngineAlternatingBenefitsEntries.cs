#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternatingBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _resolve;
    private CoreGameEngineAlternatingBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternatingBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> resolve) => new(resolve);
}
