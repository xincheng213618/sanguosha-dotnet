namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Native declaration payment has already removed a hand material when it
    // re-enters target validation. Count that same paid hand entity once.
    private int BoundaryXuHuangSupplyHandCount(ProgramCardDistanceQuery q)
    {
        var count = GetHand(q.Actor).Count;
        foreach (var id in q.PhysicalCardIds ?? [])
        {
            if (_cardZones.GetLocation(id) != CardLocation.Processing) continue;
            var movement = _cardMovements.LastOrDefault(m => m.CardId == id);
            if (movement is { From.Zone: CardZoneKind.Hand } && movement.From.OwnerSeat == q.Actor.Seat &&
                movement.To == CardLocation.Processing && movement.Reason == CardMoveReasons.Use) count++;
        }
        return count;
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 65610)]
    private bool BoundaryXuHuangMobileSupplyDistance(ProgramCardDistanceQuery q) =>
        q.Kind == CardKind.SupplyShortage && q.Actor.IsAlive && q.Target.IsAlive && q.Actor.Seat != q.Target.Seat &&
        HasProgramSkill(q.Actor, "boundary:duanliang-mobile") && GetHand(q.Target).Count >= BoundaryXuHuangSupplyHandCount(q);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 65610)]
    private bool BoundaryXuHuangMobileOtherSkippedDraw(ProgramTriggerLegalityContext c) =>
        c.SkillId != "boundary:jiezi-mobile" || c.Context is { Window: SkillProgramTriggerWindow.DrawPhaseSkipped, TargetSeat: { } target } &&
        target != c.Owner.Seat;
}
