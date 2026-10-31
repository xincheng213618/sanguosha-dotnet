#nullable enable
using System.Collections.ObjectModel;
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PhaseHandSeizureProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime) : IPhaseHandSeizureProgramHost
{
    public SkillProgramStepOutcome DiscardTurnOverAndTakeHand(ProgramSkillFrame frame, string continuation) => getCoreGameEnginePhaseHandSeizureRuntime().BeginPhaseHandSeizure(frame, continuation);
    public SkillProgramStepOutcome ReturnIssuedPhaseHandDebt(ProgramSkillFrame frame) => getCoreGameEnginePhaseHandSeizureRuntime().BeginPhaseHandDebtReturn(frame);
}
