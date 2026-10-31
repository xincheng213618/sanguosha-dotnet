#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingOwnerSkillReplacementEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> skillIds) => _resolve().AcquireDyingOwnerReplacementSkills(frame, skillIds);
    internal bool CanContinueReplacedPaidHpContinuation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueReplacedPaidHpContinuation(frame);
    internal global::CardGame.Core.ProgramReplacedPaidHpContinuation? CaptureDyingOwnerReplacementContinuation(global::CardGame.Core.ProgramSkillFrame replacement, global::CardGame.Core.SkillProgramEffect effect) => _resolve().CaptureDyingOwnerReplacementContinuation(replacement, effect);
    internal bool HasValidReplacedPaidHpContinuation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().HasValidReplacedPaidHpContinuation(frame, plan);
    internal bool IsDyingOwnerReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsDyingOwnerReplacementBinding(frame);
    internal void IssueReplacedPaidHpContinuation(global::CardGame.Core.ProgramReplacedPaidHpContinuation continuation) => _resolve().IssueReplacedPaidHpContinuation(continuation);
    internal (global::CardGame.Core.ProgramLifecycleTriggerWindowFrame Entry, global::CardGame.Core.DyingFrame Dying) RequireDyingOwnerReplacementContext(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireDyingOwnerReplacementContext(frame);
}
