#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreWenheActivityProgramsEntries
{
    internal bool AllowsWenheActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsWenheActivation(gate);
    internal bool AllowsWenheRuleTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsWenheRuleTrigger(gate);
    internal SkillProgramStepOutcome ExecuteWenheActivity(ProgramSkillFrame frame, SkillProgramEffectOp op) => _resolve().ExecuteWenheActivity(frame, op);
    internal bool HasWenheDeadOwnerEnding(int actualOwner) => _resolve().HasWenheDeadOwnerEnding(actualOwner);
    internal bool IsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate, int actualOwner) => _resolve().IsWenheDeadOwnerEndingCandidate(candidate, actualOwner);
    internal bool ProveIsWenheDeadOwnerEndingCandidate(ProgramTriggerCandidate candidate) => _resolve().ProveIsWenheDeadOwnerEndingCandidate(candidate);
    internal bool ResumeWenheRuleFlow(long id) => _resolve().ResumeWenheRuleFlow(id);
    internal bool RetainWenheIssuedFlow(ProgramSkillFrame f) => _resolve().RetainWenheIssuedFlow(f);
    internal void ReturnWenheRuleDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnWenheRuleDraw(f, outcome);
    internal bool TryBeginWenheRuleChildren(long id) => _resolve().TryBeginWenheRuleChildren(id);
}
