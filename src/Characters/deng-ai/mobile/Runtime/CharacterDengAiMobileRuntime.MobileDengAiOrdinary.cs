#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDengAiMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 82900)]
    internal bool OfferMobileDengAiOrdinaryOwnedLoss(ProgramTriggerLegalityContext c) => c.SkillId != "mobile:tuntian" || c.Context.MovementBatch is { } batch && batch.Movements.Any(m => m.From.OwnerSeat == c.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !(m.To.OwnerSeat == c.OwnerSeat && m.To.Zone is CardZoneKind.Hand or CardZoneKind.Equipment));
}
