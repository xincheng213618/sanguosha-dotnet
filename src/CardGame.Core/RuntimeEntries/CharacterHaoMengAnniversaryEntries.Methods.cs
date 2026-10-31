#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHaoMengAnniversaryEntries
{
    internal bool AdvanceHaoMengChildren() => _resolve().AdvanceHaoMengChildren();
    internal IEnumerable<RuleQueryContribution> AnniversaryHaoMengSlashContributions(CharacterState player, SkillRuleQuery query) => _resolve().AnniversaryHaoMengSlashContributions(player, query);
    internal void AnswerHaoMeng(PromptChoice choice) => _resolve().AnswerHaoMeng(choice);
    internal bool BanHaoMengConversion(ProgramViewAsLegalityQuery q) => _resolve().BanHaoMengConversion(q);
    internal SkillProgramStepOutcome BeginHaoMeng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHaoMeng(supplied, op);
    internal void BindHaoMengUse(CardUseDeclaredEvent e) => _resolve().BindHaoMengUse(e);
    internal bool CanStartHaoMeng(ProgramActivationLegalityContext c) => _resolve().CanStartHaoMeng(c);
    internal void CollectHaoMengConsequence(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectHaoMengConsequence(action, window, result);
    internal void ContinueHaoMeng(long id) => _resolve().ContinueHaoMeng(id);
    internal void FinishHaoMeng(ProgramSkillFrame f) => _resolve().FinishHaoMeng(f);
    internal PromptChoice HaoMengAi(PendingDecision p) => _resolve().HaoMengAi(p);
    internal int[] HaoMengAvailableCards(ProgramSkillFrame f) => _resolve().HaoMengAvailableCards(f);
    internal PromptChoice HaoMengChoice(ProgramSkillFrame f, string verb, string value, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default) => _resolve().HaoMengChoice(f, verb, value, label, cards, targets);
    internal IReadOnlyList<PromptChoice> HaoMengChoices(ProgramSkillFrame f) => _resolve().HaoMengChoices(f);
    internal CardUseFrame? HaoMengConsequenceUse(ProgramSkillWindowContext c) => _resolve().HaoMengConsequenceUse(c);
    internal bool HaoMengDying() => _resolve().HaoMengDying();
    internal bool HaoMengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HaoMengFirstChild(f, child);
    internal Suit HaoMengHandSuit(int owner, int id) => _resolve().HaoMengHandSuit(owner, id);
    internal bool HaoMengMaterials(int owner, IReadOnlyList<int> ids, CardConversionSource source) => _resolve().HaoMengMaterials(owner, ids, source);
    internal bool HaoMengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().HaoMengMovement(f, e, p);
    internal bool HaoMengNativeEdge(int child) => _resolve().HaoMengNativeEdge(child);
    internal int[] HaoMengNativeTargets(int actor, IReadOnlyList<int> ids, CardConversionSource source) => _resolve().HaoMengNativeTargets(actor, ids, source);
    internal ProgramSkillFrame? HaoMengObserverRoot() => _resolve().HaoMengObserverRoot();
    internal bool HaoMengOriginal(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().HaoMengOriginal(f, op);
    internal bool HaoMengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().HaoMengRecoveryParent(f, recovery);
    internal bool HaoMengSuffix(int i) => _resolve().HaoMengSuffix(i);
    internal bool IsAnniversaryHaoMengSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsAnniversaryHaoMengSettlementBatch(f, b);
    internal bool IsHaoMengNativeUse(CardUseFrame use) => _resolve().IsHaoMengNativeUse(use);
    internal bool IsIssuedHaoMengConsequence(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedHaoMengConsequence(c, w);
    internal void IssueHaoMengUse(ProgramSkillFrame f) => _resolve().IssueHaoMengUse(f);
    internal bool KeepHaoMengIssued(ProgramSkillFrame f) => _resolve().KeepHaoMengIssued(f);
    internal bool OfferHaoMengConsequence(ProgramTriggerLegalityContext c) => _resolve().OfferHaoMengConsequence(c);
    internal LegalAction? ProjectHaoMengUse(ProgramLegalActionQuery q) => _resolve().ProjectHaoMengUse(q);
    internal void PublishHaoMeng(ProgramSkillFrame f) => _resolve().PublishHaoMeng(f);
    internal bool ResumeHaoMeng(long id) => _resolve().ResumeHaoMeng(id);
    internal bool ReturnHaoMengAttack() => _resolve().ReturnHaoMengAttack();
    internal bool ReturnHaoMengMovement(ProgramSkillFrame f) => _resolve().ReturnHaoMengMovement(f);
    internal void ReturnHaoMengNative(CardUseFrame use) => _resolve().ReturnHaoMengNative(use);
    internal void SettleHaoMengNative(ProgramSkillFrame f) => _resolve().SettleHaoMengNative(f);
    internal bool ValidHaoMeng(ProgramSkillFrame f) => _resolve().ValidHaoMeng(f);
    internal void ValidateHaoMengNative(CardUseFrame use) => _resolve().ValidateHaoMengNative(use);
    internal bool WindowSettlement_IsAnniversaryHaoMengSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryHaoMengSettlementBatch(frame, batch);
    internal bool HasAnniversaryHaoMengCapability { get => _resolve().HasAnniversaryHaoMengCapability; }
}
