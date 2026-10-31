#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal static bool IsDuFuRenOwnedZone(CardLocation from) => global::CardGame.Core.RuntimeSkills.RuntimeRules_du_fu_ren_ol_DuFuRenGains.IsDuFuRenOwnedZone(from);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26520)]
    internal bool QualifyDuFuRen(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => IsDuFuRenYiseTrigger(t) ? CurrentDuFuRenGains(c, context)is not null && _coreMatchParticipantsPlayersAccess._players[context.TargetSeat!.Value].IsAlive : t.Effects is [{ Op: SkillProgramEffectOp.GiveForNextActualPhases }] ? IsDuFuRenGiftContext(c, context) : true;
}
