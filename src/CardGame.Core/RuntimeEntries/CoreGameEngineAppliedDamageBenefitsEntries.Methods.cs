#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageBenefitsEntries
{
    internal global::CardGame.Core.SkillProgram AppliedDamageProgramDefinition(string skillId) => _resolve().AppliedDamageProgramDefinition(skillId);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAppliedDamageBenefitDraw(global::CardGame.Core.ProgramSkillFrame frame, int amount) => _resolve().BeginAppliedDamageBenefitDraw(frame, amount);
    internal bool CanActivateDistinctTurnTarget(int ownerSeat, string skillId, string usageId, int targetSeat) => _resolve().CanActivateDistinctTurnTarget(ownerSeat, skillId, usageId, targetSeat);
    internal void ConsumeDistinctProgramTurnTarget(global::CardGame.Core.ProgramSkillFrame frame, string usageId) => _resolve().ConsumeDistinctProgramTurnTarget(frame, usageId);
    internal string? DistinctTurnTargetUsage(global::CardGame.Core.SkillProgramActivation activation) => _resolve().DistinctTurnTargetUsage(activation);
    internal global::CardGame.Core.SkillProgramStepOutcome ReceiveProgramOwnerDamage(global::CardGame.Core.ProgramSkillFrame frame, int amount) => _resolve().ReceiveProgramOwnerDamage(frame, amount);
}
