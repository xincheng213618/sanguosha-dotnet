#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NextActualUseTargetAdjustmentProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries> getCoreGameEngineNextActualUseTargetAdjustmentsRuntime) : INextActualUseTargetAdjustmentProgramHost
{
    public void GrantNextActualUseTargetAdjustment(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineNextActualUseTargetAdjustmentsRuntime().GrantNextActualUseTargetAdjustment(frame, effect);
}
