#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouTaiBoundaryMobileRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 58000, SkillProgramEffectOp.Draw)]
    internal bool CanRunBoundaryZhouTaiFenji(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != "boundary:fenji-mobile")
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.SourceSeat is { } ended && context.TargetSeat == ended && _coreTableSeatQueriesOperations.IsValidPlayerSeat(ended) && _coreMatchParticipantsPlayersAccess._players[ended].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ended]).Count == 0 && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().Any(parent => parent.Id == context.ParentFrameId && parent.OwnerSeat == ended && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent));
    }
}
