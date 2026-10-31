#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSimultaneousPindianSelectionsRuntime
{
    [ProgramViewContributor(ProgramViewContribution.ConcealedCardExclusion, 10)]
    internal bool IsSimultaneousPrivateTopCard(int id) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SimultaneousPindian is { Revealed: false } r && r.TopReceipts.Any(t => t.CardId == id));
    internal CardLocation SimultaneousOriginalLocation(SimultaneousPindianReceipt r, PindianRevealedCard card) => r.TopReceipts.Any(t => t.Seat == card.Seat && t.CardId == card.CardId) ? CardLocation.Processing : CardLocation.Hand(card.Seat);
    internal bool CanOfferSimultaneousTop(ProgramSkillFrame f, int seat) => _corePindianPoliciesOperations.CanOfferPindianTopChoice(_coreMatchParticipantsPlayersAccess._players[seat]);
    internal void ContinueSimultaneousRandomPolicies(ProgramSkillFrame f)
    {
        var r = f.SimultaneousPindian!;
        var selection = r.RandomSelection!;
        while (selection.Index < selection.Candidates.Count)
        {
            var c = selection.Candidates[selection.Index];
            // A simultaneously shared source entity can be forced only once.
            // Other participants keep independent exact target receipts.
            if (selection.Receipts.All(p => p.TargetSeat != c.TargetSeat) && _corePindianRandomSelectionsOperations.PindianPolicySourceLive(c.OwnerSeat, c.Source, SkillProgramCardPolicyKind.PindianOpponentRandomHand) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.TargetSeat]).Count > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { RandomSelection = selection } });
                _coreSimultaneousPindianOperations.PublishSimultaneousChoice(f);
                return;
            }

            selection = selection with
            {
                Index = selection.Index + 1
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { RandomSelection = selection, Stage = SimultaneousPindianStage.Selecting } });
        ContinueSimultaneousSelection(f);
    }

    internal void ResolveSimultaneousRandomPolicy(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.SimultaneousPindian!;
        var selection = r.RandomSelection!;
        var c = selection.Candidates[selection.Index];
        if (selection.Receipts.Any(p => p.TargetSeat == c.TargetSeat) || !_corePindianRandomSelectionsOperations.PindianPolicySourceLive(c.OwnerSeat, c.Source, SkillProgramCardPolicyKind.PindianOpponentRandomHand))
            throw new InvalidOperationException("A simultaneous random policy lost its exact live owner or already-fixed target.");
        if (choice.Parameters["branch"] == "random")
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.TargetSeat]).OrderBy(card => card.Id).ToArray();
            var before = _randomOperations._random.State;
            var card = hand[_randomOperations._random.Next(hand.Length)];
            var paid = new PindianRandomReceipt(c.OwnerSeat, c.TargetSeat, c.Source, card.Id, before, _randomOperations._random.State);
            selection = selection with
            {
                Receipts = Array.AsReadOnly(selection.Receipts.Append(paid).ToArray()),
                Index = selection.Index + 1
            };
            if (c.TargetSeat == f.OwnerSeat)
            {
                var rank = _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[c.TargetSeat], card);
                r = r with
                {
                    Cards = [new(c.TargetSeat, card.Id, rank, rank)]
                };
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { RandomSelection = selection } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianRandomHandCommittedEvent(f.Id, c.OwnerSeat, c.TargetSeat, c.Source));
        }

        if (choice.Parameters["branch"] != "random")
            selection = selection with
            {
                Index = selection.Index + 1
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = f.SimultaneousPindian!with { RandomSelection = selection } });
        ContinueSimultaneousRandomPolicies(f);
    }

    internal void ContinueSimultaneousSelection(ProgramSkillFrame f)
    {
        var r = f.SimultaneousPindian!;
        // Every reserved top is already privately held in Processing, so an
        // exhausted deck can refill normally for the next participant.
        if (r.SelectionIndex < r.Opponents.Count && _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) == 0 && _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile) > 0 && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[r.Opponents[r.SelectionIndex]], SkillProgramCardPolicyKind.PindianTopCardChoice).Any() && !r.RandomSelection!.Receipts.Any(p => p.TargetSeat == r.Opponents[r.SelectionIndex]))
        {
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { Stage = SimultaneousPindianStage.TopRefillChildren, PaymentKind = "top-refill", MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = f.SimultaneousPindian!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            if (!_coreSimultaneousPindianOperations.DrainSimultaneousChildren(f))
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        while (r.SelectionIndex < r.Opponents.Count)
        {
            var seat = r.Opponents[r.SelectionIndex];
            var forced = r.RandomSelection!.Receipts.SingleOrDefault(p => p.TargetSeat == seat);
            if (forced is null)
            {
                _coreSimultaneousPindianOperations.PublishSimultaneousChoice(f);
                return;
            }

            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Single(c => c.Id == forced.CardId);
            var rank = _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], card);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { Cards = r.Cards.Append(new(seat, card.Id, rank, rank)).ToArray(), SelectionIndex = r.SelectionIndex + 1 } });
            ContinueSimultaneousSelection(f);
            return;
        }

        RevealSimultaneousCommittedGroup(f);
    }

    internal void CommitSimultaneousSelection(ProgramSkillFrame f, int seat, PromptChoice choice)
    {
        var r = f.SimultaneousPindian!;
        Card card;
        int rank;
        if (choice.Parameters["branch"] == "maximum")
        {
            if (!_characterLuKangWumiaoOperations.CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[seat]))
                throw new InvalidOperationException("The discard-backed source lost its actual HE policy.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { Cards = r.Cards.Append(new(seat, -seat - 1, 0, 0)).ToArray(), SelectionIndex = r.SelectionIndex + 1 } });
            ContinueSimultaneousSelection(f);
            return;
        }

        if (choice.Parameters["branch"] == "top")
        {
            if (!CanOfferSimultaneousTop(f, seat) || r.RandomSelection!.Index != r.RandomSelection.Candidates.Count || r.RandomSelection.Receipts.Any(p => p.TargetSeat == seat) || f.PendingMovementContinuation is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
                throw new InvalidOperationException("A simultaneous top requires its exact unforced participant after every paid policy child.");
            card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last();
            var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.PindianTopCardChoice).First();
            var source = new CardConversionSource(binding.Source.SkillId, binding.Policy.Id, seat, binding.Source.SkillInstanceId);
            rank = card.Kind == CardKind.Alcohol && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(_coreMatchParticipantsPlayersAccess._players[seat]) ? 13 : _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], card);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            r = r with
            {
                TopReceipts = r.TopReceipts.Append(new(seat, card.Id, rank, source, before + 1)).ToArray(),
                Cards = r.Cards.Append(new(seat, card.Id, rank, rank)).ToArray(),
                SelectionIndex = r.SelectionIndex + 1,
                Stage = SimultaneousPindianStage.TopRefillChildren,
                PaymentKind = "top-reserve",
                MovementBefore = before,
                MovementAfter = before + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsDefinitions.SimultaneousTopReservationReason, _ => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimultaneousPindianTopCommittedEvent(f.Id, seat, source)), emitCardMovedEvents: false);
            if (!_coreSimultaneousPindianOperations.DrainSimultaneousChildren(f))
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }
        else
        {
            card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Single(c => c.Id == choice.Cards.Single());
            rank = _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], card);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r with { Cards = r.Cards.Append(new(seat, card.Id, rank, rank)).ToArray(), SelectionIndex = r.SelectionIndex + 1 } });
        ContinueSimultaneousSelection(f);
    }

    internal void RevealSimultaneousCommittedGroup(ProgramSkillFrame f)
    {
        if (_characterLuKangWumiaoOperations.TryBeginWuLuKangGroupBeforeReveal(f))
            return;
        if (_characterPangJiOlOperations.TryBeginDouyuSimultaneousPreReveal(f))
            return;
        var r = f.SimultaneousPindian!;
        var inputs = r.Cards.Select(c => _characterLuKangWumiaoOperations.WuLuKangActualInput(f, c.Seat, c.CardId, SimultaneousOriginalLocation(r, c), c.OriginalRank, r.TopReceipts.Any(t => t.CardId == c.CardId))).ToArray();
        if (inputs.Any(c => c.CardId <= 0 || _cardZonesOperations._cardZones.GetLocation(c.CardId) != c.From))
            throw new InvalidOperationException("A simultaneous private commitment lost its original or exactly paid replacement entity.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        foreach (var c in inputs.Where(c => c.From != CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), c.From, CardLocation.Processing, CardMoveReasons.PindianReveal);
        _characterLuKangWumiaoOperations.RevealWuLuKangInputs(f);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var mappedCards = _characterPangJiOlOperations.ApplyDouyuSimultaneousCards(f, inputs.Select(c => new PindianRevealedCard(c.Seat, c.CardId, c.Rank, c.Rank)).ToArray());
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.SimultaneousPindian!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimultaneousPindian = r = r with { Cards = mappedCards, Stage = SimultaneousPindianStage.RevealChildren, Revealed = true, RevealBefore = before, RevealAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, MovementBefore = before, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, PaymentKind = "reveal" }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var t in r.TopReceipts)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianTopSourceChosenEvent(t.Seat, t.Source, t.CardId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianCardsRevealedEvent(f.Id, r.Cards, true));
        if (!_coreSimultaneousPindianOperations.DrainSimultaneousChildren(f))
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
