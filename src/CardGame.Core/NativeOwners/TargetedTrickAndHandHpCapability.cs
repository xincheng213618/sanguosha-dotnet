#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TargetedTrickAndHandHpCapability(Func<RuntimeEntries.CoreGameEngineUnequalHandHpEndingEntries> getCoreGameEngineUnequalHandHpEndingRuntime, Func<RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries> getCoreGameEngineTargetedOrdinaryTrickRuntime) : ITargetedTrickAndHandHpHost
{
    public SkillProgramStepOutcome ResolveUnequalHandHpEnding(ProgramSkillFrame frame) => getCoreGameEngineUnequalHandHpEndingRuntime().BeginUnequalHandHpEnding(frame);
    public SkillProgramStepOutcome AdjustTargetedOrdinaryTrick(ProgramSkillFrame frame) => getCoreGameEngineTargetedOrdinaryTrickRuntime().BeginTargetedOrdinaryTrick(frame);
}
