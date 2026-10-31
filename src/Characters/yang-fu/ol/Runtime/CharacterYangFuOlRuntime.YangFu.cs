#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFuOlRuntime
{
    YangFuPlayer global::CardGame.Core.IYangFuFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].Name, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].IsChained);
    IReadOnlyList<YangFuPlayer> global::CardGame.Core.IYangFuFacts.Players => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => ((global::CardGame.Core.IYangFuFacts)this).Player(player.Seat)).ToArray());

    bool global::CardGame.Core.IYangFuFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    int global::CardGame.Core.IYangFuFacts.HandCount(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count;
    int global::CardGame.Core.IYangFuFacts.TurnNumber => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int global::CardGame.Core.IYangFuFacts.CurrentSeat => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;

    TurnPhase global::CardGame.Core.IYangFuFacts.Phase => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase;

    Winner global::CardGame.Core.IYangFuFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long global::CardGame.Core.IYangFuFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IYangFuFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    IEnumerable<T> global::CardGame.Core.IYangFuFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    CardLocation global::CardGame.Core.IYangFuFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    CardUseFrame? global::CardGame.Core.IYangFuFacts.CardUse(long useId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId);
    ContentSkillDefinition global::CardGame.Core.IYangFuFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    SkillProgramEffect? global::CardGame.Core.IYangFuFacts.PausedEffect(ProgramSkillFrame frame) => _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame);
    bool global::CardGame.Core.IYangFuFacts.HasTriggerOperation(SkillProgramEffectOp operation) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(operation);
    bool global::CardGame.Core.IYangFuFacts.ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right) => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(left, right);
    bool global::CardGame.Core.IYangFuFacts.Allies(int owner, int target) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[target]);
    bool global::CardGame.Core.IYangFuFacts.HasUseDebit(long actionId) => _coreCardUseDebitsOperations.GetCardUseDebit(actionId)is not null;
    bool global::CardGame.Core.IYangFuFacts.IsEquipment(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind);
    bool global::CardGame.Core.IYangFuOperations.BeginCharacterStateChildren(long frameId) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program);
    bool global::CardGame.Core.IYangFuOperations.BeginHpChildren(long frameId) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.AwaitedProgramMovement);
    bool global::CardGame.Core.IYangFuOperations.BeginMovementChildren(long frameId) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId);
    bool global::CardGame.Core.IYangFuOperations.BeginSkillChildren(long frameId) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frameId);
    SkillProgramStepOutcome global::CardGame.Core.IYangFuOperations.UseVirtualSlash(ProgramSkillFrame frame, int target) => _coreTriggeredCardProgramsOperations.UseTriggeredProgramVirtualSlash(frame, target);
    SkillProgramStepOutcome global::CardGame.Core.IYangFuOperations.UseDeckEquipment(ProgramSkillFrame frame, int target, string bind) => _coreDeckEquipmentProgramsOperations.UseProgramRandomDeckEquipment(frame, target, bind);
    void global::CardGame.Core.IYangFuOperations.Finish(ProgramSkillFrame frame, bool completed) => _coreSkillProgramsOperations.FinishProgramSkill(frame, completed);
    int global::CardGame.Core.IYangFuOperations.DrawCards(int recipient, int amount, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipient], amount, true, reason).Count;
    void global::CardGame.Core.IYangFuOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IYangFuOperations.UpdateRoleUse(CardUseFrame use, CardActionContext action) => _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use, action);
    void global::CardGame.Core.IYangFuOperations.SetNativeOrigin(long useId, YangFuNativeOrigin origin) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, use => use with { SuyiOrigin = origin });
    void global::CardGame.Core.IYangFuOperations.RecordSlashDebit(long useId, int owner) => _coreCardUseDebitsOperations.RecordSlashUseDebit(useId, owner);
    bool global::CardGame.Core.IYangFuOperations.AdvanceObserverSubtree() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
