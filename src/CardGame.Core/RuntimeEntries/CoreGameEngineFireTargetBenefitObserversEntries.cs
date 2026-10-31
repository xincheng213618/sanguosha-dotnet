#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineFireTargetBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> _resolve;
    private CoreGameEngineFireTargetBenefitObserversEntries(Func<EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFireTargetBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineFireTargetBenefitObserversRuntime> resolve) => new(resolve);
}
