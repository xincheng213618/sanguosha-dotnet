#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime))]
internal sealed partial class CoreGameEngineDiscardBudgetAndFactionGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> _resolve;
    private CoreGameEngineDiscardBudgetAndFactionGiftsEntries(Func<EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardBudgetAndFactionGiftsEntries Create(Func<EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> resolve) => new(resolve);
}
