#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed partial class CoreActualDiscardPhaseLedgerEntries
{
    private readonly Func<EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _resolve;
    private CoreActualDiscardPhaseLedgerEntries(Func<EngineOwners.CoreActualDiscardPhaseLedgerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualDiscardPhaseLedgerEntries Create(Func<EngineOwners.CoreActualDiscardPhaseLedgerRuntime> resolve) => new(resolve);
}
