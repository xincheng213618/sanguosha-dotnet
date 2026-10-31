#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouDunBoundaryMobileRuntime
{
    internal bool ValidBoundaryXiahouDunDeposit(BoundaryXiahouDunStoredEvent d) => d.Source.SkillId == BoundaryXiahouDunQingjian && d.Source.BindingId == "store-after-gain" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(d.Source.OwnerSeat) && d.Count > 0 && d.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, d.Source.OwnerSeat, $"boundary-xiahou-dun:{(d.FrameId)}") && _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program?.GameplayHash == d.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == d.FrameId && e.SkillId == d.Source.SkillId && e.SkillInstanceId == d.Source.SkillInstanceId && e.OwnerSeat == d.Source.OwnerSeat && e.BindingId == d.Source.BindingId && e.Window == SkillProgramTriggerWindow.CardsGained) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > d.Before && m.Sequence <= d.After && m.From == CardLocation.Hand(d.Source.OwnerSeat) && m.To == d.Location && m.Reason.Value == BoundaryXiahouDunStoreReason) == d.Count;
    internal BoundaryXiahouDunStoredEvent[] BoundaryXiahouDunDeposits(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryXiahouDunStoredEvent>().Where(d => d.Source.OwnerSeat == owner && ValidBoundaryXiahouDunDeposit(d) && _cardZonesOperations._cardZones.CardsAt(d.Location).Count > 0).OrderBy(d => d.FrameId).ToArray();
    internal IEnumerable<BoundaryXiahouDunStoredEvent> BoundaryXiahouDunDueDeposits(int owner, long endingId) => BoundaryXiahouDunDeposits(owner).Where(d => d.FrameId < endingId);
    internal BoundaryXiahouDunStoredEvent BoundaryXiahouDunDeposit(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryXiahouDunStoredEvent>().Single(d => d.FrameId == id && ValidBoundaryXiahouDunDeposit(d));
    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 60010)]
    internal void AppendBoundaryXiahouDunEnding(List<TurnEndingBoundaryItem> items)
    {
        items.RemoveAll(item => item.Candidate is { SkillId: BoundaryXiahouDunQingjian, BindingId: "distribute-at-ending" });
        foreach (var d in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => BoundaryXiahouDunDeposits(p.Seat)).DistinctBy(d => (d.Source.OwnerSeat, d.Source.SkillInstanceId)))
        {
            var c = new ProgramTriggerCandidate(d.Source.OwnerSeat, BoundaryXiahouDunQingjian, "distribute-at-ending", d.Source.SkillInstanceId, d.Hash, 0);
            items.Add(new(TurnEndingBoundaryItemKind.Program, 0, $"program:{(BoundaryXiahouDunQingjian)}:distribute:{(d.Source.OwnerSeat)}:{(d.Source.SkillInstanceId)}", c));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 60010)]
    internal bool IssuedBoundaryXiahouDun(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryXiahouDunQingjian && c.BindingId == "distribute-at-ending" && context.Window == SkillProgramTriggerWindow.TurnEnding && context.SourceSeat == context.TargetSeat && context.TargetSeat is { } turnOwner && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().Any(w => w.Id == context.ParentFrameId && w.OwnerSeat == turnOwner) && BoundaryXiahouDunDueDeposits(c.OwnerSeat, context.ParentFrameId).Any(d => d.Source.SkillInstanceId == c.SkillInstanceId && d.Hash == c.GameplayHash);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 60010)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 60010)]
    internal void DiscardBoundaryXiahouDunDeathPiles()
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian) || !_cardZonesOperations._cardZones.HasPrivatePersistentCards())
            return;
        foreach (var d in _coreMatchParticipantsPlayersAccess._players.Where(p => !p.IsAlive).SelectMany(p => BoundaryXiahouDunDeposits(p.Seat)).ToArray())
            foreach (var card in _cardZonesOperations._cardZones.CardsAt(d.Location).ToArray())
                _corePhysicalMovementFacadesOperations.MoveCard(card, d.Location, CardLocation.DiscardPile, new("skill.boundary-xiahou-dun.qingjian.owner-death"), disclosure: CardMovementDisclosure.FaceUp);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateWhirlPiles, 60010)]
    internal IReadOnlyList<PrivateWhirlPileSnapshot>? BoundaryXiahouDunPileViews(IReadOnlyList<PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll)
    {
        var cards = BoundaryXiahouDunDeposits(owner).SelectMany(d => _cardZonesOperations._cardZones.CardsAt(d.Location)).ToArray();
        return cards.Length == 0 ? existing : Array.AsReadOnly((existing ?? []).Append(new PrivateWhirlPileSnapshot { SkillId = BoundaryXiahouDunQingjian, PileName = "清俭", Count = cards.Length, Cards = owner == viewer || revealAll ? Array.AsReadOnly(cards.Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray()) : null }).ToArray());
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 60010)]
    internal IEnumerable<CardSnapshot> BoundaryXiahouDunPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.Distributing }).SelectMany(f => BoundaryXiahouDunHeld(f).Select(_coreSnapshotValueProjectionOperations.ToSnapshot));
}
