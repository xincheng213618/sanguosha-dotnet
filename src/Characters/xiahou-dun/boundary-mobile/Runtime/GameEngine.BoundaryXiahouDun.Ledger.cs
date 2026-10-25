namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool ValidBoundaryXiahouDunDeposit(BoundaryXiahouDunStoredEvent d) => d.Source.SkillId == BoundaryXiahouDunQingjian && d.Source.BindingId == "store-after-gain" &&
        IsValidPlayerSeat(d.Source.OwnerSeat) && d.Count > 0 && d.Location == new CardLocation(CardZoneKind.PrivatePersistentPile, d.Source.OwnerSeat, $"boundary-xiahou-dun:{d.FrameId}") &&
        _contentRegistry.GetSkill(d.Source.SkillId).Program?.GameplayHash == d.Hash &&
        ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == d.FrameId && e.SkillId == d.Source.SkillId && e.SkillInstanceId == d.Source.SkillInstanceId &&
            e.OwnerSeat == d.Source.OwnerSeat && e.BindingId == d.Source.BindingId && e.Window == SkillProgramTriggerWindow.CardsGained) == 1 &&
        _cardMovements.Count(m => m.Sequence > d.Before && m.Sequence <= d.After && m.From == CardLocation.Hand(d.Source.OwnerSeat) && m.To == d.Location && m.Reason.Value == BoundaryXiahouDunStoreReason) == d.Count;
    private BoundaryXiahouDunStoredEvent[] BoundaryXiahouDunDeposits(int owner) => ProgramEventHistory<BoundaryXiahouDunStoredEvent>()
        .Where(d => d.Source.OwnerSeat == owner && ValidBoundaryXiahouDunDeposit(d) && _cardZones.CardsAt(d.Location).Count > 0).OrderBy(d => d.FrameId).ToArray();
    // Both identifiers come from the one monotonic resolution sequence. A store
    // created inside an existing ending belongs to a later ending, even when an
    // older deposit already put this skill on the current ending's item list.
    private IEnumerable<BoundaryXiahouDunStoredEvent> BoundaryXiahouDunDueDeposits(int owner, long endingId) => BoundaryXiahouDunDeposits(owner).Where(d => d.FrameId < endingId);
    private BoundaryXiahouDunStoredEvent BoundaryXiahouDunDeposit(long id) => ProgramEventHistory<BoundaryXiahouDunStoredEvent>().Single(d => d.FrameId == id && ValidBoundaryXiahouDunDeposit(d));

    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 60010)]
    private void AppendBoundaryXiahouDunEnding(List<TurnEndingBoundaryItem> items)
    {
        items.RemoveAll(item => item.Candidate is { SkillId: BoundaryXiahouDunQingjian, BindingId: "distribute-at-ending" });
        foreach (var d in _players.Where(p => p.IsAlive).SelectMany(p => BoundaryXiahouDunDeposits(p.Seat)).DistinctBy(d => (d.Source.OwnerSeat, d.Source.SkillInstanceId)))
        {
            var c = new ProgramTriggerCandidate(d.Source.OwnerSeat, BoundaryXiahouDunQingjian, "distribute-at-ending", d.Source.SkillInstanceId, d.Hash, 0);
            items.Add(new(TurnEndingBoundaryItemKind.Program, 0, $"program:{BoundaryXiahouDunQingjian}:distribute:{d.Source.OwnerSeat}:{d.Source.SkillInstanceId}", c));
        }
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 60010)]
    private bool IssuedBoundaryXiahouDun(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryXiahouDunQingjian && c.BindingId == "distribute-at-ending" &&
        context.Window == SkillProgramTriggerWindow.TurnEnding && context.SourceSeat == context.TargetSeat && context.TargetSeat is { } turnOwner &&
        _resolutionStack.OfType<TurnEndingBoundaryFrame>().Any(w => w.Id == context.ParentFrameId && w.OwnerSeat == turnOwner) &&
        BoundaryXiahouDunDueDeposits(c.OwnerSeat, context.ParentFrameId).Any(d => d.Source.SkillInstanceId == c.SkillInstanceId && d.Hash == c.GameplayHash);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 60010)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 60010)]
    private void DiscardBoundaryXiahouDunDeathPiles()
    {
        if (!_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.StoreBoundaryXiahouDunQingjian) || !_cardZones.HasPrivatePersistentCards()) return;
        foreach (var d in _players.Where(p => !p.IsAlive).SelectMany(p => BoundaryXiahouDunDeposits(p.Seat)).ToArray())
            foreach (var card in _cardZones.CardsAt(d.Location).ToArray()) MoveCard(card, d.Location, CardLocation.DiscardPile, new("skill.boundary-xiahou-dun.qingjian.owner-death"), disclosure: CardMovementDisclosure.FaceUp);
    }
    [ProgramViewContributor(ProgramViewContribution.PrivateWhirlPiles, 60010)]
    private IReadOnlyList<PrivateWhirlPileSnapshot>? BoundaryXiahouDunPileViews(IReadOnlyList<PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll)
    {
        var cards = BoundaryXiahouDunDeposits(owner).SelectMany(d => _cardZones.CardsAt(d.Location)).ToArray();
        return cards.Length == 0 ? existing : Array.AsReadOnly((existing ?? []).Append(new PrivateWhirlPileSnapshot { SkillId = BoundaryXiahouDunQingjian, PileName = "清俭", Count = cards.Length,
            Cards = owner == viewer || revealAll ? Array.AsReadOnly(cards.Select(ToSnapshot).ToArray()) : null }).ToArray());
    }
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 60010)]
    private IEnumerable<CardSnapshot> BoundaryXiahouDunPrivateCards(int viewer) => _resolutionStack.OfType<ProgramSkillFrame>()
        .Where(f => f.OwnerSeat == viewer && f.BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.Distributing }).SelectMany(f => BoundaryXiahouDunHeld(f).Select(ToSnapshot));
}
