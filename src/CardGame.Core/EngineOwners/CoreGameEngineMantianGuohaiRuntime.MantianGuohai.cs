#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMantianGuohaiRuntime
{
    internal const string MantianGuohaiGiftSkill = "mobile:mantian-guohai-gift";
    internal static string MantianGuohaiGiftGrant(long useId, int index) => $"card-use:mantian-guohai:{(useId)}:{(index)}";
    internal string MantianGuohaiGiftSource(long useId, int index) => $"turn:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)}:{(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)}:{(MantianGuohaiGiftGrant(useId, index))}";
    internal IReadOnlyList<int> CreateMobileShiMantianGuohaiCards(int seat)
    {
        var ids = new List<int>();
        foreach (var suit in new[]
        {
            Suit.Heart,
            Suit.Diamond
        }

        )
        {
            var id = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1);
            var card = new Card(id, CardKind.MantianGuohai, suit, 5);
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(id);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(seat), new("skill-program.mobile-shi-dong-zhao.opening"));
            ids.Add(id);
        }

        return Array.AsReadOnly(ids.ToArray());
    }

    internal void AddMantianGuohaiActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards)
    {
        foreach (var card in playableCards.Where(c => c.Kind == CardKind.MantianGuohai))
        {
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, card.Kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, card.Kind, [card.Id], null, null)))
                continue;
            var targets = _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != actor.Seat && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count + _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).Count > 0 && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, card.Kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, card.Kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))).Select(t => t.Seat).ToArray();
            foreach (var first in targets)
            {
                actions.Add(new(LegalActionKind.MantianGuohai, card.Id, first, $"对 {(_coreMatchParticipantsPlayersAccess._players[first].Name)} 使用【瞒天过海】"));
                foreach (var second in targets.Where(t => t != first))
                    actions.Add(new(LegalActionKind.MantianGuohai, card.Id, first, $"对 {(_coreMatchParticipantsPlayersAccess._players[first].Name)}、{(_coreMatchParticipantsPlayersAccess._players[second].Name)} 使用【瞒天过海】", TargetSeats: [first, second]));
            }
        }
    }

    internal void ResolveMantianGuohai(CharacterState source, Card card, LegalAction action)
    {
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: card.Id).Any(candidate => candidate.Kind == LegalActionKind.MantianGuohai && candidate.CardId == card.Id && candidate.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("Mantian Guohai lost its legal material or ordered targets.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, action.TargetSeats);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { MantianGuohaiGift = new() });
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, source.Seat, action.TargetSeats, LegalActionKind.MantianGuohai);
    }

    internal bool IsMantianGuohaiUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardKind: CardKind.MantianGuohai, MantianGuohaiGift: not null, Action: { EffectiveKind: CardKind.MantianGuohai } };
    internal bool KeepMantianGuohaiMaterial(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MantianGuohaiGift: { Complete: false } };
    internal void PrepareMantianGuohaiEffectRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MantianGuohaiGift: { Complete: true } state } use)
            return;
        if (!IsMantianGuohaiUse(id) || !_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(id) || use.TargetIndex != 0 || state.ProgramFrameId is not null || state.GiftIndex != state.GiftTargets.Count || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Mantian Guohai cannot repeat before its original gifts finish.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MantianGuohaiGift = new() { EffectRound = checked(state.EffectRound + 1) } });
    }

    internal void ResolveMantianGuohaiEffect(NullificationWindowFrame pending)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("Mantian Guohai lost its native use.");
        var target = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) ?? throw new InvalidOperationException("Mantian Guohai lost its target.");
        if (!IsMantianGuohaiUse(use.Id) || use.MantianGuohaiGift is not { Complete: false, ProgramFrameId: null } state || state.GiftTargets.Contains(target) || use.Action is null || !use.Action.TargetSeats.Contains(target))
            throw new InvalidOperationException("Mantian Guohai changed its acquisition cursor.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MantianGuohaiGift = state with { GiftTargets = state.GiftTargets.Append(target).ToArray() } });
        _coreNativeTrickEffectsOperations.ResolveTargetCardEffect(pending);
    }

    internal bool TryContinueMantianGuohaiGifts(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MantianGuohaiGift: { Complete: false } state } use)
            return false;
        if (!IsMantianGuohaiUse(id) || state.ProgramFrameId is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Mantian Guohai cannot advance above an unfinished child.");
        var next = state.GiftIndex;
        while (next < state.GiftTargets.Count && (!_coreMatchParticipantsPlayersAccess._players[state.GiftTargets[next]].IsAlive || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat]).Count == 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None))
            next++;
        if (next == state.GiftTargets.Count)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MantianGuohaiGift = state with { GiftIndex = next, Complete = true } });
            return false;
        }

        var program = _contentRegistryOperations._contentRegistry.GetSkill(MantianGuohaiGiftSkill).Program!;
        var grant = MantianGuohaiGiftGrant(id, next);
        _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].SkillGrants.Grant(new(grant, MantianGuohaiGiftSkill, grant, MantianGuohaiGiftSource(id, next), TurnExpiry: new(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)));
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { Step = ResolutionFrameStep.ResolvingEffect, MantianGuohaiGift = state with { GiftIndex = next, ProgramFrameId = frameId } });
        var frame = new ProgramSkillFrame(frameId, use.SourceSeat, MantianGuohaiGiftSkill, "card-effect", program.GameplayHash, 0, [], Array.AsReadOnly(new[] { state.GiftTargets[next] }))
        {
            SkillInstanceId = grant,
            MantianGuohaiReturn = new(id, use.Action!.ActionId, next, state.GiftTargets[next])
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 47700, SkillProgramEffectOp.SelectOwnedCards)]
    internal bool AllowsMantianGuohaiGiftActivation(ProgramActivationLegalityContext gate) => gate.SkillId != MantianGuohaiGiftSkill;
    internal bool IsMantianGuohaiGiftProgram(ProgramSkillFrame frame)
    {
        if (frame.MantianGuohaiReturn is not { } receipt || frame.SkillId != MantianGuohaiGiftSkill || frame.TriggerId is not null || frame.ActivationId != "card-effect" || frame.SelectedCardIds.Count != 0 || !frame.SelectedTargetSeats.SequenceEqual(new[] { receipt.RecipientSeat }) || frame.SkillInstanceId != MantianGuohaiGiftGrant(receipt.CardUseFrameId, receipt.GiftIndex) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.CardUseFrameId)is not { MantianGuohaiGift: { Complete: false } state, Action: { } action } use || !IsMantianGuohaiUse(use.Id) || state.ProgramFrameId != frame.Id || state.GiftIndex != receipt.GiftIndex || action.ActionId != receipt.ActionId || frame.OwnerSeat != use.SourceSeat || state.GiftIndex < 0 || state.GiftIndex >= state.GiftTargets.Count || state.GiftTargets[state.GiftIndex] != receipt.RecipientSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == use.Id && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Any(grant => grant.GrantId == frame.SkillInstanceId && grant.SkillId == frame.SkillId && grant.SkillInstanceId == frame.SkillInstanceId && grant.SourceId == MantianGuohaiGiftSource(receipt.CardUseFrameId, receipt.GiftIndex) && grant.TurnExpiry == new SkillGrantTurnExpiry(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat));
    }

    internal bool TryCompleteMantianGuohaiGift(ProgramSkillFrame frame, bool completed)
    {
        if (frame.MantianGuohaiReturn is not { } receipt)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !IsMantianGuohaiGiftProgram(frame))
            throw new InvalidOperationException("Mantian Guohai lost its issued gift return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, completed));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.RemoveGrant(frame.SkillInstanceId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(receipt.CardUseFrameId, use => use with { MantianGuohaiGift = use.MantianGuohaiGift!with { GiftIndex = receipt.GiftIndex + 1, ProgramFrameId = null } });
        var owner = _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.CardUseFrameId)!;
        _coreNativeCardUseCompletionOperations.FinishCardUse(owner.Id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(owner.Id, owner.CardId, requireProcessing: true), CardKind.MantianGuohai);
        return true;
    }

    internal bool MantianGuohaiFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => child is ProgramSkillFrame { MantianGuohaiReturn: { } receipt } frame && receipt.CardUseFrameId == parent.Id && IsMantianGuohaiGiftProgram(frame);
}
