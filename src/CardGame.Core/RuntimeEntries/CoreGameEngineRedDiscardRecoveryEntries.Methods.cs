#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRedDiscardRecoveryEntries
{
    internal void AssertRedDiscardRecoveryDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertRedDiscardRecoveryDraft(f, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRedDiscardRecovery(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginRedDiscardRecovery(f);
    internal void CaptureMovementBatchFactsCaptureTurnRedDiscardCount(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureTurnRedDiscardCount(batch, movements);
    internal void CaptureTurnRedDiscardCount(int turn, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureTurnRedDiscardCount(turn, movements);
    internal global::System.Collections.Generic.Dictionary<string, string> RedDiscardRecoveryParameters(global::CardGame.Core.ProgramSkillFrame f, string mode) => _resolve().RedDiscardRecoveryParameters(f, mode);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RedDiscardRecoveryQuestionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RedDiscardRecoveryQuestionChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RedDiscardRecoverySelectionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RedDiscardRecoverySelectionChoices(f);
    internal bool RedDiscardRecoverySourceValid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RedDiscardRecoverySourceValid(f);
    internal void ResolveRedDiscardRecoveryChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveRedDiscardRecoveryChoice(selected);
    internal bool ResumeRedDiscardRecovery(long id) => _resolve().ResumeRedDiscardRecovery(id);
    internal global::CardGame.Core.PromptChoice SelectAiRedDiscardRecovery(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiRedDiscardRecovery(decision, frame);
    internal int CurrentTurnRedDiscardCount { get => _resolve().CurrentTurnRedDiscardCount; }
    internal bool UsesRedDiscardRecoveryCounter { get => _resolve().UsesRedDiscardRecoveryCounter; }
}
