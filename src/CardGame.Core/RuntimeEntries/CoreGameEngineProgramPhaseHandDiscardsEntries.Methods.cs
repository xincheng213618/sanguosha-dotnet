#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<int> CollectPhaseHandLimitDiscardIds(int phaseOwnerSeat) => _resolve().CollectPhaseHandLimitDiscardIds(phaseOwnerSeat);
    internal void ResolveProgramPhaseHandDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramPhaseHandDiscardChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome RestoreProgramPhaseHandDiscards(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference chooser, global::CardGame.Core.ProgramParticipantReference phaseOwnerRef) => _resolve().RestoreProgramPhaseHandDiscards(frame, chooser, phaseOwnerRef);
    internal global::CardGame.Core.PromptChoice SelectAiProgramPhaseHandDiscardRestore(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiProgramPhaseHandDiscardRestore(decision, frame);
}
