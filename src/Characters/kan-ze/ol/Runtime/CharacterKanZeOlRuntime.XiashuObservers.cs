#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKanZeOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 350)]
    internal bool AllowsGiveAllHandAndResolveXiashuTrigger(ProgramTriggerLegalityContext gate)
    {
        var features = gate.Features;
        var candidate = gate.Candidate;
        return !(features.HasOperation(SkillProgramEffectOp.GiveAllHandAndResolveXiashu) && !CanOfferXiashu(candidate.OwnerSeat));
    }

    internal bool CanOfferXiashu(int ownerSeat) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(ownerSeat)).Count > 0;
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 120)]
    internal bool IsXiashuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.XiashuPaid is { Stage: XiashuStage.GiftChildren or XiashuStage.ClaimChildren } && effect?.Op == SkillProgramEffectOp.GiveAllHandAndResolveXiashu && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && CanContinueIssuedKanZe(f);
}
