#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> _resolve;
    private CoreGameEngineFireTargetBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFireTargetBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitsRuntime> resolve) => new(resolve);
}
