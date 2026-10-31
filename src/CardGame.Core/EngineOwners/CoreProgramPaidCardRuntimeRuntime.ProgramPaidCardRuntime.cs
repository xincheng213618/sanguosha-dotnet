#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPaidCardRuntimeRuntime
{
    bool IProgramPaidCardFacts.CanContinueAfterOwnerDeath(ProgramSkillFrame frame) => _continuationPermissions.Value.CanContinueProgramAfterOwnerDeath(frame);
    bool IProgramPaidCardFacts.CanContinueAfterSkillLoss(ProgramSkillFrame frame) => _continuationPermissions.Value.CanContinueProgramAfterSkillLoss(frame);
    bool? IProgramPaidCardFacts.EvaluateOptionExpression(ProgramSkillFrame frame, ProgramExpression expression) => ProgramExpressions.EvaluateBool(expression, _expressionWorld.CreateProgramExpressionWorld(frame));
    int IProgramPaidCardFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;
    long IProgramPaidCardFacts.ActiveMovementBatchId => _activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Peek();

    IReadOnlyList<int> IProgramPaidCardFacts.Seats => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(player => player.Seat).ToArray());

    int IProgramPaidCardFacts.Seat(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Seat;
    bool IProgramPaidCardFacts.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IProgramPaidCardFacts.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IProgramPaidCardFacts.IsHuman(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IProgramPaidCardFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    bool IProgramPaidCardFacts.EquipmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentAreaAbolished;
    bool IProgramPaidCardFacts.JudgmentAreaAbolished(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].JudgmentAreaAbolished;
    int IProgramPaidCardFacts.EquipmentSlotCapacity(int seat, EquipmentSlot slot) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot);
    EquipmentSlot IProgramPaidCardFacts.FieldEquipmentSlot(Card card) => _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
    bool IProgramPaidCardFacts.CanInvokeProgramSkill(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame);
    ContentSkillDefinition IProgramPaidCardFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry!.GetSkill(skillId);
    PlayerSkillContext IProgramPaidCardFacts.SkillContext(int seat) => _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<Card> IProgramPaidCardFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> IProgramPaidCardFacts.Equipment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> IProgramPaidCardFacts.Judgment(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray());
    IReadOnlyList<Card> IProgramPaidCardFacts.CardsAt(CardLocation location) => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(location).ToArray());
    CardLocation IProgramPaidCardFacts.Location(int cardId) => _cardZonesOperations._cardZones.GetLocation(cardId);
    int IProgramPaidCardFacts.ProviderSeat(ProgramSkillFrame frame) => _characterGuanYuRuntimeOperations.GuanParentAction(frame).ProviderSeat;
    SkillProgramEffect IProgramPaidCardFacts.OwnedSelectionEffect(ProgramSkillFrame frame) => _coreProgramOwnedCardsOperations.GetOwnedSelectionEffect(frame);
    bool IProgramPaidCardFacts.IsSatisfiedOwnHandReward(ProgramSkillFrame frame, SkillProgramEffect effect) => _characterLeiBoAnniversaryOperations.Value.IsAnniversaryLeiBoSelfLoot(frame, effect);
    bool IProgramPaidCardFacts.ForeignDiscardPrevented(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, source, intent);
    bool IProgramPaidCardFacts.SelfDiscardForbidden(int chooser, Card card, CardLocation source, OwnedCardMoveIntent intent) => _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, card, source, intent);
    bool IProgramPaidCardFacts.AllowsOwnedCardPayment(ProgramSkillFrame frame, CardLocation source, Card card) => _characterDaQiaoMobileOperations.AllowsPostCostSlashRedirectPayment(frame, source, card);
    bool IProgramPaidCardFacts.IsActiveSourceEquipment(int owner, string skillId, string instanceId, Card card) => _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card);
    bool IProgramPaidCardFacts.IsZixiJudgment(int cardId) => _characterDaQiaoLeOperations.IsZixiQinJudgmentCard(cardId);
    bool IProgramPaidCardFacts.CanTransferZixiJudgment(Card card, int destinationSeat) => _characterDaQiaoLeOperations.CanTransferZixiQinJudgmentCard(card, destinationSeat);
    bool IProgramPaidCardFacts.HasCardPolicy(int seat, SkillProgramCardPolicyKind policy) => _coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[seat], policy);
    CardKind IProgramPaidCardFacts.JudgmentKind(Card card) => _coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card);
    bool IProgramPaidCardFacts.IsDelayedCard(CardKind kind) => CoreNativeCardClassificationsRuntime.IsDelayedCard(kind);
    bool IProgramPaidCardFacts.HasJudgmentEffectiveCard(int seat, CardKind kind) => _coreActualTurnsOperations.HasJudgmentEffectiveCard(_coreMatchParticipantsPlayersAccess._players[seat], kind);
    Suit IProgramPaidCardFacts.EffectiveSuit(int seat, Card card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], card);
    int IProgramPaidCardFacts.CountLivingInAttackRange(int seat) => _coreProgramCardPaymentsOperations.CountLivingInAttackRange(seat);
    IReadOnlyList<int> IProgramPaidCardFacts.TargetSeats(int owner, SkillProgramTargetKind kind, ProgramSkillWindowContext? context) => _coreProgramLifecycleOperations.GetProgramTargetSeats(owner, kind, context);
    bool IProgramPaidCardFacts.CanSelectPindianOpponent(ProgramSkillFrame frame, int seat) => _coreEquipmentSlotGroupProgramsOperations.CanSelectProgramPindianOpponent(frame, seat);
    Card[] IProgramPaidCardFacts.ClaimableDamageCards(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetClaimableProgramDamageCards(frame);
    bool IProgramPaidCardFacts.FrozenSuitMatchesChoice(ProgramSkillFrame frame, string cardBind, string choiceBind) => _coreProgramRandomTransfersOperations.Value.DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind);
    bool IProgramPaidCardFacts.BoundCardsMatchCategories(ProgramSkillFrame frame, string bind, IReadOnlyList<SkillProgramCardCategory> categories) => _coreProgramCategoryChallengesOperations.DoProgramBoundCardsMatchCategories(frame, bind, categories);
    void IProgramPaidCardOperations.Cancel(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    void IProgramPaidCardOperations.SetCardSet(long frameId, string bind, IReadOnlyList<int> ids, SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? locations, Suit? frozenSuit) => _coreProgramLifecycleOperations.SetProgramCardSet(frameId, bind, ids, visibility, locations, frozenSuit);
    void IProgramPaidCardOperations.Move(Card card, CardLocation source, CardLocation destination, CardMoveReason reason, CardMovementDisclosure disclosure, Action<CardMovementRecord>? beforeFact) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, destination, reason, beforeFact, disclosure: disclosure);
    bool IProgramPaidCardOperations.MoveToProcessing(ProgramSkillFrame frame, Card card, CardLocation source, CardMoveReason reason, CardMovementDisclosure disclosure, Action<CardMovementRecord>? beforeFact)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Processing, reason, beforeFact, disclosure: disclosure);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Processing)
            return true;
        // A generated field entity can be destroyed by this exact first
        // departure. Credit that real payment; never run a second transfer
        // from Processing or report an obtain that did not take place.
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id && source.Zone == CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > before && move.CardId == card.Id).ToArray()is [var movement] && movement.CardKind == card.Kind && movement.From == source && movement.To == CardLocation.OutsideGame && movement.Reason == reason && _cardZonesOperations._cardZones.GetLocation(card.Id) == movement.To && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement))
            return false;
        throw new InvalidOperationException("A selected transfer lost its exact first processing entry or native destruction proof.");
    }

    bool IProgramPaidCardOperations.SettleRetainedGain(ProgramSkillFrame frame, Card card, CardLocation source, CardLocation destination, CardMoveReason reason) => _coreHappyWeiYanGainDestinationsOperations.TrySettleHappyWeiYanRetainedGain(card, source, destination, reason, frame.Id);
    void IProgramPaidCardOperations.Give(ProgramSkillFrame frame, int giver, Card card, CardLocation source, CardLocation destination, CardMoveReason reason, bool throughProcessing, CardMovementDisclosure disclosure, Action<CardMovementRecord>? beforeFact)
    {
        if (((IProgramPaidCardOperations)this).SettleRetainedGain(frame, card, source, destination, reason))
            return;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (throughProcessing && !((IProgramPaidCardOperations)this).MoveToProcessing(frame, card, source, reason, disclosure, beforeFact))
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(card, throughProcessing ? CardLocation.Processing : source, destination, reason, _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, giver, destination.OwnerSeat!.Value, [card.Id], before, beforeFact), disclosure: disclosure);
    }

    bool IProgramPaidCardOperations.BeginMovementWindow() => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow();
    bool IProgramPaidCardOperations.BeginPublicHandReveal(long frameId, string returnKey) => _characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(frameId, returnKey);
    void IProgramPaidCardOperations.ReturnMovement(long frameId) => _coreRuntimeOperations.ReturnRuntimeProgramMovement(frameId);
    void IProgramPaidCardOperations.Log(string type, string message, int? actor) => _coreMatchLogOperations.AddLog(type, message, actor);
}
