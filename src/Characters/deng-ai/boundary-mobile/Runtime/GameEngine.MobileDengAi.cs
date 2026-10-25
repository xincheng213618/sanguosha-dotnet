namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 66410)]
    private bool OfferMobileDengAiOwnedLoss(ProgramTriggerLegalityContext c) => c.SkillId != "boundary:tuntian-mobile" ||
        c.Context.MovementBatch is { } batch && batch.Movements.Any(m =>
            m.From.OwnerSeat == c.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
            !(m.To.OwnerSeat == c.OwnerSeat && m.To.Zone is CardZoneKind.Hand or CardZoneKind.Equipment));
}
