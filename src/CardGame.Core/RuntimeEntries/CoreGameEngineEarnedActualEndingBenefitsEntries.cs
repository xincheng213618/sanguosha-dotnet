#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime))]
internal sealed partial class CoreGameEngineEarnedActualEndingBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> _resolve;
    private CoreGameEngineEarnedActualEndingBenefitsEntries(Func<EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEarnedActualEndingBenefitsEntries Create(Func<EngineOwners.CoreGameEngineEarnedActualEndingBenefitsRuntime> resolve) => new(resolve);
}
