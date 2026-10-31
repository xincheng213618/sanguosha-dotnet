#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageBenefitsEntries
{
    internal SkillProgram AppliedDamageProgramDefinition(string skillId) => _resolve().AppliedDamageProgramDefinition(skillId);
    internal SkillProgramStepOutcome BeginAppliedDamageBenefitDraw(ProgramSkillFrame frame, int amount) => _resolve().BeginAppliedDamageBenefitDraw(frame, amount);
    internal bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _resolve().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    internal void ConsumeDistinctProgramTurnTarget(ProgramSkillFrame frame, string usageId) => _resolve().ConsumeDistinctProgramTurnTarget(frame, usageId);
    internal string? DistinctTurnTargetUsage(SkillProgramActivation activation) => _resolve().DistinctTurnTargetUsage(activation);
    internal SkillProgramStepOutcome ReceiveProgramOwnerDamage(ProgramSkillFrame frame, int amount) => _resolve().ReceiveProgramOwnerDamage(frame, amount);
}
