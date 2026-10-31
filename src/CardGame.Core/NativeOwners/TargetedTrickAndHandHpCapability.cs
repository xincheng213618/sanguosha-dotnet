#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TargetedTrickAndHandHpCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineUnequalHandHpEndingRuntime>> getCoreGameEngineUnequalHandHpEndingRuntime, Func<RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries> getCoreGameEngineTargetedOrdinaryTrickRuntime) : ITargetedTrickAndHandHpHost
{
    public SkillProgramStepOutcome ResolveUnequalHandHpEnding(ProgramSkillFrame frame) => getCoreGameEngineUnequalHandHpEndingRuntime().Value.BeginUnequalHandHpEnding(frame);
    public SkillProgramStepOutcome AdjustTargetedOrdinaryTrick(ProgramSkillFrame frame) => getCoreGameEngineTargetedOrdinaryTrickRuntime().BeginTargetedOrdinaryTrick(frame);
}
