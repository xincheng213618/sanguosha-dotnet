#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class OutsidePhaseDrawDiscardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime) : IOutsidePhaseDrawDiscardProgramHost
{
    public SkillProgramStepOutcome OutsidePhaseDrawDiscard(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().BeginOutsidePhaseDrawDiscard(f, op);
}
