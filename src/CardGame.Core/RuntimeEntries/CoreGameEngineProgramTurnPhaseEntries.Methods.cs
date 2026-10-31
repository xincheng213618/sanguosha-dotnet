#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTurnPhaseEntries
{
    internal SkillProgramStepOutcome BeginProgramVirtualCardUse(ProgramSkillFrame frame, int targetSeat, CardKind cardKind, bool ignoreDistance) => _resolve().BeginProgramVirtualCardUse(frame, targetSeat, cardKind, ignoreDistance);
    internal SkillProgramStepOutcome SkipProgramTurnPhases(ProgramSkillFrame frame, IReadOnlyList<SkillProgramTurnPhase> phases) => _resolve().SkipProgramTurnPhases(frame, phases);
}
