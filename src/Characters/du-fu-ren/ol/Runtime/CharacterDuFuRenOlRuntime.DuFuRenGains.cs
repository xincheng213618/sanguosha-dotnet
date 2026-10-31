#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal static bool IsDuFuRenOwnedZone(CardLocation from) => RuntimeSkills.RuntimeRules_du_fu_ren_ol_DuFuRenGains.IsDuFuRenOwnedZone(from);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26520)]
    internal bool QualifyDuFuRen(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => IsDuFuRenYiseTrigger(t) ? CurrentDuFuRenGains(c, context)is not null && _coreMatchParticipantsPlayersAccess._players[context.TargetSeat!.Value].IsAlive : t.Effects is [{ Op: SkillProgramEffectOp.GiveForNextActualPhases }] ? IsDuFuRenGiftContext(c, context) : true;
}
