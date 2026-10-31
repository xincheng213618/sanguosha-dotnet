#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFireTargetBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> _resolve;
    private CoreGameEngineFireTargetBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFireTargetBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> resolve) => new(resolve);
}
