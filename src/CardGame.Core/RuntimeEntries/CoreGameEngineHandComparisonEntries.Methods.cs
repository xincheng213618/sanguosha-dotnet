#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHandComparisonEntries
{
    internal void AddHandComparisonEffectPublicRuleStates(CharacterState owner, string skillId, IndexedSkillProgramInstance instance, List<ProgramPublicRuleStateSnapshot> result) => _resolve().AddHandComparisonEffectPublicRuleStates(owner, skillId, instance, result);
    internal void AdjustPersistentHandLimit(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => _resolve().AdjustPersistentHandLimit(f, targetSeat, amount, stateId);
    internal void AssertHandComparison(ProgramSkillFrame f, SkillProgramEffect? op) => _resolve().AssertHandComparison(f, op);
    internal bool CanActivateHandComparison(CharacterState owner, SkillProgramActivation activation) => _resolve().CanActivateHandComparison(owner, activation);
    internal SkillProgramStepOutcome CompareSelectedHandWithHpHand(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().CompareSelectedHandWithHpHand(f, effect);
    internal void ContinueHandComparisonAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueHandComparisonAfterPublicReveal(f);
    internal void FinishHandComparison(ProgramSkillFrame f) => _resolve().FinishHandComparison(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot>? GetHandComparisonPublicRuleStates(CharacterState owner, string skillId) => _resolve().GetHandComparisonPublicRuleStates(owner, skillId);
    internal IReadOnlyList<int> GetSelfProhibitionPolicyTargets(CharacterState actor, LegalAction action) => _resolve().GetSelfProhibitionPolicyTargets(actor, action);
    internal IEnumerable<IGameEvent> HandComparisonHistory() => _resolve().HandComparisonHistory();
    internal int? HandComparisonPhaseLimit(CharacterState owner, SkillProgram program, SkillProgramActivation activation, string? instanceId = default) => _resolve().HandComparisonPhaseLimit(owner, program, activation, instanceId);
    internal bool HasSelfCardTargetProhibition(int seat) => _resolve().HasSelfCardTargetProhibition(seat);
    internal bool IsImplicitSelfCardUse(CardKind kind) => _resolve().IsImplicitSelfCardUse(kind);
    internal bool IsSelfTargetForbiddenAction(CharacterState actor, CardKind kind, IReadOnlyList<int> targets) => _resolve().IsSelfTargetForbiddenAction(actor, kind, targets);
    internal IEnumerable<RuleQueryContribution> PersistentHandLimitContributions(CharacterState owner) => _resolve().PersistentHandLimitContributions(owner);
    internal void PublishHandComparison(ProgramSkillFrame f) => _resolve().PublishHandComparison(f);
    internal void ResolveHandComparison(PromptChoice selected) => _resolve().ResolveHandComparison(selected);
    internal bool SelfCardDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().SelfCardDirectedTargetProhibited(query);
}
