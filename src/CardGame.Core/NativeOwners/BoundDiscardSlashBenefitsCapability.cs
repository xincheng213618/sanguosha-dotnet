#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundDiscardSlashBenefitsCapability(Func<RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries> getCoreGameEngineBoundDiscardSlashBenefitsRuntime) : IBoundDiscardSlashBenefitsHost
{
    public SkillProgramStepOutcome DiscardBoundCardForTurnSlashBenefits(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineBoundDiscardSlashBenefitsRuntime().BeginBoundDiscardSlashBenefits(frame.Id, sourceBind);
    public void ScheduleFirstRoundGameUsageRefund(ProgramSkillFrame frame) => getCoreGameEngineBoundDiscardSlashBenefitsRuntime().ScheduleProgramFirstRoundRefund(frame);
}
