#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengBoundaryMobileRuntime
{
    // "Game player count" is the fixed roster, including dead seats. The shared
    // preparation comparison supplies the owner's actual hand count and HP.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 60700)]
    internal bool QualifyBoundaryLuMengMobileQinxue(ProgramTriggerLegalityContext c) => c.SkillId != "boundary:qinxue-mobile" || c.Context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.Owner.HasHpValue && _corePhysicalOwnedCardQueriesOperations.GetHand(c.Owner).Count - c.Owner.Hp >= (_coreMatchParticipantsPlayersAccess._players.Count >= 7 ? 2 : 3);
}
