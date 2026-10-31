#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidHpLossProgramsEntries
{
    internal void AssertPaidHpLossModifiers() => _resolve().AssertPaidHpLossModifiers();
    internal void AssertPaidHpLossState(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertPaidHpLossState(f, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHpLossQuantity(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().BeginHpLossQuantity(frame, bind);
    internal bool CanContinueIssuedHpLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedHpLoss(f);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawProgramPaidHpLoss(global::CardGame.Core.ProgramSkillFrame f, string bind) => _resolve().DrawProgramPaidHpLoss(f, bind);
    internal void GrantProgramPaidHpLoss(global::CardGame.Core.ProgramSkillFrame f, string bind, bool distance) => _resolve().GrantProgramPaidHpLoss(f, bind, distance);
    internal bool HasExactPaidHpLossGrant(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramPaidHpLossReceipt p, long? sequence, int index, global::CardGame.Core.SkillRuleQuery query, int amount) => _resolve().HasExactPaidHpLossGrant(f, p, sequence, index, query, amount);
    internal bool HasValidPaidHpLossReceipt(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().HasValidPaidHpLossReceipt(f, plan);
    internal void ResolveHpLossQuantity(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHpLossQuantity(choice);
    internal global::CardGame.Core.PromptChoice SelectAiHpLossQuantity(global::CardGame.Core.PendingDecision d) => _resolve().SelectAiHpLossQuantity(d);
}
