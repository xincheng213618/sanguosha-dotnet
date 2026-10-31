#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal const string MouLuXunViewReason = "skill-program.mou-mobile:lianying.view";
    internal void BeginMouLuXunView(ProgramSkillFrame f)
    {
        var r = f.MouLuXun!;
        var cards = new List<int>();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        for (var i = 0; i < r.Limit; i++)
        {
            if (_corePhysicalDrawFacadesOperations.DrawOneToProcessing(new(MouLuXunViewReason))is not { } card)
                break;
            cards.Add(card.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunTopViewedEvent(f.Id, i, card.Id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuXun = f.MouLuXun!with { Viewed = cards, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouLuXun(f.Id);
    }

    internal void PayMouLuXunGift(ProgramSkillFrame f, int target)
    {
        var r = f.MouLuXun!;
        var id = r.Viewed[r.Cursor];
        if (!ValidMouLuXun(f) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing)
            throw new InvalidOperationException("Lianying can only give this exact privately viewed original entity to a living recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.GiftChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreResponseEntityExchangeOperations.EntityAtCurrentLocation(id), CardLocation.Processing, CardLocation.Hand(target), new("skill-program.mou-mobile:lianying.give"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, target, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceDown);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = f.MouLuXun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuXunEntityMovedEvent(f.Id, r.Cursor, id, move.From, move.To, move.Sequence));
        ContinueMouLuXun(f.Id);
    }

    internal void ReturnMouLuXunUnassigned(ProgramSkillFrame f)
    {
        var r = f.MouLuXun!;
        var cards = r.Viewed.Skip(r.Cursor).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(_coreResponseEntityExchangeOperations.EntityAtCurrentLocation).ToArray();
        if (cards.Length == 0)
        {
            FinishMouLuXun(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuXun = r with { Stage = MouLuXunStage.ReturnChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Processing, CardLocation.DrawPile, new("skill-program.mou-mobile:lianying.return"));
        _cardZonesOperations._cardZones.PlaceDrawPileCardsAtTop(cards.Select(c => c.Id).ToArray());
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuXun = f.MouLuXun!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueMouLuXun(f.Id);
    }

    internal bool IsMouLuXunPrivateEntity(int id) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MouLuXun is { Paid: true } r && r.Viewed.Skip(r.Cursor).Contains(id) && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && ValidMouLuXun(f));
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 32802)]
    internal IEnumerable<CardSnapshot> MouLuXunPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.MouLuXun is not null).SelectMany(f => f.MouLuXun!.Viewed.Skip(f.MouLuXun.Cursor)).Where(IsMouLuXunPrivateEntity).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreResponseEntityExchangeOperations.EntityAtCurrentLocation(id)));
    [ProgramViewContributor(ProgramViewContribution.PublicProcessingExclusion, 32802)]
    internal bool HideMouLuXunPrivateEntity(int id) => IsMouLuXunPrivateEntity(id);
    [ProgramViewContributor(ProgramViewContribution.InvariantProcessing, 32802)]
    internal IReadOnlyList<Card> RemoveMouLuXunPrivateProcessing(IReadOnlyList<Card> cards) => cards.Where(c => !IsMouLuXunPrivateEntity(c.Id)).ToArray();
}
