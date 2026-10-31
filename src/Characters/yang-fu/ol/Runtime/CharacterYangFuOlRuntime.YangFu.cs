#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    YangFuPlayer IYangFuFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].IsChained);
    IReadOnlyList<YangFuPlayer> IYangFuFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => ((IYangFuFacts)this).Player(player.Seat)).ToArray());

    bool IYangFuFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    int IYangFuFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int IYangFuFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int IYangFuFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    TurnPhase IYangFuFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner IYangFuFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long IYangFuFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    IReadOnlyList<CardMovementRecord> IYangFuFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    IEnumerable<T> IYangFuFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    CardLocation IYangFuFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    CardUseFrame? IYangFuFacts.CardUse(long useId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId);
    ContentSkillDefinition IYangFuFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    SkillProgramEffect? IYangFuFacts.PausedEffect(ProgramSkillFrame frame) => _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame);
    bool IYangFuFacts.HasTriggerOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(operation);
    bool IYangFuFacts.ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    bool IYangFuFacts.Allies(int owner, int target) => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    bool IYangFuFacts.HasUseDebit(long actionId) => _coreCardUseDebitsOperations.GetCardUseDebit(actionId)is not null;
    bool IYangFuFacts.IsEquipment(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind);
    bool IYangFuOperations.BeginCharacterStateChildren(long frameId) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    bool IYangFuOperations.BeginHpChildren(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    bool IYangFuOperations.BeginMovementChildren(long frameId) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId);
    bool IYangFuOperations.BeginSkillChildren(long frameId) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frameId);
    SkillProgramStepOutcome IYangFuOperations.UseVirtualSlash(ProgramSkillFrame frame, int target) => _coreTriggeredCardProgramsOperations.UseTriggeredProgramVirtualSlash(frame, target);
    SkillProgramStepOutcome IYangFuOperations.UseDeckEquipment(ProgramSkillFrame frame, int target, string bind) => _coreDeckEquipmentProgramsOperations.UseProgramRandomDeckEquipment(frame, target, bind);
    void IYangFuOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    int IYangFuOperations.DrawCards(int recipient, int amount, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipient], amount, true, reason).Count;
    void IYangFuOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IYangFuOperations.UpdateRoleUse(CardUseFrame use, CardActionContext action) => _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use, action);
    void IYangFuOperations.SetNativeOrigin(long useId, YangFuNativeOrigin origin) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, use => use with { SuyiOrigin = origin });
    void IYangFuOperations.RecordSlashDebit(long useId, int owner) => _coreCardUseDebitsOperations.RecordSlashUseDebit(useId, owner);
    bool IYangFuOperations.AdvanceObserverSubtree() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
