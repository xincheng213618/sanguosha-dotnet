#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineSuitPreventionBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> _resolve;
    private CoreGameEngineSuitPreventionBenefitObserversEntries(Func<EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSuitPreventionBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineSuitPreventionBenefitObserversRuntime> resolve) => new(resolve);
}
