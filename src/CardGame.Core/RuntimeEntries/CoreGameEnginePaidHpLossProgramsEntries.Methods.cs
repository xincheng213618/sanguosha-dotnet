#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidHpLossProgramsEntries
{
    internal SkillProgramStepOutcome BeginHpLossQuantity(ProgramSkillFrame frame, string bind) => _resolve().BeginHpLossQuantity(frame, bind);
    internal bool CanContinueIssuedHpLoss(ProgramSkillFrame f) => _resolve().CanContinueIssuedHpLoss(f);
    internal SkillProgramStepOutcome DrawProgramPaidHpLoss(ProgramSkillFrame f, string bind) => _resolve().DrawProgramPaidHpLoss(f, bind);
    internal void GrantProgramPaidHpLoss(ProgramSkillFrame f, string bind, bool distance) => _resolve().GrantProgramPaidHpLoss(f, bind, distance);
    internal bool HasExactPaidHpLossGrant(ProgramSkillFrame f, ProgramPaidHpLossReceipt p, long? sequence, int index, SkillRuleQuery query, int amount) => _resolve().HasExactPaidHpLossGrant(f, p, sequence, index, query, amount);
    internal bool HasValidPaidHpLossReceipt(ProgramSkillFrame f, ProgramExecutionPlan plan) => _resolve().HasValidPaidHpLossReceipt(f, plan);
    internal void ResolveHpLossQuantity(PromptChoice choice) => _resolve().ResolveHpLossQuantity(choice);
    internal PromptChoice SelectAiHpLossQuantity(PendingDecision d) => _resolve().SelectAiHpLossQuantity(d);
}
