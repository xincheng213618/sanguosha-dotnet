#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuHuangBoundaryMobileRuntime
{
    internal int BoundaryXuHuangSupplyHandCount(ProgramCardDistanceQuery q)
    {
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(q.Actor).Count;
        foreach (var id in q.PhysicalCardIds ?? [])
        {
            if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing)
                continue;
            var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == id);
            if (movement is { From.Zone: CardZoneKind.Hand } && movement.From.OwnerSeat == q.Actor.Seat && movement.To == CardLocation.Processing && movement.Reason == CardMoveReasons.Use)
                count++;
        }

        return count;
    }

    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 65610)]
    internal bool BoundaryXuHuangMobileSupplyDistance(ProgramCardDistanceQuery q) => q.Kind == CardKind.SupplyShortage && q.Actor.IsAlive && q.Target.IsAlive && q.Actor.Seat != q.Target.Seat && _coreNativeSkillContextQueriesOperations.HasProgramSkill(q.Actor, "boundary:duanliang-mobile") && _corePhysicalOwnedCardQueriesOperations.GetHand(q.Target).Count >= BoundaryXuHuangSupplyHandCount(q);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 65610)]
    internal bool BoundaryXuHuangMobileOtherSkippedDraw(ProgramTriggerLegalityContext c) => c.SkillId != "boundary:jiezi-mobile" || c.Context is { Window: SkillProgramTriggerWindow.DrawPhaseSkipped, TargetSeat: { } target } && target != c.Owner.Seat;
}
