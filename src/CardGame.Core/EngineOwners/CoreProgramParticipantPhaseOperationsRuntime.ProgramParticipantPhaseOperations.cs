#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramParticipantPhaseOperationsRuntime
{
    internal bool HasActuallyUsableHandCard(CharacterState owner) => _coreNativeLegalActionsOperations.BuildLegalActions(owner, includeProgramActions: false).Any(action => action.CardId is { } id && id != 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner.Seat) && action.Kind is not (LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.EndPlay or LegalActionKind.Recast) || action.ConversionSource is not null && action.MinCardCount > 1 && action.SelectableCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(owner.Seat)) || action.Kind == LegalActionKind.UseEquipmentEffect && action.EquipmentKind == CardKind.ZhangbaSerpentSpear && _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner).Any(pair => !IsTurnPhysicalUseForbidden(owner.Seat, pair.Select(c => c.Id).ToArray())));
    internal bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target) => _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner, skill, $"{(usage)}:target:{(target)}", SkillUsageScope.Phase) == 0;
    internal void ConsumeProgramTargetPhaseLedger(ProgramSkillFrame frame, string usage)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var activation = ProgramInstructionResolver.Default.Resolve(_contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!, ProgramInstructionSourceKind.Activation, frame.ActivationId).Activation!;
        if (active.TriggerId is not null || active.SelectedTargetSeats is not [var target] || activation.TargetPhaseLedgerId != usage || !CanActivateTargetPhaseLedger(active.OwnerSeat, active.SkillId, usage, target))
            throw new InvalidOperationException("Participant phase usage is unavailable.");
        var key = $"{(usage)}:target:{(target)}";
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(active.OwnerSeat, active.SkillId, key, SkillUsageScope.Phase, 1))
            throw new InvalidOperationException("Participant phase usage could not advance.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(active.OwnerSeat, active.SkillId, key, SkillUsageScope.Phase, 1));
    }

    internal bool IsTurnPhysicalUseForbidden(int seat, IReadOnlyList<int> ids) => IsTurnPhysicalUseForbiddenByExistingPolicies(seat, ids) || _characterZhouYuOlMouOperations.MouGuquPhysicalUseForbidden(seat, ids) || _characterHeYanAnniversaryOperations.IsAnniversaryHeYanHandUseForbidden(seat, ids) || _characterWuFuAnniversaryOperations.IsAnniversaryWuFuHandUseForbidden(seat, ids);
    internal bool IsTurnPhysicalUseForbiddenByExistingPolicies(int seat, IReadOnlyList<int> ids) => _characterZhugeLiangWumiaoOperations.IsWuZglUseMaterialForbidden(seat, ids) || ids.Any(id => _coreProgramCardLegalityOperations.IsProgramEntityPhysicalRestricted(seat, id)) || _coreTurnHandCategoryRestrictionsOperations.IsHandCategoryMaterialRestricted(ids) || ids.Count > 0 && _corePlayCardHistoryOperations.CaptureUsedCardSuit(seat, ids, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(ids[0])).Single(c => c.Id == ids[0]))is { } suit && IsTurnSuitUseForbidden(seat, suit);
    internal bool IsTurnSuitUseForbidden(int seat, Suit suit) => _characterDiaoChanHappyGodOperations.IsHappyDiaoChanSuitForbidden(seat, suit) || _turnCardUseEffectsOperations._turnCardUseEffects.ActionProhibitions.Any(p => p.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && p.Source.OwnerSeat == seat && p.Suits?.Contains(suit) == true && p.ActionTypes.Contains(CardActionType.Use));
    internal void GrantProgramTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string bind)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(active, bind);
        if (set.CardIds is not [var id])
            throw new InvalidOperationException("Suit restriction requires one chosen real card.");
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        if (location != CardLocation.DiscardPile)
            throw new InvalidOperationException("Suit restriction requires completed real discard payment.");
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == id);
        var suit = set.FrozenRevealedSuit ?? card.Suit;
        var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantActionProhibition(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, frame.Id, frame.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), [], [CardActionType.Use], [suit]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionProhibitionGrantedEvent(granted));
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { CardSetBindings = Array.AsReadOnly(active.CardSetBindings.Select(b => b.Name == bind ? b with { Visibility = SkillProgramCardSetVisibility.Public, FrozenRevealedSuit = suit } : b).ToArray()) });
    }

    internal void CollectFullDiscardPhaseSuit(Card card, CardMovementRecord move)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move)is not { OwnerSeat: { } owner } || owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return;
        if (_coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
            _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Clear();
        }

        _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Add(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], card));
    }

    internal bool FullDiscardPhaseSuitsAllDistinct => _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuitTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Count >= 2 && _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Distinct().Count() == _coreProgramParticipantPhaseOperationsFullDiscardPhaseSuitTurnFullDiscardPhaseSuitsAccess._fullDiscardPhaseSuits.Count;

    void IParticipantPhaseProgramEffectHost.ConsumeTargetPhaseLedger(ProgramSkillFrame frame, string usageId) => ConsumeProgramTargetPhaseLedger(frame, usageId);
    void IParticipantPhaseProgramEffectHost.GrantTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string sourceBind) => GrantProgramTurnBoundSuitUseProhibition(frame, sourceBind);
}
