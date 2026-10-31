#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime))]
internal sealed partial class CoreGameEnginePairBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime> _resolve;
    private CoreGameEnginePairBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairBenefitObserversRuntime> resolve) => new(resolve);
}
