#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeferredHandPairSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries> getCoreGameEngineHalfHandPhaseDebtProgramsRuntime) : IDeferredHandPairSelectionQuery
{
    public bool CanSelect(int ownerSeat, int first, int second) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().IsPayableDeferredHandPair(ownerSeat, first, second);
}
