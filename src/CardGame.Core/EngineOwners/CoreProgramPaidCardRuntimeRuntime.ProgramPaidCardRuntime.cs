#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPaidCardRuntimeRuntime
{
    int global::CardGame.Core.IProgramPaidCardFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    IReadOnlyList<int> global::CardGame.Core.IProgramPaidCardFacts.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => player.Seat).ToArray());

    int global::CardGame.Core.IProgramPaidCardFacts.Seat(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Seat;
    bool global::CardGame.Core.IProgramPaidCardFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IProgramPaidCardFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IProgramPaidCardFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IProgramPaidCardFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool global::CardGame.Core.IProgramPaidCardFacts.EquipmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentAreaAbolished;
    bool global::CardGame.Core.IProgramPaidCardFacts.JudgmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished;
    int global::CardGame.Core.IProgramPaidCardFacts.EquipmentSlotCapacity(int seat, EquipmentSlot slot) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot);
    EquipmentSlot global::CardGame.Core.IProgramPaidCardFacts.FieldEquipmentSlot(Card card) => _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
    bool global::CardGame.Core.IProgramPaidCardFacts.HasSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    ContentSkillDefinition global::CardGame.Core.IProgramPaidCardFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry!.GetSkill(skillId);
    PlayerSkillContext global::CardGame.Core.IProgramPaidCardFacts.SkillContext(int seat) => _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> global::CardGame.Core.IProgramPaidCardFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> global::CardGame.Core.IProgramPaidCardFacts.Equipment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> global::CardGame.Core.IProgramPaidCardFacts.Judgment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> global::CardGame.Core.IProgramPaidCardFacts.CardsAt(CardLocation location) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(location).ToArray());
    CardLocation global::CardGame.Core.IProgramPaidCardFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    int global::CardGame.Core.IProgramPaidCardFacts.ProviderSeat(ProgramSkillFrame frame) => _characterGuanYuRuntimeOperations.GuanParentAction(frame).ProviderSeat;
    SkillProgramEffect global::CardGame.Core.IProgramPaidCardFacts.OwnedSelectionEffect(ProgramSkillFrame frame) => _coreProgramOwnedCardsOperations.GetOwnedSelectionEffect(frame);
    bool global::CardGame.Core.IProgramPaidCardFacts.IsSatisfiedOwnHandReward(ProgramSkillFrame frame, SkillProgramEffect effect) => _characterLeiBoAnniversaryOperations.IsAnniversaryLeiBoSelfLoot(frame, effect);
    bool global::CardGame.Core.IProgramPaidCardFacts.ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
    bool global::CardGame.Core.IProgramPaidCardFacts.SelfDiscardForbidden(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, card, source, intent);
    bool global::CardGame.Core.IProgramPaidCardFacts.AllowsOwnedCardPayment(ProgramSkillFrame frame, CardLocation source, Card card) => _characterDaQiaoMobileOperations.AllowsPostCostSlashRedirectPayment(frame, source, card);
    bool global::CardGame.Core.IProgramPaidCardFacts.IsActiveSourceEquipment(int owner, string skillId, string instanceId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    bool global::CardGame.Core.IProgramPaidCardFacts.IsZixiJudgment(int cardId) => _characterDaQiaoLeOperations.IsZixiQinJudgmentCard(cardId);
    bool global::CardGame.Core.IProgramPaidCardFacts.CanTransferZixiJudgment(Card card, int destinationSeat) => _characterDaQiaoLeOperations.CanTransferZixiQinJudgmentCard(card, destinationSeat);
    bool global::CardGame.Core.IProgramPaidCardFacts.HasCardPolicy(int seat, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], policy);
    CardKind global::CardGame.Core.IProgramPaidCardFacts.JudgmentKind(Card card) => _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card);
    bool global::CardGame.Core.IProgramPaidCardFacts.IsDelayedCard(CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(kind);
    bool global::CardGame.Core.IProgramPaidCardFacts.HasJudgmentEffectiveCard(int seat, CardKind kind) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], kind);
    Suit global::CardGame.Core.IProgramPaidCardFacts.EffectiveSuit(int seat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card);
    int global::CardGame.Core.IProgramPaidCardFacts.CountLivingInAttackRange(int seat) => _coreProgramCardPaymentsOperations.CountLivingInAttackRange(seat);
    IReadOnlyList<int> global::CardGame.Core.IProgramPaidCardFacts.TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind, context);
    bool global::CardGame.Core.IProgramPaidCardFacts.CanSelectPindianOpponent(ProgramSkillFrame frame, int seat) => _coreEquipmentSlotGroupProgramsOperations.CanSelectProgramPindianOpponent(frame, seat);
    Card[] global::CardGame.Core.IProgramPaidCardFacts.ClaimableDamageCards(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetClaimableProgramDamageCards(frame);
    bool global::CardGame.Core.IProgramPaidCardFacts.FrozenSuitMatchesChoice(ProgramSkillFrame frame, string cardBind, string choiceBind) => _coreProgramRandomTransfersOperations.DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind);
    bool global::CardGame.Core.IProgramPaidCardFacts.BoundCardsMatchCategories(ProgramSkillFrame frame, string bind, IReadOnlyList<SkillProgramCardCategory> categories) => _coreProgramCategoryChallengesOperations.DoProgramBoundCardsMatchCategories(frame, bind, categories);
    void global::CardGame.Core.IProgramPaidCardOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void global::CardGame.Core.IProgramPaidCardOperations.SetCardSet(long frameId, string bind, IReadOnlyList<int> ids, SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? locations, Suit? frozenSuit) => _coreProgramLifecycleOperations.SetProgramCardSet(frameId, bind, ids, visibility, locations, frozenSuit);
    void global::CardGame.Core.IProgramPaidCardOperations.Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, destination, reason, disclosure: disclosure);
    bool global::CardGame.Core.IProgramPaidCardOperations.MoveToProcessing(ProgramSkillFrame frame, Card card, CardLocation source, CardMoveReason reason, CardMovementDisclosure disclosure)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Processing, reason, disclosure: disclosure);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
            return true;
        // A generated field entity can be destroyed by this exact first
        // departure. Credit that real payment; never run a second transfer
        // from Processing or report an obtain that did not take place.
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id && source.Zone == CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > before && move.CardId == card.Id).ToArray()is [var movement] && movement.CardKind == card.Kind && movement.From == source && movement.To == CardLocation.OutsideGame && movement.Reason == reason && _cardZonesOperations._cardZones.GetLocation(card.Id) == movement.To && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement))
            return false;
        throw new InvalidOperationException("A selected transfer lost its exact first processing entry or native destruction proof.");
    }

    bool global::CardGame.Core.IProgramPaidCardOperations.SettleRetainedGain(ProgramSkillFrame frame, Card card, CardLocation source, CardLocation destination, CardMoveReason reason) => _coreHappyWeiYanGainDestinationsOperations.TrySettleHappyWeiYanRetainedGain(card, source, destination, reason, frame.Id);
    void global::CardGame.Core.IProgramPaidCardOperations.Give(ProgramSkillFrame frame, int giver, Card card, CardLocation source, CardLocation destination, CardMoveReason reason, bool throughProcessing, CardMovementDisclosure disclosure)
    {
        if (((global::CardGame.Core.IProgramPaidCardOperations)this).SettleRetainedGain(frame, card, source, destination, reason))
            return;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (throughProcessing && !((global::CardGame.Core.IProgramPaidCardOperations)this).MoveToProcessing(frame, card, source, reason, disclosure))
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(card, throughProcessing ? CardLocation.Processing : source, destination, reason, _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, giver, destination.OwnerSeat!.Value, [card.Id], before), disclosure: disclosure);
    }

    bool global::CardGame.Core.IProgramPaidCardOperations.BeginMovementWindow() => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow();
    bool global::CardGame.Core.IProgramPaidCardOperations.BeginPublicHandReveal(long frameId, string returnKey) => _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(frameId, returnKey);
    void global::CardGame.Core.IProgramPaidCardOperations.ReturnMovement(long frameId) => _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    void global::CardGame.Core.IProgramPaidCardOperations.Log(string type, string message, int? actor) => _coreMatchLogOperations.AddLog(type, message, actor);
}
