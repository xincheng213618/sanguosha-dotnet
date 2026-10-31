#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedDiscardRecoveryEntries
{
    internal SkillProgramStepOutcome BeginRedDiscardRecovery(ProgramSkillFrame f) => _resolve().BeginRedDiscardRecovery(f);
    internal void CaptureMovementBatchFactsCaptureTurnRedDiscardCount(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureTurnRedDiscardCount(batch, movements);
    internal void CaptureTurnRedDiscardCount(int turn, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureTurnRedDiscardCount(turn, movements);
    internal Dictionary<string, string> RedDiscardRecoveryParameters(ProgramSkillFrame f, string mode) => _resolve().RedDiscardRecoveryParameters(f, mode);
    internal IReadOnlyList<PromptChoice> RedDiscardRecoveryQuestionChoices(ProgramSkillFrame f) => _resolve().RedDiscardRecoveryQuestionChoices(f);
    internal IReadOnlyList<PromptChoice> RedDiscardRecoverySelectionChoices(ProgramSkillFrame f) => _resolve().RedDiscardRecoverySelectionChoices(f);
    internal bool RedDiscardRecoverySourceValid(ProgramSkillFrame f) => _resolve().RedDiscardRecoverySourceValid(f);
    internal void ResolveRedDiscardRecoveryChoice(PromptChoice selected) => _resolve().ResolveRedDiscardRecoveryChoice(selected);
    internal bool ResumeRedDiscardRecovery(long id) => _resolve().ResumeRedDiscardRecovery(id);
    internal PromptChoice SelectAiRedDiscardRecovery(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiRedDiscardRecovery(decision, frame);
    internal int CurrentTurnRedDiscardCount { get => _resolve().CurrentTurnRedDiscardCount; }
    internal bool UsesRedDiscardRecoveryCounter { get => _resolve().UsesRedDiscardRecoveryCounter; }
}
