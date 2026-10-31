#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class TargetedTrickAndHandHpCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineUnequalHandHpEndingEntries> getCoreGameEngineUnequalHandHpEndingRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries> getCoreGameEngineTargetedOrdinaryTrickRuntime) : ITargetedTrickAndHandHpHost
{
    public SkillProgramStepOutcome ResolveUnequalHandHpEnding(ProgramSkillFrame frame) => getCoreGameEngineUnequalHandHpEndingRuntime().BeginUnequalHandHpEnding(frame);
    public SkillProgramStepOutcome AdjustTargetedOrdinaryTrick(ProgramSkillFrame frame) => getCoreGameEngineTargetedOrdinaryTrickRuntime().BeginTargetedOrdinaryTrick(frame);
}
