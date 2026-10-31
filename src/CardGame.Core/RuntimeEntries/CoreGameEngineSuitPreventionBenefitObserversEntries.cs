#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSuitPreventionBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> _resolve;
    private CoreGameEngineSuitPreventionBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSuitPreventionBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> resolve) => new(resolve);
}
