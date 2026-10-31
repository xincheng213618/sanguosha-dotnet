#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaHuiOlRuntime
{
    internal const string SimaHuiViewReason = "program.sima-hui.chenghao.view", SimaHuiGiftReason = "program.sima-hui.chenghao.give";
    internal SkillProgramStepOutcome BeginSimaHuiDistribution(ProgramSkillFrame f)
    {
        if (!TrySimaHuiChainOrigin(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f), f.WindowContext!, out var proof))
            throw new InvalidOperationException("Chenghao requires actual applied elemental chain-origin damage.");
        var r = NewSimaHuiReceipt(f, "view-children")with
        {
            ChainOrigin = proof,
            MovementBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = new List<int>();
        for (var i = 0; i <= proof.LivingChainedCount; i++)
            if (_corePhysicalDrawFacadesOperations.DrawOneToProcessing(new(SimaHuiViewReason))is { } card)
                cards.Add(card.Id);
            else
                break;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { OriginalCards = cards, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimaHuiTopViewedEvent(f.Id, r.Source, r.GameplayHash, proof, f.SimaHui!.OriginalCards, r.MovementBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void IssueSimaHuiGift(ProgramSkillFrame f, int recipient)
    {
        var r = f.SimaHui!;
        var id = r.OriginalCards[r.Cursor];
        if (!_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing)
            throw new InvalidOperationException("Chenghao lost its original privately viewed card.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = "gift-children", ChosenHolder = recipient, MovementBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(c => c.Id == id), CardLocation.Processing, CardLocation.Hand(recipient), new(SimaHuiGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, recipient, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = f.SimaHui!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimaHuiCardDistributedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.Cursor, id, recipient, f.SimaHui!.MovementBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool ReturnSimaHuiUnassignedCards(ProgramSkillFrame f)
    {
        var cards = f.SimaHui!.OriginalCards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(c => c.Id == id)).ToArray();
        if (cards.Length == 0)
            return false;
        var r = f.SimaHui!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = r with { Stage = "return-children", MovementBefore = before, MovementAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Processing, CardLocation.DrawPile, new("program.sima-hui.chenghao.return"));
        _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(cards.Select(c => c.Id).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SimaHui = f.SimaHui!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SimaHuiUnassignedReturnedEvent(f.Id, r.Source, r.GameplayHash, Array.AsReadOnly(cards.Select(c => c.Id).ToArray()), before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
