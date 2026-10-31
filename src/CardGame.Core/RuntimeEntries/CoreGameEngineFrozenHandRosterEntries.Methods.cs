#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterEntries
{
    internal void FinishFrozenHandRoster(ProgramSkillFrame f) => _resolve().FinishFrozenHandRoster(f);
    internal IReadOnlyList<PromptChoice> FrozenHandRosterChoices(ProgramSkillFrame f) => _resolve().FrozenHandRosterChoices(f);
    internal int FrozenHandRosterChooser(ProgramSkillFrame f) => _resolve().FrozenHandRosterChooser(f);
    internal bool FrozenHandRosterCurrent(ProgramSkillFrame f) => _resolve().FrozenHandRosterCurrent(f);
    internal bool FrozenHandRosterEndingParentMatches(ProgramSkillFrame f) => _resolve().FrozenHandRosterEndingParentMatches(f);
    internal SkillProgramEffect? FrozenHandRosterPausedEffect(ProgramSkillFrame f) => _resolve().FrozenHandRosterPausedEffect(f);
    internal bool IsFrozenHandRosterMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsFrozenHandRosterMovement(f, e, p);
    internal void PayFrozenHandRosterDraw(ProgramSkillFrame f, int target, string next) => _resolve().PayFrozenHandRosterDraw(f, target, next);
    internal void PublishFrozenHandRoster(ProgramSkillFrame f) => _resolve().PublishFrozenHandRoster(f);
    internal void ResolveFrozenHandRosterChoice(PromptChoice selected) => _resolve().ResolveFrozenHandRosterChoice(selected);
    internal SkillProgramStepOutcome ResolveFrozenHandRosterEndingExchange(ProgramSkillFrame supplied) => _resolve().ResolveFrozenHandRosterEndingExchange(supplied);
    internal bool ResumeFrozenHandRoster(long id) => _resolve().ResumeFrozenHandRoster(id);
    internal bool ReturnFrozenHandRosterMovement(ProgramSkillFrame f) => _resolve().ReturnFrozenHandRosterMovement(f);
    internal PromptChoice SelectAiFrozenHandRoster(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiFrozenHandRoster(p, f);
    internal bool ValidFrozenHandRoster(ProgramSkillFrame f) => _resolve().ValidFrozenHandRoster(f);
}
