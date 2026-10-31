#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionUsageQueries(Func<RuntimeEntries.CoreGameEngineAppliedDamageBenefitsEntries> getCoreGameEngineAppliedDamageBenefitsRuntime, Func<RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime) : ISelectionUsageQueries
{
    public bool CanUseDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => getCoreGameEngineAppliedDamageBenefitsRuntime().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    public bool CanUsePhaseTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => getCoreProgramParticipantPhaseOperationsRuntime().CanActivateTargetPhaseLedger(ownerSeat, skillId, usageId, targetSeat);
    public bool CanUseCategoryTarget(int ownerSeat, string skillId, string usageId, int cardId, int targetSeat) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, [cardId], [targetSeat]);
}
