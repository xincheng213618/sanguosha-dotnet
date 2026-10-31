#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentRecastsRuntime
{
    internal void BeginEquipmentRecast(CharacterState actor, Card card, CardKind effectiveKind)
    {
        var cost = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card);
        if (_resolutionStackOperations._resolutionStack.Count != 0 || cost != CardLocation.Equipment(actor.Seat) && !(cost == CardLocation.Hand(actor.Seat) && (_coreModeCardRecastsOperations.CanModeHandEquipmentRecast(card) || _coreHellDizangProgramsOperations.CanHellDitingMountRecast(actor, card) || _modeRulesOperations._modeRules.Cards.AllowsNativePhysicalRecast(card.Kind, effectiveKind))))
            throw new InvalidOperationException("Equipment recast requires its fresh play boundary and owned equipment cost.");
        var frame = new EquipmentRecastFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, actor.Seat, card.Id, effectiveKind);
        frame = frame with
        {
            NativeRecastInvocation = _coreNativeRecastInvocationsOperations.CaptureNativeRecastInvocation(actor.Seat, actor.Seat, [card.Id], frame.Id)
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _corePhysicalMovementFacadesOperations.MoveCards([card], cost, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        ContinueEquipmentRecast(frame.Id);
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 180, NativeFrameAutomaticPending.Ignore, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(EquipmentRecastFrame), 340)]
    internal void ContinueEquipmentRecast(long frameId)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is EquipmentRecastFrame frame && frame.Id == frameId)
        {
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.EquipmentRecast) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
                return;
            if (!frame.DrawApplied)
            {
                if (_coreHellDizangProgramsOperations.TryContinueEquipmentRecastNativeDraw(frame))
                    return;
                // The physical equipment has already left its zone. Child returns
                // resume this scalar receipt and cannot pay or draw a second time.
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DrawApplied = true });
                var actor = _coreMatchParticipantsPlayersAccess._players[frame.ActorSeat];
                var drawn = _coreMatchOutcomeWinnerAccess._winner == Winner.None && actor.IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(actor, 1, log: false, reason: CardMoveReasons.RecastDraw) : Array.Empty<int>();
                _coreRuntimeOperations.ReplaceRuntimeTop(((EquipmentRecastFrame)_resolutionStackOperations._resolutionStack.Last())with { DrawCount = drawn.Count });
                if (_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(actor.Seat, out var brain))
                    brain.ObserveRecast(_coreActualTurnProgressTurnNumberAccess._turnNumber, frame.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(actor.Seat, frame.CardId, frame.EffectiveKind, drawn.Count));
                _coreNativeRecastInvocationsOperations.CompleteNativeRecastInvocation(frame.NativeRecastInvocation, drawn.Count);
                if (frame.NativeRecastInvocation is not null)
                    _coreRuntimeOperations.ReplaceRuntimeTop(((EquipmentRecastFrame)_resolutionStackOperations._resolutionStack.Last())with { NativeRecastInvocation = null });
                _coreMatchLogOperations.AddLog("Recast", $"{(actor.Name)} 重铸【{(CardCatalog.Get(frame.EffectiveKind).DisplayName)}】，摸 {(drawn.Count)} 张牌。", actor.Seat);
                continue;
            }

            _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.EquipmentRecast);
            return;
        }
    }
}
