#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversRuntime
{
    internal bool OwnTrickDrawLegacyAlcoholRide(ProgramSkillFrame root, ActualUseTargetWindowFrame parent, int dyingIndex, DyingFrame dying)
    {
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
        if (rootIndex < 1 || rootIndex + 1 >= dyingIndex || _resolutionStackOperations._resolutionStack[rootIndex - 1] is not ActualUseTargetWindowFrame owningWindow || owningWindow.Id != parent.Id || !_coreOwnTrickDrawTargetsOperations.OwnTrickDrawMatches(root, parent) || !_coreOwnTrickDrawTargetsOperations.OwnTrickDrawFirstChild(root, _resolutionStackOperations._resolutionStack[rootIndex + 1]) || !_coreLegacyDyingAlcoholReturnsOperations.Value.ExactLegacyDyingAlcoholReturnRide(dyingIndex, dying))
            return false;
        for (var index = rootIndex + 1; index <= dyingIndex; index++)
            if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index))
                return false;
        return true;
    }
}
