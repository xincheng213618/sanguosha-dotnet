#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOldHandDispositionEntries
{
    internal void AssertOldHandDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertOldHandDisposition(f);
    internal void FinishOldHandDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishOldHandDisposition(f);
    internal bool IsOldHandDispositionMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsOldHandDispositionMovement(f, e, p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OldHandChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OldHandChoices(f);
    internal bool OldHandDispositionCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OldHandDispositionCurrent(f);
    internal int[] OldHandDispositionIds(int owner, int turn) => _resolve().OldHandDispositionIds(owner, turn);
    internal global::CardGame.Core.SkillProgramEffect? OldHandPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OldHandPausedEffect(f);
    internal void PayOldHandDisposition(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, string next, int? target) => _resolve().PayOldHandDisposition(f, ids, destination, next, target);
    internal void PublishOldHandDisposition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOldHandDisposition(f);
    internal int[] RemainingOldHandIds(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RemainingOldHandIds(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ReplaceDiscardWithOldHandDisposition(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReplaceDiscardWithOldHandDisposition(supplied);
    internal void ResolveOldHandDisposition(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveOldHandDisposition(selected);
    internal bool ResumeOldHandDisposition(long id) => _resolve().ResumeOldHandDisposition(id);
    internal bool ReturnOldHandDispositionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOldHandDispositionMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiOldHandDisposition(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiOldHandDisposition(decision, f);
}
