#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtEntries
{
    internal void AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendFixedDistanceOneEndingItems(items);
    internal RuleQueryEvaluation ApplyFixedDistanceOne(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyFixedDistanceOne(source, target, original);
    internal bool CanContinueIssuedFixedDistanceEnding(ProgramSkillFrame f) => _resolve().CanContinueIssuedFixedDistanceEnding(f);
    internal void ConsumeFixedDistanceDebt(FixedDistanceOneTurnGrant g, bool required) => _resolve().ConsumeFixedDistanceDebt(g, required);
    internal void ConsumeFixedDistanceDebtAtBegin(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().ConsumeFixedDistanceDebtAtBegin(c, context);
    internal void ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame f) => _resolve().ConsumeSkippedFixedDistanceDebt(f);
    internal bool DrainFixedDistanceDebtChildren(ProgramSkillFrame f) => _resolve().DrainFixedDistanceDebtChildren(f);
    internal void ExpireFixedDistanceOneGrants(int turn, int seat) => _resolve().ExpireFixedDistanceOneGrants(turn, seat);
    internal IReadOnlyList<PromptChoice> FixedDistanceDebtChoices(ProgramSkillFrame f) => _resolve().FixedDistanceDebtChoices(f);
    internal bool FixedDistanceDebtConsumed(FixedDistanceOneTurnGrant g) => _resolve().FixedDistanceDebtConsumed(g);
    internal IEnumerable<FixedDistanceOneTurnGrant> FixedDistanceOneGrants() => _resolve().FixedDistanceOneGrants();
    internal bool FixedDistanceTargetDamagedThisTurn(FixedDistanceOneTurnGrant g) => _resolve().FixedDistanceTargetDamagedThisTurn(g);
    internal void GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame supplied, int target) => _resolve().GrantFixedDistanceOneTurnPolicy(supplied, target);
    internal bool HasIssuedFixedDistanceDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().HasIssuedFixedDistanceDebtCandidate(c, context);
    internal bool IsFixedDistanceDebtMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => _resolve().IsFixedDistanceDebtMovement(f, e, pending);
    internal bool MatchesFixedDistanceDebt(ProgramSkillFrame f) => _resolve().MatchesFixedDistanceDebt(f);
    internal bool MatchesFixedDistanceDebt2(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun) => _resolve().MatchesFixedDistanceDebt(c, context, begun);
    internal void PublishFixedDistanceDebt(ProgramSkillFrame f) => _resolve().PublishFixedDistanceDebt(f);
    internal void ResolveFixedDistanceDebtChoice(PromptChoice choice) => _resolve().ResolveFixedDistanceDebtChoice(choice);
    internal bool ResumeFixedDistanceOneDebt(long id) => _resolve().ResumeFixedDistanceOneDebt(id);
    internal bool ReturnFixedDistanceOneDebtMovement(ProgramSkillFrame f) => _resolve().ReturnFixedDistanceOneDebtMovement(f);
    internal PromptChoice SelectAiFixedDistanceDebt(PendingDecision d) => _resolve().SelectAiFixedDistanceDebt(d);
    internal SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(ProgramSkillFrame supplied) => _resolve().SettleFixedDistanceOneEndingDebt(supplied);
    internal bool ValidFixedDistanceDebtPayment(ProgramSkillFrame f) => _resolve().ValidFixedDistanceDebtPayment(f);
    internal bool ValidFixedDistanceOneGrant(FixedDistanceOneTurnGrant g) => _resolve().ValidFixedDistanceOneGrant(g);
}
