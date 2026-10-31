#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramHealthChildQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDyingCursorRuntimeRuntime>> getCoreDyingCursorRuntimeRuntime, EngineStates.CoreGameEngineHpChangeProgramsState gameEngineHpChangeProgramsState, Func<RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries> getCoreGameEngineProgramRecoveryReceiptsRuntime, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IProgramHealthChildQueries
{
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().Value.ActiveDying;

    public HpChangeContext? LastPendingLoss(long parentFrameId, int targetSeat) => gameEngineHpChangeProgramsState.ReferenceToPendingHpChanges.LastOrDefault(c => c.ParentFrameId == parentFrameId && c.TargetSeat == targetSeat && c.Kind == HpChangeKind.Loss);
    public bool ProgramRecoveryChild(ProgramSkillFrame parent, ResolutionFrame child) => child switch
    {
        RecoveryReplacementFrame recovery => getCoreGameEngineProgramRecoveryReceiptsRuntime().ProgramRecoveryReplacementReceiptMatches(parent, recovery, getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().ToArray()),
        HpChangedTriggerWindowFrame hp => getCoreGameEngineProgramRecoveryReceiptsRuntime().ProgramRecoveryReceiptMatches(parent, hp, getCoreGameEngineAlternatingBenefitsRuntime().CompleteProgramEventHistory().ToArray()),
        _ => false
    };
    public bool PaidObserverSuffix(long programFrameId)
    {
        var index = getFrameStore().FindIndex(f => f.Id == programFrameId);
        if (index < 0 || index + 1 >= getFrameStore().Count)
            return false;
        if (getFrameStore()[index + 1] is DyingFrame dying && index + 2 < getFrameStore().Count && getCoreGameEngineObserverSubtreesRuntime().IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying))
            return true;
        return getCoreGameEngineObserverSubtreesRuntime().HasPaidDamageObserverSuffix(index);
    }
}
