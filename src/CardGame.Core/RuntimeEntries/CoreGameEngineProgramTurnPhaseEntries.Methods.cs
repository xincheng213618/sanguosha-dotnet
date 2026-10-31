#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTurnPhaseEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramVirtualCardUse(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, global::CardGame.Core.CardKind cardKind, bool ignoreDistance) => _resolve().BeginProgramVirtualCardUse(frame, targetSeat, cardKind, ignoreDistance);
    internal global::CardGame.Core.SkillProgramStepOutcome SkipProgramTurnPhases(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramTurnPhase> phases) => _resolve().SkipProgramTurnPhases(frame, phases);
}
