#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OrderedPrintedSkillLossProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossEntries> getCoreGameEngineOrderedPrintedSkillLossRuntime) : IOrderedPrintedSkillLossProgramHost
{
    public SkillProgramStepOutcome ExecuteOrderedPrintedSkillLoss(ProgramSkillFrame frame) => getCoreGameEngineOrderedPrintedSkillLossRuntime().ExecuteOrderedPrintedSkillLoss(frame);
}
