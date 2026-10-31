#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DeferredHandPairSelectionQuery(Func<RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries> getCoreGameEngineHalfHandPhaseDebtProgramsRuntime) : IDeferredHandPairSelectionQuery
{
    public bool CanSelect(int ownerSeat, int first, int second) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().IsPayableDeferredHandPair(ownerSeat, first, second);
}
