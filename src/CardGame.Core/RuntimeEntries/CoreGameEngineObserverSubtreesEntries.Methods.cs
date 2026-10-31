#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineObserverSubtreesEntries
{
    internal ProgramSkillFrame? FindProgramObserverRoot(Func<ProgramSkillFrame, ResolutionFrame, bool> firstChild, Func<int, bool> suffix, long? requiredRootId = default) => _resolve().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    internal bool HasHandGainDebtObserverSuffix(int rootIndex) => _resolve().HasHandGainDebtObserverSuffix(rootIndex);
    internal bool HasMovementObserverSuffix(int rootIndex) => _resolve().HasMovementObserverSuffix(rootIndex);
    internal bool HasObserverSuffix(int rootIndex, Func<int, bool> edge, Func<int, DyingFrame, bool>? completedDyingSuffix) => _resolve().HasObserverSuffix(rootIndex, edge, completedDyingSuffix);
    internal bool HasPaidDamageObserverSuffix(int rootIndex) => _resolve().HasPaidDamageObserverSuffix(rootIndex);
    internal bool IsMovementObserverEdge(int child) => _resolve().IsMovementObserverEdge(child);
    internal bool IsPaidRescueObserverSuffix(int child, DyingFrame dying) => _resolve().IsPaidRescueObserverSuffix(child, dying);
    internal bool IsSuspendedOrPaidRescueObserverSuffix(int child, DyingFrame dying) => _resolve().IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    internal bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => _resolve().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}
