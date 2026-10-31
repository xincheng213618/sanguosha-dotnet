#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangSkillsBEntries
{
    internal void AnswerWushuangB(PromptChoice selected) => _resolve().AnswerWushuangB(selected);
    internal PromptChoice BChoice(ProgramSkillFrame f, string step, string key, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().BChoice(f, step, key, label, cards, targets);
    internal IEnumerable<int> BDinganDrawTargets(int owner, IReadOnlyList<int> targets) => _resolve().BDinganDrawTargets(owner, targets);
    internal bool BDyingThisTurn() => _resolve().BDyingThisTurn();
    internal bool BSecondTurnUse(long actionId, CardKind kind) => _resolve().BSecondTurnUse(actionId, kind);
    internal CardConversionSource BSource(ProgramSkillFrame f) => _resolve().BSource(f);
    internal bool BSourceCurrent(ProgramSkillFrame f) => _resolve().BSourceCurrent(f);
    internal IReadOnlyList<int> BTargets(ProgramCardUseContext use) => _resolve().BTargets(use);
    internal IEnumerable<IGameEvent> BTurnHistory() => _resolve().BTurnHistory();
    internal bool BZhendanDisabled(int seat) => _resolve().BZhendanDisabled(seat);
    internal SkillProgramStepOutcome BeginWushuangB(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginWushuangB(supplied, op);
    internal WushuangBianyuMarkedEvent? BianyuActiveMark(int seat, int id) => _resolve().BianyuActiveMark(seat, id);
    internal bool CanTriggerWushuangBSkill(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangBSkill(c, trigger, context);
    internal PromptChoice ChooseWushuangBAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseWushuangBAi(p, f);
    internal IEnumerable<ProgramTriggerCandidate> CollectWushuangBChunlaoLossCandidates(HpChangeContext change) => _resolve().CollectWushuangBChunlaoLossCandidates(change);
    internal bool ContinueWushuangB(long id) => _resolve().ContinueWushuangB(id);
    internal ProgramSkillWindowContext CreateWushuangBChunlaoLossContext(HpChangeContext change, ProgramTriggerCandidate c) => _resolve().CreateWushuangBChunlaoLossContext(change, c);
    internal SkillProgramStepOutcome DrawWushuangBFallback(ProgramSkillFrame f, int count) => _resolve().DrawWushuangBFallback(f, count);
    internal void FinishWushuangB(ProgramSkillFrame f) => _resolve().FinishWushuangB(f);
    internal void FinishWushuangLiantao(ProgramSkillFrame f) => _resolve().FinishWushuangLiantao(f);
    internal bool HasWushuangBianyuPotential(CharacterState owner) => _resolve().HasWushuangBianyuPotential(owner);
    internal bool IsWushuangBZeroUse(long id) => _resolve().IsWushuangBZeroUse(id);
    internal bool IsWushuangZhendanConversionForbidden(ProgramConfiguredViewAsQuery q) => _resolve().IsWushuangZhendanConversionForbidden(q);
    internal bool IsWushuangZhendanSubmittedConversionForbidden(ProgramViewAsLegalityQuery q) => _resolve().IsWushuangZhendanSubmittedConversionForbidden(q);
    internal void IssueWushuangBTrick(ProgramSkillFrame f, GameEngine.ProgramOrdinaryTrickUseOption option, Card? physical) => _resolve().IssueWushuangBTrick(f, option, physical);
    internal void ObserveWushuangBSkills(IGameEvent payload) => _resolve().ObserveWushuangBSkills(payload);
    internal bool? ProveWushuangChunlao(ProgramTriggerCandidate candidate, CharacterState owner, HpChangeContext change) => _resolve().ProveWushuangChunlao(candidate, owner, change);
    internal void PublishWushuangBChoice(ProgramSkillFrame f) => _resolve().PublishWushuangBChoice(f);
    internal bool ReturnWushuangBMovement(ProgramSkillFrame f) => _resolve().ReturnWushuangBMovement(f);
    internal bool ReturnWushuangBNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnWushuangBNativeAttack(completion);
    internal bool ReturnWushuangBNativeUse(CardUseFrame use) => _resolve().ReturnWushuangBNativeUse(use);
    internal IReadOnlyList<PromptChoice> WushuangBChoices(ProgramSkillFrame f) => _resolve().WushuangBChoices(f);
    internal GameEngine.ProgramCardIdentityMatch? WushuangBianyuIdentity(CharacterState owner, Card card) => _resolve().WushuangBianyuIdentity(owner, card);
    internal bool WushuangBianyuQuota(ProgramSlashQuotaQuery q) => _resolve().WushuangBianyuQuota(q);
    internal IEnumerable<RuleQueryContribution> WushuangSkillsBNumericContributions(CharacterState player, SkillRuleQuery query) => _resolve().WushuangSkillsBNumericContributions(player, query);
    internal bool HasWushuangBSkills { get => _resolve().HasWushuangBSkills; }
}
