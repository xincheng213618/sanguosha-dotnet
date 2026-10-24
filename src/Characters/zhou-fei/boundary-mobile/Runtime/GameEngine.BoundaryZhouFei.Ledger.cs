namespace CardGame.Core;
public sealed partial class GameEngine
{
    private const string BoundaryZfLiangyin = "boundary:liangyin-mobile", BoundaryZfKongsheng = "boundary:kongsheng-mobile", BoundaryZfAction = "boundary-zhou-fei";
    private const string BoundaryZfStoreReason = "skill.boundary-zhou-fei.store", BoundaryZfReturnReason = "skill.boundary-zhou-fei.return", BoundaryZfDiscardReason = "skill.boundary-zhou-fei.discard";
    private static CardConversionSource BoundaryZfSource(ProgramSkillFrame frame) => new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    // These typed personal stores are on/beside a general. A destroyed entity,
    // an ordinary zone and cards carried by Wooden Ox are not general piles.
    private static bool BoundaryZfGeneralPile(CardLocation location) => location.OwnerSeat is not null && location.Zone is
        CardZoneKind.Authority or CardZoneKind.BuquWound or CardZoneKind.Chunlao or CardZoneKind.PublicPersistentPile or CardZoneKind.PrivatePersistentPile or
        CardZoneKind.PrivateReserve or CardZoneKind.PublicDeferredPile or CardZoneKind.PojunHold or CardZoneKind.PrivateTurnHold or CardZoneKind.RoundHold;
    private static bool IsBoundaryZhouFeiMovement(ProgramTriggerCandidate candidate) => candidate.SkillId == BoundaryZfLiangyin && candidate.BindingId == "global-general-pile";
    private IEnumerable<(int Encoded, int Index, int Direction)> BoundaryZfMovementGroups(CardMovementBatchContext batch)
    {
        var count = Math.Max(1, batch.Movements.Count);
        for (var direction = 0; direction < 2; direction++)
        {
            var selectedDirection = direction;
            var matches = batch.Movements.Select((move, index) => (move, index)).Where(item => item.move.From != item.move.To &&
                (selectedDirection == 0 ? BoundaryZfGeneralPile(item.move.To) : BoundaryZfGeneralPile(item.move.From) && item.move.To.Zone == CardZoneKind.Hand));
            foreach (var group in matches.GroupBy(item => selectedDirection == 0 ? (item.move.To.OwnerSeat, (int?)null) : (item.move.From.OwnerSeat, item.move.To.OwnerSeat)))
            { var index = group.Min(item => item.index); yield return (direction * count + index, index, direction); }
        }
    }
    private IEnumerable<ProgramTriggerCandidate> CollectBoundaryZhouFeiMovementCandidates(CardMovementBatchContext batch)
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin)) yield break;
        var groups = BoundaryZfMovementGroups(batch).ToArray(); if (groups.Length == 0) yield break;
        foreach (var owner in _players.Where(player => player.IsAlive))
        foreach (var candidate in CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(IsBoundaryZhouFeiMovement))
        foreach (var group in groups) yield return candidate with { OccurrenceIndex = group.Encoded };
    }
    private ProgramSkillWindowContext CreateBoundaryZhouFeiMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var group = BoundaryZfMovementGroups(frame.Batch).Single(group => group.Encoded == candidate.OccurrenceIndex);
        var movement = frame.Batch.Movements[group.Index];
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: movement.From.OwnerSeat,
            TargetSeat: movement.To.OwnerSeat, Amount: 1, OccurrenceIndex: group.Encoded, MovementBatch: frame.Batch, MovementIndex: group.Index,
            Facts: CaptureProgramTriggerFacts(_players[candidate.OwnerSeat]));
    }
    private bool ValidBoundaryZfMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsBoundaryZhouFeiMovement(candidate) &&
        context.Window == SkillProgramTriggerWindow.CardsMoved && context.MovementBatch is { } batch && context.ParentFrameId == batch.Id && context.OwnerSeat == candidate.OwnerSeat &&
        _players[candidate.OwnerSeat].IsAlive && HasRuntimeSkillInstance(_players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) &&
        _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault() is { } window && window.Id == batch.Id && window.Batch.Id == batch.Id &&
        context.OccurrenceIndex == candidate.OccurrenceIndex && BoundaryZfMovementGroups(batch).Any(group => group.Encoded == candidate.OccurrenceIndex && group.Index == context.MovementIndex) &&
        batch.Movements.All(movement => _cardMovements.Contains(movement));
    private bool ValidBoundaryZfDeposit(BoundaryZhouFeiStoredEvent deposit) => deposit.Source.SkillId == BoundaryZfKongsheng && deposit.Source.BindingId is "prepare-store" or "health-store" &&
        IsValidPlayerSeat(deposit.Source.OwnerSeat) && IsValidPlayerSeat(deposit.Holder) && deposit.Count > 0 && deposit.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, deposit.Holder, $"boundary-zhou-fei:{deposit.FrameId}") &&
        _contentRegistry.GetSkill(BoundaryZfKongsheng).Program?.GameplayHash == deposit.Hash &&
        ProgramEventHistory<BoundaryZhouFeiStartedEvent>().Count(entry => entry.FrameId == deposit.FrameId && entry.Source == deposit.Source && entry.Hash == deposit.Hash && entry.Op == SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng && entry.Holder == deposit.Holder) == 1 &&
        _cardMovements.Count(move => move.Sequence > deposit.Before && move.Sequence <= deposit.After && move.To == deposit.Location && move.Reason.Value == BoundaryZfStoreReason) == deposit.Count;
    private BoundaryZhouFeiStoredEvent[] BoundaryZfLiveDeposits(int holder) => ProgramEventHistory<BoundaryZhouFeiStoredEvent>().Where(deposit => deposit.Holder == holder && ValidBoundaryZfDeposit(deposit) &&
        _cardZones.CardsAt(deposit.Location).Count > 0 && !ProgramEventHistory<BoundaryZhouFeiDepositReturnedEvent>().Any(entry => entry.DepositId == deposit.FrameId)).OrderBy(deposit => deposit.FrameId).ToArray();
    private BoundaryZhouFeiStoredEvent BoundaryZfDeposit(long id) => ProgramEventHistory<BoundaryZhouFeiStoredEvent>().Single(deposit => deposit.FrameId == id && ValidBoundaryZfDeposit(deposit));
    private IReadOnlyList<int> DiscardBoundaryZhouFeiHolderDeathPiles(DeathFrame death)
    {
        var cards = BoundaryZfLiveDeposits(death.VictimSeat).SelectMany(deposit => _cardZones.CardsAt(deposit.Location).Select(card => (Card: card, From: deposit.Location))).ToArray();
        foreach (var item in cards) MoveCard(item.Card, item.From, CardLocation.DiscardPile, new("skill.boundary-zhou-fei.holder-death"), disclosure: CardMovementDisclosure.FaceUp);
        return Array.AsReadOnly(cards.Select(item => item.Card.Id).ToArray());
    }
    private (int? Seen, int? Unseen) BoundaryZfRoundTargets(int owner, int round)
    {
        var seen = ProgramEventHistory<BoundaryZhouFeiTargetEvent>().Where(entry => entry.Owner == owner && entry.Round == round).Select(entry => entry.Target).Distinct().ToArray();
        var unseen = _players.Where(player => player.IsAlive && !seen.Contains(player.Seat)).Select(player => player.Seat).ToArray();
        return (seen.Length == 1 && _players[seen[0]].IsAlive ? seen[0] : null, unseen.Length == 1 ? unseen[0] : null);
    }
    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 54700)]
    private void AppendBoundaryZfReturns(List<TurnEndingBoundaryItem> items)
    {
        items.RemoveAll(item => item.Candidate is { SkillId: BoundaryZfKongsheng, BindingId: "return-held" });
        foreach (var deposit in BoundaryZfLiveDeposits(_currentSeat).DistinctBy(deposit => (deposit.Source.OwnerSeat, deposit.Source.SkillInstanceId)))
        {
            var candidate = new ProgramTriggerCandidate(deposit.Source.OwnerSeat, BoundaryZfKongsheng, "return-held", deposit.Source.SkillInstanceId, deposit.Hash, 0);
            items.Add(new(TurnEndingBoundaryItemKind.Program, 0, $"program:{BoundaryZfKongsheng}:return-held:{deposit.Source.OwnerSeat}:{deposit.Source.SkillInstanceId}", candidate));
        }
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 54700)]
    private bool IsIssuedBoundaryZf(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (IsBoundaryZhouFeiMovement(candidate)) return ValidBoundaryZfMovement(candidate, context);
        return candidate.SkillId == BoundaryZfKongsheng && candidate.BindingId == "return-held" && context.Window == SkillProgramTriggerWindow.TurnEnding && context.SourceSeat == context.TargetSeat &&
            context.TargetSeat is { } holder && _resolutionStack.OfType<TurnEndingBoundaryFrame>().Any(window => window.Id == context.ParentFrameId && window.OwnerSeat == holder) &&
            BoundaryZfLiveDeposits(holder).Any(deposit => deposit.Source.OwnerSeat == candidate.OwnerSeat && deposit.Source.SkillInstanceId == candidate.SkillInstanceId && deposit.Hash == candidate.GameplayHash);
    }
}
