#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterEntries
{
    internal void FinishFrozenHandRoster(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishFrozenHandRoster(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FrozenHandRosterChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FrozenHandRosterChoices(f);
    internal int FrozenHandRosterChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FrozenHandRosterChooser(f);
    internal bool FrozenHandRosterCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FrozenHandRosterCurrent(f);
    internal bool FrozenHandRosterEndingParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FrozenHandRosterEndingParentMatches(f);
    internal global::CardGame.Core.SkillProgramEffect? FrozenHandRosterPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FrozenHandRosterPausedEffect(f);
    internal bool IsFrozenHandRosterMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsFrozenHandRosterMovement(f, e, p);
    internal void PayFrozenHandRosterDraw(global::CardGame.Core.ProgramSkillFrame f, int target, string next) => _resolve().PayFrozenHandRosterDraw(f, target, next);
    internal void PublishFrozenHandRoster(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFrozenHandRoster(f);
    internal void ResolveFrozenHandRosterChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFrozenHandRosterChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveFrozenHandRosterEndingExchange(supplied);
    internal bool ResumeFrozenHandRoster(long id) => _resolve().ResumeFrozenHandRoster(id);
    internal bool ReturnFrozenHandRosterMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFrozenHandRosterMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiFrozenHandRoster(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiFrozenHandRoster(p, f);
    internal bool ValidFrozenHandRoster(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidFrozenHandRoster(f);
}
