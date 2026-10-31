#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionUsageQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAppliedDamageBenefitsEntries> getCoreGameEngineAppliedDamageBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries> getCoreProgramParticipantPhaseOperationsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime) : ISelectionUsageQueries
{
    public bool CanUseDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => getCoreGameEngineAppliedDamageBenefitsRuntime().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    public bool CanUsePhaseTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => getCoreProgramParticipantPhaseOperationsRuntime().CanActivateTargetPhaseLedger(ownerSeat, skillId, usageId, targetSeat);
    public bool CanUseCategoryTarget(int ownerSeat, string skillId, string usageId, int cardId, int targetSeat) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().CanActivateCategoryTargetLedger(ownerSeat, skillId, usageId, [cardId], [targetSeat]);
}
