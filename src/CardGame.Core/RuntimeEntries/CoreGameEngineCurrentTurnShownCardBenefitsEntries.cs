#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime))]
internal sealed partial class CoreGameEngineCurrentTurnShownCardBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> _resolve;
    private CoreGameEngineCurrentTurnShownCardBenefitsEntries(Func<EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnShownCardBenefitsEntries Create(Func<EngineOwners.CoreGameEngineCurrentTurnShownCardBenefitsRuntime> resolve) => new(resolve);
}
