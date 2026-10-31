#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingOwnerSkillReplacementEntries
{
    internal IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _resolve().AcquireDyingOwnerReplacementSkills(frame, skillIds);
    internal bool CanContinueReplacedPaidHpContinuation(ProgramSkillFrame frame) => _resolve().CanContinueReplacedPaidHpContinuation(frame);
    internal ProgramReplacedPaidHpContinuation? CaptureDyingOwnerReplacementContinuation(ProgramSkillFrame replacement, SkillProgramEffect effect) => _resolve().CaptureDyingOwnerReplacementContinuation(replacement, effect);
    internal bool HasValidReplacedPaidHpContinuation(ProgramSkillFrame frame, ProgramExecutionPlan plan) => _resolve().HasValidReplacedPaidHpContinuation(frame, plan);
    internal bool IsDyingOwnerReplacementBinding(ProgramSkillFrame frame) => _resolve().IsDyingOwnerReplacementBinding(frame);
    internal void IssueReplacedPaidHpContinuation(ProgramReplacedPaidHpContinuation continuation) => _resolve().IssueReplacedPaidHpContinuation(continuation);
    internal (ProgramLifecycleTriggerWindowFrame Entry, DyingFrame Dying) RequireDyingOwnerReplacementContext(ProgramSkillFrame frame) => _resolve().RequireDyingOwnerReplacementContext(frame);
}
