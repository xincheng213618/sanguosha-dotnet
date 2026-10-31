#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFriendDuelResponsesRuntime
{
    internal IReadOnlyList<PromptChoice> FriendDuelResponseChoices(global::CardGame.Core.NativeOwners.DuelHandle duel, CharacterState responder)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(duel.ResolutionId);
        if (use?.FriendRecastDuel is not { } origin || !_coreFriendEventsOperations.IsFriendRecastDuelUse(use.Id) || use.FriendRecastDuelResponses >= origin.RecastCount || responder.Seat != duel.ResponderSeat || _coreCardUseModulesOperations.IsCardUseForbidden(responder.Seat, CardKind.Slash, CardActionType.Response))
            return[];
        var choices = new List<PromptChoice>();
        foreach (var zone in new[]
        {
            CardZoneKind.Hand,
            CardZoneKind.Equipment,
            CardZoneKind.Judgment
        }

        )
        {
            var location = new CardLocation(zone, responder.Seat);
            var cards = _cardZonesOperations._cardZones.CardsAt(location);
            for (var slot = 0; slot < cards.Count; slot++)
            {
                var card = cards[slot];
                // The shared discard query also enforces self hand-category restrictions.
                if (card.IsGeneralWeapon || !_coreFriendEventsOperations.FriendDuelCategoryAllowed(origin.OwnerSeat, card.Kind) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(responder.Seat, card, location, OwnedCardMoveIntent.Discard))
                    continue;
                choices.Add(new PromptChoice(new($"friend-duel.use-{(use.Id)}.zone-{(zone)}.slot-{(slot)}"), $"钦英：弃置【{(card.DisplayName)}】，视为打出普通杀。", [card.Id], [], new Dictionary<string, string> { ["response"] = "friend-duel-discard", ["frame-id"] = use.Id.ToString(CultureInfo.InvariantCulture), ["card-owner-seat"] = responder.Seat.ToString(CultureInfo.InvariantCulture), ["source-zone"] = zone.ToString(), ["slot-index"] = slot.ToString(CultureInfo.InvariantCulture) }));
            }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void ResolveFriendDuelDiscard(PromptChoice supplied)
    {
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("Recast Duel payment lost its active Duel.");
        var responder = _coreMatchParticipantsPlayersAccess._players[duel.ResponderSeat];
        var canonical = FriendDuelResponseChoices(duel, responder).SingleOrDefault(c => c.Id == supplied.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } prompt || prompt.PlayerSeat != responder.Seat || canonical is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [supplied]) || supplied.Cards is not [var id])
            throw new InvalidOperationException("Recast Duel accepts only its exact published current discard.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(duel.ResolutionId)!;
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _coreCardAttackStateOperations.GetAttackCard(id);
        if (from.OwnerSeat != responder.Seat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment))
            throw new InvalidOperationException("Recast Duel cannot substitute another owner's card.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(use.Id);
        _coreNativeResponseWindowsOperations.SetResponseParentStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var frame = new FriendDuelResponseFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, use.Id, responder.Seat, duel.OpponentSeat, id, card.Kind, from, use.FriendRecastDuelResponses, use.FriendRecastDuel!);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, parent => parent with { FriendRecastDuelResponses = parent.FriendRecastDuelResponses + 1 });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.Origin.OwnerSeat, responder.Seat, card, from, new("skill.friend-qinying.response-cost"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FriendDuelDiscardPaidEvent(use.Id, responder.Seat, id, from, frame.PreviousResponses + 1));
        ContinueFriendDuelResponse(frame.Id);
    }

    internal bool IsFriendResponseParent(FriendDuelResponseFrame f) => _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && duel.ResolutionId == f.CardUseFrameId && duel.ResponderSeat == f.ResponderSeat && duel.OpponentSeat == f.OpponentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(f.CardUseFrameId)is { } use && use.FriendRecastDuel == f.Origin && use.FriendRecastDuelResponses == f.PreviousResponses + 1 && f.PreviousResponses >= 0 && use.FriendRecastDuelResponses <= f.Origin.RecastCount && _coreFriendEventsOperations.IsFriendRecastDuelUse(use.Id);
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 70, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(FriendDuelResponseFrame), 100)]
    internal void ContinueFriendDuelResponse(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not FriendDuelResponseFrame f || f.Id != id || !IsFriendResponseParent(f))
            throw new InvalidOperationException("Paid Recast Duel response lost its original typed owner.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.FriendDuelResponse) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.FriendDuelResponse) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendDuelDiscardPaidEvent>().Count(e => e.CardUseFrameId == f.CardUseFrameId && e.ResponderSeat == f.ResponderSeat && e.CardId == f.CardId && e.ResponseCount == f.PreviousResponses + 1) != 1 || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == f.CardId && m.From == f.From && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill.friend-qinying.response-cost"))
            throw new InvalidOperationException("Recast Duel response lost its actual paid discard.");
        _coreRuntimeOperations.PopResolutionFrame(id, ResolutionFrameKind.FriendDuelResponse);
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel!;
        var responder = _coreMatchParticipantsPlayersAccess._players[f.ResponderSeat];
        if (!responder.IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OpponentSeat].IsAlive)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(duel.Attack);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(f.CardUseFrameId, f.ResponderSeat, true, null, CardKind.Slash));
        // The discarded cost is not the material of the virtual Slash.
        if (!_coreCardActionsOperations.TryBeginCardResponsePrograms(duel.Attack, responder, responder, null, f.OpponentSeat, CardKind.Slash, [], ProgramCardContinuation.DuelSlash))
            _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, f.ResponderSeat);
    }

    internal bool IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement) => movement.ResumeFriendDuelResponseFrameId == parent.Id && movement.Batch.ParentFrameId == parent.Id && movement.Batch.AwaitingProgramFrameId is null && IsFriendResponseParent(parent);
}
