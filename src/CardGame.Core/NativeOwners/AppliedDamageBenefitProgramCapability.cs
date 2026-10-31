#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AppliedDamageBenefitProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAppliedDamageBenefitsEntries> getCoreGameEngineAppliedDamageBenefitsRuntime) : IAppliedDamageBenefitProgramHost
{
    public SkillProgramStepOutcome ReceiveOwnerDamage(ProgramSkillFrame frame, int amount) => getCoreGameEngineAppliedDamageBenefitsRuntime().ReceiveProgramOwnerDamage(frame, amount);
    public void ConsumeDistinctTurnTarget(ProgramSkillFrame frame, string usageId) => getCoreGameEngineAppliedDamageBenefitsRuntime().ConsumeDistinctProgramTurnTarget(frame, usageId);
    public SkillProgramStepOutcome DrawOwnerAtAppliedDamage(ProgramSkillFrame frame, int amount) => getCoreGameEngineAppliedDamageBenefitsRuntime().BeginAppliedDamageBenefitDraw(frame, amount);
}
