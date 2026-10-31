#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OutsidePhaseDrawDiscardProgramCapability(Func<RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IOutsidePhaseDrawDiscardProgramHost
{
    public SkillProgramStepOutcome OutsidePhaseDrawDiscard(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().BeginOutsidePhaseDrawDiscard(f, op);
}
