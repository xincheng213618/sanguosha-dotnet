#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardBudgetAndFactionGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> _resolve;
    private CoreGameEngineDiscardBudgetAndFactionGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardBudgetAndFactionGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardBudgetAndFactionGiftsRuntime> resolve) => new(resolve);
}
