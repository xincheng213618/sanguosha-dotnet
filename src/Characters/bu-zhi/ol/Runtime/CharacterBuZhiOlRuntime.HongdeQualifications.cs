#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuZhiOlRuntime
{
    internal bool BuZhiMovementQualifies(CardMovementBatchContext batch, int owner)
    {
        var hand = CardLocation.Hand(owner);
        // A source-side processing trip is still a real loss. Returning the same
        // used card to its original hand is not a new acquisition.
        return batch.NativeDrawInvocation is { } draw && draw.LastBatchId == batch.Id && draw.ParentFrameId == batch.ParentFrameId && draw.ActualTurnNumber == batch.TurnNumber && draw.OwnerSeat == owner && draw.ActualCount >= 2 && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw) || batch.NativeBuZhiDiscardInvocation is { } discard && discard.LastBatchId == batch.Id && discard.ParentFrameId == batch.ParentFrameId && discard.ActualTurnNumber == batch.TurnNumber && discard.Source.OwnerSeat == owner && discard.ActualCount >= 2 && ValidNativeBuZhiDiscardInvocation(discard) || batch.Movements.Count(m => m.From.OwnerSeat == owner && m.From != m.To) >= 2 || batch.Movements.Count(m => m.To == hand && _coreActualHandGainProgramsOperations.ActualHandGainOriginalSource(m) != hand) >= 2;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 230, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveBuZhiMovement(SkillProgramTrigger trigger, CardMovementBatchContext batch, CharacterState owner) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.OfferNativeBatchHandGainOrCardLossBenefit) ? BuZhiMovementQualifies(batch, owner.Seat) : null;
}
