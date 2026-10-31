#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed partial class CoreGameEngineAlternatingBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _resolve;
    private CoreGameEngineAlternatingBenefitsEntries(Func<EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlternatingBenefitsEntries Create(Func<EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> resolve) => new(resolve);
}
