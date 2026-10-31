#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePairBenefitObserversRuntime))]
internal sealed partial class CoreGameEnginePairBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePairBenefitObserversRuntime> _resolve;
    private CoreGameEnginePairBenefitObserversEntries(Func<EngineOwners.CoreGameEnginePairBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairBenefitObserversEntries Create(Func<EngineOwners.CoreGameEnginePairBenefitObserversRuntime> resolve) => new(resolve);
}
