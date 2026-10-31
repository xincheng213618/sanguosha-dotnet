#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    internal bool FeiYiYanruAvailable(CharacterState actor)
    {
        var odd = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count % 2 != 0;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && actor.IsAlive && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && !_coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiYanruPaidEvent>().Any(e => e.Source.OwnerSeat == actor.Seat && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && e.Odd == odd);
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 26600, SkillProgramEffectOp.YanruParityDrawDiscard)]
    internal bool AllowsFeiYiYanru(CharacterState actor) => FeiYiYanruAvailable(actor);
    internal bool FeiYiCanDiscard(int owner, Card card) => !card.IsGeneralWeapon && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, CardLocation.Hand(owner), OwnedCardMoveIntent.Discard);
    internal void PayFeiYiYanru(ProgramSkillFrame f)
    {
        var r = f.FeiYi!;
        if (r.Paid || !FeiYiSourceCurrent(f) || !FeiYiYanruAvailable(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) || r.Odd != (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count % 2 != 0) || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            throw new InvalidOperationException("Yanru must revalidate and consume exactly its accepted parity in this actual Play.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FeiYi = r with { Paid = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FeiYiYanruPaidEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.TurnOwner, r.PhaseInstance, r.Odd));
    }

    internal void BeginFeiYiDiscard(ProgramSkillFrame f)
    {
        var r = f.FeiYi!;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            CompleteFeiYi(f, r.Paid);
            return;
        }

        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]);
        var candidates = hand.Where(c => FeiYiCanDiscard(f.OwnerSeat, c)).Select(c => c.Id).ToArray();
        var minimum = hand.Count / 2;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FeiYi = r with { Stage = FeiYiStage.DiscardChoice, Minimum = minimum, HandCount = hand.Count, Candidates = candidates, Selected = [] } });
        // Native forced discard exhausts the legal set if fewer than the required
        // count are legally discardable; its fact still records the true bound.
        if (candidates.Length < minimum)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FeiYi = f.FeiYi!with { Selected = candidates } });
            PayFeiYiDiscard(f);
            return;
        }

        PublishFeiYiChoice(f);
    }

    internal void PayFeiYiDiscard(ProgramSkillFrame f)
    {
        var r = f.FeiYi!;
        if (!r.Paid)
        {
            PayFeiYiYanru(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.FeiYi!;
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count != r.HandCount || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => !r.Candidates.Contains(id) || !_cardZonesOperations._cardZones.Contains(CardLocation.Hand(f.OwnerSeat), id) || !FeiYiCanDiscard(f.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(id))) || r.Selected.Count < Math.Min(r.Minimum, r.Candidates.Count))
            throw new InvalidOperationException("Yanru lost its genuine minimum-half hand discard.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FeiYi = r with { Stage = FeiYiStage.DiscardChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Selected.Count > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, r.Selected, new(FeiYiDiscardReason));
        RecordFeiYiMovement(f.Id, before, FeiYiStage.DiscardChildren, r.Minimum);
        ContinueFeiYi(f.Id);
    }
}
