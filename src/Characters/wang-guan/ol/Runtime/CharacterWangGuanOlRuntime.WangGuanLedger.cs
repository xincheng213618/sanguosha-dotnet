#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangGuanOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27210)]
    internal bool QualifyWangGuan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => t.Effects is not [{ Op: SkillProgramEffectOp.ResolveMiuyanCompletedUse }] || CanRunIssuedMiuyanCandidate(c, context);
}
