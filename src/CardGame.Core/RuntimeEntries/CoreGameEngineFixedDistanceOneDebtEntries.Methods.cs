#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtEntries
{
    internal void AppendFixedDistanceOneEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendFixedDistanceOneEndingItems(items);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyFixedDistanceOne(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.RuleQueryEvaluation original) => _resolve().ApplyFixedDistanceOne(source, target, original);
    internal bool CanContinueIssuedFixedDistanceEnding(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedFixedDistanceEnding(f);
    internal void ConsumeFixedDistanceDebt(global::CardGame.Core.FixedDistanceOneTurnGrant g, bool required) => _resolve().ConsumeFixedDistanceDebt(g, required);
    internal void ConsumeFixedDistanceDebtAtBegin(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ConsumeFixedDistanceDebtAtBegin(c, context);
    internal void ConsumeSkippedFixedDistanceDebt(global::CardGame.Core.TurnEndingBoundaryFrame f) => _resolve().ConsumeSkippedFixedDistanceDebt(f);
    internal bool DrainFixedDistanceDebtChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainFixedDistanceDebtChildren(f);
    internal void ExpireFixedDistanceOneGrants(int turn, int seat) => _resolve().ExpireFixedDistanceOneGrants(turn, seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FixedDistanceDebtChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FixedDistanceDebtChoices(f);
    internal bool FixedDistanceDebtConsumed(global::CardGame.Core.FixedDistanceOneTurnGrant g) => _resolve().FixedDistanceDebtConsumed(g);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.FixedDistanceOneTurnGrant> FixedDistanceOneGrants() => _resolve().FixedDistanceOneGrants();
    internal bool FixedDistanceTargetDamagedThisTurn(global::CardGame.Core.FixedDistanceOneTurnGrant g) => _resolve().FixedDistanceTargetDamagedThisTurn(g);
    internal void GrantFixedDistanceOneTurnPolicy(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().GrantFixedDistanceOneTurnPolicy(supplied, target);
    internal bool HasIssuedFixedDistanceDebtCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedFixedDistanceDebtCandidate(c, context);
    internal bool IsFixedDistanceDebtMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsFixedDistanceDebtMovement(f, e, pending);
    internal bool MatchesFixedDistanceDebt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesFixedDistanceDebt(f);
    internal bool MatchesFixedDistanceDebt2(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, bool begun) => _resolve().MatchesFixedDistanceDebt(c, context, begun);
    internal void PublishFixedDistanceDebt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishFixedDistanceDebt(f);
    internal void ResolveFixedDistanceDebtChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveFixedDistanceDebtChoice(choice);
    internal bool ResumeFixedDistanceOneDebt(long id) => _resolve().ResumeFixedDistanceOneDebt(id);
    internal bool ReturnFixedDistanceOneDebtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnFixedDistanceOneDebtMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiFixedDistanceDebt(global::CardGame.Core.PendingDecision d) => _resolve().SelectAiFixedDistanceDebt(d);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SettleFixedDistanceOneEndingDebt(supplied);
    internal bool ValidFixedDistanceDebtPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidFixedDistanceDebtPayment(f);
    internal bool ValidFixedDistanceOneGrant(global::CardGame.Core.FixedDistanceOneTurnGrant g) => _resolve().ValidFixedDistanceOneGrant(g);
}
