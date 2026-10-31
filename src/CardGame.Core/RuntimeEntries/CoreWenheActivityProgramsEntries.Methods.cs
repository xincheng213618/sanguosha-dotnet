#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreWenheActivityProgramsEntries
{
    internal bool AllowsWenheActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsWenheActivation(gate);
    internal bool AllowsWenheRuleTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsWenheRuleTrigger(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteWenheActivity(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteWenheActivity(frame, op);
    internal bool HasWenheDeadOwnerEnding(int actualOwner) => _resolve().HasWenheDeadOwnerEnding(actualOwner);
    internal bool IsWenheDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, int actualOwner) => _resolve().IsWenheDeadOwnerEndingCandidate(candidate, actualOwner);
    internal bool ProveIsWenheDeadOwnerEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().ProveIsWenheDeadOwnerEndingCandidate(candidate);
    internal bool ResumeWenheRuleFlow(long id) => _resolve().ResumeWenheRuleFlow(id);
    internal bool RetainWenheIssuedFlow(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainWenheIssuedFlow(f);
    internal void ReturnWenheRuleDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnWenheRuleDraw(f, outcome);
    internal bool TryBeginWenheRuleChildren(long id) => _resolve().TryBeginWenheRuleChildren(id);
}
