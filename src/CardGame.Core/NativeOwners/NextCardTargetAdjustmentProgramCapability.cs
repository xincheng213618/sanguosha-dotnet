#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NextCardTargetAdjustmentProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries> getCoreGameEngineNextCardTargetAdjustmentsRuntime) : INextCardTargetAdjustmentProgramHost
{
    public void GrantNextCardTargetAdjustment(ProgramSkillFrame frame) => getCoreGameEngineNextCardTargetAdjustmentsRuntime().GrantNextCardTargetAdjustment(frame);
}
