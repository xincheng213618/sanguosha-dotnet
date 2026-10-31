#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiBoundaryMobileRuntime
{
    internal const string BoundaryZfLiangyin = "boundary:liangyin-mobile", BoundaryZfKongsheng = "boundary:kongsheng-mobile", BoundaryZfAction = "boundary-zhou-fei";
    internal const string BoundaryZfStoreReason = "skill.boundary-zhou-fei.store", BoundaryZfReturnReason = "skill.boundary-zhou-fei.return", BoundaryZfDiscardReason = "skill.boundary-zhou-fei.discard";
    internal static CardConversionSource BoundaryZfSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal static bool BoundaryZfGeneralPile(CardLocation location) => location.OwnerSeat is not null && location.Zone is CardZoneKind.Authority or CardZoneKind.BuquWound or CardZoneKind.Chunlao or CardZoneKind.PublicPersistentPile or CardZoneKind.PrivatePersistentPile or CardZoneKind.PrivateReserve or CardZoneKind.PublicDeferredPile or CardZoneKind.PojunHold or CardZoneKind.PrivateTurnHold or CardZoneKind.RoundHold;
    internal static bool IsBoundaryZhouFeiMovement(ProgramTriggerCandidate candidate) => candidate.SkillId == BoundaryZfLiangyin && candidate.BindingId == "global-general-pile";
    internal IEnumerable<(int Encoded, int Index, int Direction)> BoundaryZfMovementGroups(CardMovementBatchContext batch)
    {
        var count = Math.Max(1, batch.Movements.Count);
        for (var direction = 0; direction < 2; direction++)
        {
            var selectedDirection = direction;
            var matches = batch.Movements.Select((move, index) => (move, index)).Where(item => item.move.From != item.move.To && (selectedDirection == 0 ? BoundaryZfGeneralPile(item.move.To) : BoundaryZfGeneralPile(item.move.From) && item.move.To.Zone == CardZoneKind.Hand));
            foreach (var group in matches.GroupBy(item => selectedDirection == 0 ? (item.move.To.OwnerSeat, (int? )null) : (item.move.From.OwnerSeat, item.move.To.OwnerSeat)))
            {
                var index = group.Min(item => item.index);
                yield return (direction * count + index, index, direction);
            }
        }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectBoundaryZhouFeiMovementCandidates(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin))
            yield break;
        var groups = BoundaryZfMovementGroups(batch).ToArray();
        if (groups.Length == 0)
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(IsBoundaryZhouFeiMovement))
                foreach (var group in groups)
                    yield return candidate with
                    {
                        OccurrenceIndex = group.Encoded
                    };
    }

    internal ProgramSkillWindowContext CreateBoundaryZhouFeiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var group = BoundaryZfMovementGroups(frame.Batch).Single(group => group.Encoded == candidate.OccurrenceIndex);
        var movement = frame.Batch.Movements[group.Index];
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: movement.From.OwnerSeat, TargetSeat: movement.To.OwnerSeat, Amount: 1, OccurrenceIndex: group.Encoded, MovementBatch: frame.Batch, MovementIndex: group.Index, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]));
    }

    internal bool ValidBoundaryZfMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsBoundaryZhouFeiMovement(candidate) && context.Window == SkillProgramTriggerWindow.CardsMoved && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && context.OwnerSeat == candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == batch.Id && window.Batch.Id == batch.Id && context.OccurrenceIndex == candidate.OccurrenceIndex && BoundaryZfMovementGroups(batch).Any(group => group.Encoded == candidate.OccurrenceIndex && group.Index == context.MovementIndex) && batch.Movements.All(movement => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement));
    internal bool ValidBoundaryZfDeposit(BoundaryZhouFeiStoredEvent deposit) => deposit.Source.SkillId == BoundaryZfKongsheng && deposit.Source.BindingId is "prepare-store" or "health-store" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(deposit.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(deposit.Holder) && deposit.Count > 0 && deposit.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, deposit.Holder, $"boundary-zhou-fei:{(deposit.FrameId)}") && _contentRegistryOperations._contentRegistry.GetSkill(BoundaryZfKongsheng).Program?.GameplayHash == deposit.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiStartedEvent>().Count(entry => entry.FrameId == deposit.FrameId && entry.Source == deposit.Source && entry.Hash == deposit.Hash && entry.Op == SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng && entry.Holder == deposit.Holder) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.Sequence > deposit.Before && move.Sequence <= deposit.After && move.To == deposit.Location && move.Reason.Value == BoundaryZfStoreReason) == deposit.Count;
    internal BoundaryZhouFeiStoredEvent[] BoundaryZfLiveDeposits(int holder) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiStoredEvent>().Where(deposit => deposit.Holder == holder && ValidBoundaryZfDeposit(deposit) && _cardZonesOperations._cardZones.CardsAt(deposit.Location).Count > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiDepositReturnedEvent>().Any(entry => entry.DepositId == deposit.FrameId)).OrderBy(deposit => deposit.FrameId).ToArray();
    internal BoundaryZhouFeiStoredEvent BoundaryZfDeposit(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiStoredEvent>().Single(deposit => deposit.FrameId == id && ValidBoundaryZfDeposit(deposit));
    internal IReadOnlyList<int> DiscardBoundaryZhouFeiHolderDeathPiles(DeathFrame death)
    {
        var cards = BoundaryZfLiveDeposits(death.VictimSeat).SelectMany(deposit => _cardZonesOperations._cardZones.CardsAt(deposit.Location).Select(card => (Card: card, From: deposit.Location))).ToArray();
        foreach (var item in cards)
            _corePhysicalMovementFacadesOperations.MoveCard(item.Card, item.From, CardLocation.DiscardPile, new("skill.boundary-zhou-fei.holder-death"), disclosure: CardMovementDisclosure.FaceUp);
        return Array.AsReadOnly(cards.Select(item => item.Card.Id).ToArray());
    }

    internal (int? Seen, int? Unseen) BoundaryZfRoundTargets(int owner, int round)
    {
        var seen = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhouFeiTargetEvent>().Where(entry => entry.Owner == owner && entry.Round == round).Select(entry => entry.Target).Distinct().ToArray();
        var unseen = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && !seen.Contains(player.Seat)).Select(player => player.Seat).ToArray();
        return (seen.Length == 1 && _coreMatchParticipantsPlayersAccess._players[seen[0]].IsAlive ? seen[0] : null, unseen.Length == 1 ? unseen[0] : null);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 54700)]
    internal void AppendBoundaryZfReturns(List<TurnEndingBoundaryItem> items)
    {
        items.RemoveAll(item => item.Candidate is { SkillId: BoundaryZfKongsheng, BindingId: "return-held" });
        foreach (var deposit in BoundaryZfLiveDeposits(_coreActualTurnProgressCurrentSeatTurnProgressionAccess._currentSeat).DistinctBy(deposit => (deposit.Source.OwnerSeat, deposit.Source.SkillInstanceId)))
        {
            var candidate = new ProgramTriggerCandidate(deposit.Source.OwnerSeat, BoundaryZfKongsheng, "return-held", deposit.Source.SkillInstanceId, deposit.Hash, 0);
            items.Add(new(TurnEndingBoundaryItemKind.Program, 0, $"program:{(BoundaryZfKongsheng)}:return-held:{(deposit.Source.OwnerSeat)}:{(deposit.Source.SkillInstanceId)}", candidate));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 54700)]
    internal bool IsIssuedBoundaryZf(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (IsBoundaryZhouFeiMovement(candidate))
            return ValidBoundaryZfMovement(candidate, context);
        return candidate.SkillId == BoundaryZfKongsheng && candidate.BindingId == "return-held" && context.Window == SkillProgramTriggerWindow.TurnEnding && context.SourceSeat == context.TargetSeat && context.TargetSeat is { } holder && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().Any(window => window.Id == context.ParentFrameId && window.OwnerSeat == holder) && BoundaryZfLiveDeposits(holder).Any(deposit => deposit.Source.OwnerSeat == candidate.OwnerSeat && deposit.Source.SkillInstanceId == candidate.SkillInstanceId && deposit.Hash == candidate.GameplayHash);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 700)]
    internal void CollectWindow_CollectBoundaryZhouFeiMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectBoundaryZhouFeiMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 200)]
    internal bool ExcludeOrdinaryMovement_IsBoundaryZhouFeiMovement(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsBoundaryZhouFeiMovement(candidate);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 500)]
    internal ProgramSkillWindowContext? TryCreateBoundaryZhouFeiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsBoundaryZhouFeiMovement(candidate))
            return CreateBoundaryZhouFeiMovementContext(frame, candidate);
        return null;
    }
}
