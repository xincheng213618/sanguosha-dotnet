#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBEntries
{
    internal void AnswerWushuangB(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerWushuangB(selected);
    internal global::CardGame.Core.PromptChoice BChoice(global::CardGame.Core.ProgramSkillFrame f, string step, string key, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().BChoice(f, step, key, label, cards, targets);
    internal global::System.Collections.Generic.IEnumerable<int> BDinganDrawTargets(int owner, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().BDinganDrawTargets(owner, targets);
    internal bool BDyingThisTurn() => _resolve().BDyingThisTurn();
    internal bool BSecondTurnUse(long actionId, global::CardGame.Core.CardKind kind) => _resolve().BSecondTurnUse(actionId, kind);
    internal global::CardGame.Core.CardConversionSource BSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BSource(f);
    internal bool BSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BSourceCurrent(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> BTargets(global::CardGame.Core.ProgramCardUseContext use) => _resolve().BTargets(use);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> BTurnHistory() => _resolve().BTurnHistory();
    internal bool BZhendanDisabled(int seat) => _resolve().BZhendanDisabled(seat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWushuangB(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWushuangB(supplied, op);
    internal global::CardGame.Core.WushuangBianyuMarkedEvent? BianyuActiveMark(int seat, int id) => _resolve().BianyuActiveMark(seat, id);
    internal bool CanTriggerWushuangBSkill(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangBSkill(c, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseWushuangBAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseWushuangBAi(p, f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectWushuangBChunlaoLossCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectWushuangBChunlaoLossCandidates(change);
    internal bool ContinueWushuangB(long id) => _resolve().ContinueWushuangB(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateWushuangBChunlaoLossContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateWushuangBChunlaoLossContext(change, c);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawWushuangBFallback(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().DrawWushuangBFallback(f, count);
    internal void FinishWushuangB(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWushuangB(f);
    internal void FinishWushuangLiantao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWushuangLiantao(f);
    internal bool HasWushuangBianyuPotential(global::CardGame.Core.CharacterState owner) => _resolve().HasWushuangBianyuPotential(owner);
    internal bool IsWushuangBZeroUse(long id) => _resolve().IsWushuangBZeroUse(id);
    internal bool IsWushuangZhendanConversionForbidden(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().IsWushuangZhendanConversionForbidden(q);
    internal bool IsWushuangZhendanSubmittedConversionForbidden(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().IsWushuangZhendanSubmittedConversionForbidden(q);
    internal void IssueWushuangBTrick(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption option, global::CardGame.Core.Card? physical) => _resolve().IssueWushuangBTrick(f, option, physical);
    internal void ObserveWushuangBSkills(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveWushuangBSkills(payload);
    internal bool? ProveWushuangChunlao(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.CharacterState owner, global::CardGame.Core.HpChangeContext change) => _resolve().ProveWushuangChunlao(candidate, owner, change);
    internal void PublishWushuangBChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWushuangBChoice(f);
    internal bool ReturnWushuangBMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWushuangBMovement(f);
    internal bool ReturnWushuangBNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnWushuangBNativeAttack(completion);
    internal bool ReturnWushuangBNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnWushuangBNativeUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WushuangBChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WushuangBChoices(f);
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WushuangBianyuIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().WushuangBianyuIdentity(owner, card);
    internal bool WushuangBianyuQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().WushuangBianyuQuota(q);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> WushuangSkillsBNumericContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().WushuangSkillsBNumericContributions(player, query);
    internal bool HasWushuangBSkills { get => _resolve().HasWushuangBSkills; }
}
