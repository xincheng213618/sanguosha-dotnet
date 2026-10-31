#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceAgricultureOfficerRuntime
{
    // A transfer between the same owner's HE regions is not loss of ownership.
    // The original atomic batch covers actual Use/Response payment only once;
    // its later Processing cleanup cannot create another owner loss candidate.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62062)]
    internal bool OfferSourceAgricultureOfficerOwnedLoss(ProgramTriggerLegalityContext c) => c.SkillId != "shanhe:source-tuntian-959" || c.Candidate.BindingId != "off-turn-owned-loss" || c.Context.MovementBatch is { } batch && batch.Movements.Any(m => m.From.OwnerSeat == c.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !(m.To.OwnerSeat == c.OwnerSeat && m.To.Zone is CardZoneKind.Hand or CardZoneKind.Equipment));
}
