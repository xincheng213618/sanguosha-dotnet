#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NextCardTargetAdjustmentProgramCapability(Func<RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries> getCoreGameEngineNextCardTargetAdjustmentsRuntime) : INextCardTargetAdjustmentProgramHost
{
    public void GrantNextCardTargetAdjustment(ProgramSkillFrame frame) => getCoreGameEngineNextCardTargetAdjustmentsRuntime().GrantNextCardTargetAdjustment(frame);
}
