#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunYuMouRuntime
{
    internal IEnumerable<MouXyBizuoEndedEvent> MouXyAllocationsDue() => _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoEndedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.Actor == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(e.Grant.Source.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[e.Grant.Source.OwnerSeat].IsAlive && e.Grant.Source.SkillId == MouXunYuAnniversaryRuntime.Bizuo && e.Grant.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(MouXunYuAnniversaryRuntime.Bizuo).Program?.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouXyAllocationCompletedEvent>().Any(x => x.GrantFrameId == e.Grant.FrameId) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoStartedEvent>().Count(x => x.Grant == e.Grant && x.ActualTurn == e.ActualTurn && x.Actor == e.Actor) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoGrantedEvent>().Count(x => x.Grant == e.Grant) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(x => x.TurnNumber == e.ActualTurn && x.ActorSeat == e.Actor));
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeMouXyAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (_contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(MouXunYuAnniversaryRuntime.Bizuo)?.Program is not { } program)
            return ordinary;
        var items = ordinary.ToList();
        foreach (var ended in MouXyAllocationsDue().Where(e => e.ActualTurn == parent.TurnNumber && e.Actor == parent.OwnerSeat))
        {
            var grant = ended.Grant;
            var binding = grant.Source.OwnerSeat == parent.OwnerSeat ? "allocate-own" : "allocate-otherLiving";
            var trigger = program.Triggers.Single(t => t.Id == binding);
            var identity = $"mou-xy-bizuo:{(grant.FrameId)}:{(ended.ActualTurn)}";
            items.RemoveAll(i => i.Candidate is { } c && c.OwnerSeat == grant.Source.OwnerSeat && c.SkillId == MouXunYuAnniversaryRuntime.Bizuo && c.BindingId == binding && i.StableIdentity != identity && parent.AfterTurnEnded is null);
            if (items.Any(i => i.StableIdentity == identity))
                continue;
            var occurrence = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoGrantedEvent>().Select((e, i) => (e, i)).Single(x => x.e.Grant == grant).i;
            var candidate = new ProgramTriggerCandidate(grant.Source.OwnerSeat, MouXunYuAnniversaryRuntime.Bizuo, binding, grant.Source.SkillInstanceId, grant.GameplayHash, trigger.Priority, occurrence);
            items.Add(new(TurnEndingBoundaryItemKind.Program, trigger.Priority, identity, candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])));
        }

        return items.SequenceEqual(ordinary) ? ordinary : Array.AsReadOnly(items.ToArray());
    }

    [ProgramIssuedTrigger(330)]
    internal bool IsIssuedMouXyAllocationCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != MouXunYuAnniversaryRuntime.Bizuo || c.BindingId is not ("allocate-own" or "allocate-otherLiving") || context.Window != SkillProgramTriggerWindow.AfterTurnEnded || context.OwnerSeat != c.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not DeferredTurnEndFrame p || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(p) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p) != c || context.SourceSeat != p.OwnerSeat || context.TargetSeat != p.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || context.Facts != p.AfterTurnEnded!.Items[p.AfterTurnEnded.ItemIndex].Facts || c.BindingId != (c.OwnerSeat == p.OwnerSeat ? "allocate-own" : "allocate-otherLiving"))
            return false;
        return MouXyAllocationsDue().Any(e => e.ActualTurn == p.TurnNumber && e.Actor == p.OwnerSeat && e.Grant.Source.OwnerSeat == c.OwnerSeat && e.Grant.Source.SkillInstanceId == c.SkillInstanceId && e.Grant.GameplayHash == c.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoGrantedEvent>().Select((x, i) => (x, i)).Any(x => x.i == c.OccurrenceIndex && x.x.Grant == e.Grant));
    }

    internal MouXyBizuoGrant? MouXyAllocationLease(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not DeferredTurnEndFrame p)
            return null;
        var c = _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p);
        if (c.OwnerSeat != f.OwnerSeat || c.SkillInstanceId != f.SkillInstanceId || c.GameplayHash != f.GameplayHash || c.BindingId != f.TriggerId || !IsIssuedMouXyAllocationCandidate(c, context))
            return null;
        return MouXyAllocationsDue().Single(e => e.Grant.Source.OwnerSeat == f.OwnerSeat && e.Grant.Source.SkillInstanceId == f.SkillInstanceId && e.Grant.GameplayHash == f.GameplayHash).Grant;
    }

    internal IReadOnlyList<MouXyAllocationCard> MouXyAllocationCards(ProgramSkillFrame f)
    {
        if (f.MouXunYuAnniversary is not { Lease: { } grant } || MouXyAllocationLease(f) != grant)
            return[];
        var allocated = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyAllocatedEvent>().Where(a => a.GrantFrameId == grant.FrameId).Select(a => a.CardId).ToHashSet();
        var discard = _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyDiscardEntryRecordedEvent>().Where(e => e.Entry.GrantFrameId == grant.FrameId && e.Entry.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && !allocated.Contains(e.Entry.CardId)).OrderBy(e => e.Entry.Sequence).DistinctBy(e => e.Entry.CardId).Where(e => _cardZonesOperations._cardZones.GetLocation(e.Entry.CardId) == CardLocation.DiscardPile && !MouXyCompletedLogicalDiscard(e.Entry) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Entry.Sequence && m.CardId == e.Entry.CardId && m.TurnNumber == e.Entry.ActualTurn && m.To == CardLocation.DiscardPile && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)is null)).Select(e =>
        {
            var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(e.Entry.CardId);
            return new MouXyAllocationCard(card.Id, card.Kind, card.DisplayName, CardLocation.DiscardPile, e.Entry.Sequence);
        });
        var removed = _coreProgramEventHistoryOperations.ProgramEventHistory<CaoXianZhifouRemovedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && !allocated.Contains(e.CardId) && MouXyExactZhifouRemoval(e, grant)).Select(e =>
        {
            var card = _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(e.CardId);
            return new MouXyAllocationCard(card.Id, card.Kind, card.DisplayName, CardLocation.OutsideGame, e.Sequence, e.FrameId);
        });
        return Array.AsReadOnly(discard.Concat(removed).OrderBy(c => c.SourceSequence).DistinctBy(c => c.Id).ToArray());
    }

    internal bool MouXyCompletedLogicalDiscard(MouXyDiscardEntry entry) => _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(m => m.CardId == entry.CardId && m.CompletionSequence == entry.Sequence && m.To == CardLocation.DiscardPile && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Any(i => i.InvocationId == m.InvocationId && i.TurnNumber == entry.ActualTurn && entry.Sequence > i.SequenceBefore && entry.Sequence <= i.SequenceAfter));
    internal bool MouXyExactZhifouRemoval(CaoXianZhifouRemovedEvent e, MouXyBizuoGrant grant)
    {
        if (e.Source.SkillId != "ol:zhifou" || e.Pile.SkillId != "ol:lingxi" || e.Pile.OwnerSeat != e.Source.OwnerSeat || e.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || _cardZonesOperations._cardZones.GetLocation(e.CardId) != CardLocation.OutsideGame || e.GameplayHash != _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(e.Source.SkillId)?.Program?.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouXyBizuoStartedEvent>().Count(s => s.Grant == grant && s.ActualTurn == e.ActualTurn) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<CaoXianZhifouPaidEvent>().Count(p => p.FrameId == e.FrameId && p.Source == e.Source && p.GameplayHash == e.GameplayHash && p.ActualTurn == e.ActualTurn && p.CompletedUseId == e.CompletedUseId && p.ActionId == e.ActionId) != 1)
            return false;
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == e.CardId);
        return move is not null && move.Sequence == e.Sequence && move.TurnNumber == e.ActualTurn && move.CardKind == e.CardKind && move.From == e.Pile.Location && move.To == CardLocation.OutsideGame;
    }
}
