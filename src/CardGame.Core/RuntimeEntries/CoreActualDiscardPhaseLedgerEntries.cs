#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed partial class CoreActualDiscardPhaseLedgerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _resolve;
    private CoreActualDiscardPhaseLedgerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualDiscardPhaseLedgerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> resolve) => new(resolve);
}
