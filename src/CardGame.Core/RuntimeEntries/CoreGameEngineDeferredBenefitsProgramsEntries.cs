#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime))]
internal sealed partial class CoreGameEngineDeferredBenefitsProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> _resolve;
    private CoreGameEngineDeferredBenefitsProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeferredBenefitsProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeferredBenefitsProgramsRuntime> resolve) => new(resolve);
}
