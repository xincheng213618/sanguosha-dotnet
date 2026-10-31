#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOldHandDispositionEntries
{
    internal void FinishOldHandDisposition(ProgramSkillFrame f) => _resolve().FinishOldHandDisposition(f);
    internal bool IsOldHandDispositionMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsOldHandDispositionMovement(f, e, p);
    internal IReadOnlyList<PromptChoice> OldHandChoices(ProgramSkillFrame f) => _resolve().OldHandChoices(f);
    internal bool OldHandDispositionCurrent(ProgramSkillFrame f) => _resolve().OldHandDispositionCurrent(f);
    internal int[] OldHandDispositionIds(int owner, int turn) => _resolve().OldHandDispositionIds(owner, turn);
    internal SkillProgramEffect? OldHandPausedEffect(ProgramSkillFrame f) => _resolve().OldHandPausedEffect(f);
    internal void PayOldHandDisposition(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, string next, int? target) => _resolve().PayOldHandDisposition(f, ids, destination, next, target);
    internal void PublishOldHandDisposition(ProgramSkillFrame f) => _resolve().PublishOldHandDisposition(f);
    internal int[] RemainingOldHandIds(ProgramSkillFrame f) => _resolve().RemainingOldHandIds(f);
    internal SkillProgramStepOutcome ReplaceDiscardWithOldHandDisposition(ProgramSkillFrame supplied) => _resolve().ReplaceDiscardWithOldHandDisposition(supplied);
    internal void ResolveOldHandDisposition(PromptChoice selected) => _resolve().ResolveOldHandDisposition(selected);
    internal bool ResumeOldHandDisposition(long id) => _resolve().ResumeOldHandDisposition(id);
    internal bool ReturnOldHandDispositionMovement(ProgramSkillFrame f) => _resolve().ReturnOldHandDispositionMovement(f);
    internal PromptChoice SelectAiOldHandDisposition(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiOldHandDisposition(decision, f);
}
