#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHandComparisonEntries
{
    internal void AddHandComparisonEffectPublicRuleStates(global::CardGame.Core.CharacterState owner, string skillId, global::CardGame.Core.IndexedSkillProgramInstance instance, global::System.Collections.Generic.List<global::CardGame.Core.ProgramPublicRuleStateSnapshot> result) => _resolve().AddHandComparisonEffectPublicRuleStates(owner, skillId, instance, result);
    internal void AdjustPersistentHandLimit(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, int amount, string stateId) => _resolve().AdjustPersistentHandLimit(f, targetSeat, amount, stateId);
    internal void AssertHandComparison(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op) => _resolve().AssertHandComparison(f, op);
    internal bool CanActivateHandComparison(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanActivateHandComparison(owner, activation);
    internal global::CardGame.Core.SkillProgramStepOutcome CompareSelectedHandWithHpHand(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().CompareSelectedHandWithHpHand(f, effect);
    internal void ContinueHandComparisonAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueHandComparisonAfterPublicReveal(f);
    internal void FinishHandComparison(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHandComparison(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot>? GetHandComparisonPublicRuleStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().GetHandComparisonPublicRuleStates(owner, skillId);
    internal global::System.Collections.Generic.IReadOnlyList<int> GetSelfProhibitionPolicyTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().GetSelfProhibitionPolicyTargets(actor, action);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> HandComparisonHistory() => _resolve().HandComparisonHistory();
    internal int? HandComparisonPhaseLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramActivation activation, string? instanceId = default) => _resolve().HandComparisonPhaseLimit(owner, program, activation, instanceId);
    internal bool HasSelfCardTargetProhibition(int seat) => _resolve().HasSelfCardTargetProhibition(seat);
    internal bool IsImplicitSelfCardUse(global::CardGame.Core.CardKind kind) => _resolve().IsImplicitSelfCardUse(kind);
    internal bool IsSelfTargetForbiddenAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IsSelfTargetForbiddenAction(actor, kind, targets);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> PersistentHandLimitContributions(global::CardGame.Core.CharacterState owner) => _resolve().PersistentHandLimitContributions(owner);
    internal void PublishHandComparison(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHandComparison(f);
    internal void ResolveHandComparison(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveHandComparison(selected);
    internal bool SelfCardDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().SelfCardDirectedTargetProhibited(query);
}
