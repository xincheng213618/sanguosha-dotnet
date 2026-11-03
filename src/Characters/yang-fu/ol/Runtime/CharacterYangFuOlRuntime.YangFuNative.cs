#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    internal void BindSuyiEquipmentUse(CardUseFrame use) => _yangFuOperations.Get().BindSuyiEquipmentUse(use);
    internal void IssueSuyiSlashUse(long id, ProgramSkillFrame supplied, CardActionContext action) => _yangFuOperations.Get().IssueSuyiSlashUse(id, supplied, action);
    internal bool IsSuyiSlashUse(long id) => _yangFuOperations.Get().IsSuyiSlashUse(id);
    internal bool IsSuyiEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => _yangFuOperations.Get().IsSuyiEquipmentRide(ride, beneath);
    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27310)]
    internal bool ReturnSuyiAttack(GameEngine.AttackCompletionReceipt completion) => _yangFuOperations.Get().ReturnSuyiAttack(new(completion.ResolutionId, completion.ProgramFrameId));
    bool IYangFuAdjacencyFacts.IgnoresSlashLimit(int owner, CardConversionSource source) => _coreCardConversionsOperations.IgnoresProgramSlashLimit(_coreMatchParticipantsPlayersAccess._players[owner], source);
    void IYangFuAdjacencyFacts.AssertCurrentSlashFirePolicy(CardUseFrame use) => _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
    DyingFrame? IYangFuAdjacencyFacts.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    bool IYangFuAdjacencyFacts.MovementObserverEdge(long childId) => _coreObserverSubtreesOperations.IsMovementObserverEdge(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == childId));
    bool IYangFuAdjacencyFacts.DyingExitEdge(long childId) => _characterYanPuOlOperations.IsYanPuDyingExitEdge(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == childId));
    bool IYangFuAdjacencyFacts.RandomEquipmentRide(ResolutionFrame child, ResolutionFrame parent) => _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent);
    bool IYangFuAdjacencyFacts.RecoveryReplacementRide(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool IYangFuAdjacencyFacts.SuspendedOrPaidRescueSuffix(DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(_resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id), dying);
}
