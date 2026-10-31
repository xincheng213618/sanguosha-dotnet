#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsEntries
{
    internal IReadOnlyList<int> CollectPhaseHandLimitDiscardIds(int phaseOwnerSeat) => _resolve().CollectPhaseHandLimitDiscardIds(phaseOwnerSeat);
    internal void ResolveProgramPhaseHandDiscardChoice(PromptChoice selected) => _resolve().ResolveProgramPhaseHandDiscardChoice(selected);
    internal SkillProgramStepOutcome RestoreProgramPhaseHandDiscards(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference phaseOwnerRef) => _resolve().RestoreProgramPhaseHandDiscards(frame, chooser, phaseOwnerRef);
    internal PromptChoice SelectAiProgramPhaseHandDiscardRestore(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiProgramPhaseHandDiscardRestore(decision, frame);
}
