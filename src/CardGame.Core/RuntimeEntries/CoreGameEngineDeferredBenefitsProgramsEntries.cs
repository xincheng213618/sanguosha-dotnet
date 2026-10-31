#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime))]
internal sealed partial class CoreGameEngineDeferredBenefitsProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> _resolve;
    private CoreGameEngineDeferredBenefitsProgramsEntries(Func<EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredBenefitsProgramsEntries Create(Func<EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> resolve) => new(resolve);
}
