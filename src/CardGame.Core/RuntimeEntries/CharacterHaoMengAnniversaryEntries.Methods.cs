#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHaoMengAnniversaryEntries
{
    internal bool AdvanceHaoMengChildren() => _resolve().AdvanceHaoMengChildren();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> AnniversaryHaoMengSlashContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().AnniversaryHaoMengSlashContributions(player, query);
    internal void AnswerHaoMeng(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHaoMeng(choice);
    internal void AssertHaoMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertHaoMeng(f);
    internal bool BanHaoMengConversion(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BanHaoMengConversion(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHaoMeng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHaoMeng(supplied, op);
    internal void BindHaoMengUse(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().BindHaoMengUse(e);
    internal bool CanStartHaoMeng(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanStartHaoMeng(c);
    internal void CollectHaoMengConsequence(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectHaoMengConsequence(action, window, result);
    internal void ContinueHaoMeng(long id) => _resolve().ContinueHaoMeng(id);
    internal void FinishHaoMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHaoMeng(f);
    internal global::CardGame.Core.PromptChoice HaoMengAi(global::CardGame.Core.PendingDecision p) => _resolve().HaoMengAi(p);
    internal int[] HaoMengAvailableCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HaoMengAvailableCards(f);
    internal bool HaoMengBeforeDamage(long id) => _resolve().HaoMengBeforeDamage(id);
    internal global::CardGame.Core.PromptChoice HaoMengChoice(global::CardGame.Core.ProgramSkillFrame f, string verb, string value, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default) => _resolve().HaoMengChoice(f, verb, value, label, cards, targets);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HaoMengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HaoMengChoices(f);
    internal global::CardGame.Core.CardUseFrame? HaoMengConsequenceUse(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().HaoMengConsequenceUse(c);
    internal bool HaoMengDamageObserver(long id) => _resolve().HaoMengDamageObserver(id);
    internal bool HaoMengDying() => _resolve().HaoMengDying();
    internal bool HaoMengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HaoMengFirstChild(f, child);
    internal global::CardGame.Core.Suit HaoMengHandSuit(int owner, int id) => _resolve().HaoMengHandSuit(owner, id);
    internal bool HaoMengMaterials(int owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardConversionSource source) => _resolve().HaoMengMaterials(owner, ids, source);
    internal bool HaoMengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HaoMengMovement(f, e, p);
    internal bool HaoMengNativeEdge(int child) => _resolve().HaoMengNativeEdge(child);
    internal int[] HaoMengNativeTargets(int actor, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardConversionSource source) => _resolve().HaoMengNativeTargets(actor, ids, source);
    internal global::CardGame.Core.ProgramSkillFrame? HaoMengObserverRoot() => _resolve().HaoMengObserverRoot();
    internal bool HaoMengOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().HaoMengOriginal(f, op);
    internal bool HaoMengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().HaoMengRecoveryParent(f, recovery);
    internal bool HaoMengSuffix(int i) => _resolve().HaoMengSuffix(i);
    internal bool HasAnniversaryHaoMengPrompt() => _resolve().HasAnniversaryHaoMengPrompt();
    internal bool IsAnniversaryHaoMengSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext b) => _resolve().IsAnniversaryHaoMengSettlementBatch(f, b);
    internal bool IsHaoMengMaterialSlashAttack(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsHaoMengMaterialSlashAttack(pending, attack, processing);
    internal bool IsHaoMengNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsHaoMengNativeUse(use);
    internal bool IsIssuedHaoMengConsequence(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedHaoMengConsequence(c, w);
    internal void IssueHaoMengUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueHaoMengUse(f);
    internal bool KeepHaoMengIssued(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepHaoMengIssued(f);
    internal bool OfferHaoMengConsequence(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().OfferHaoMengConsequence(c);
    internal global::CardGame.Core.LegalAction? ProjectHaoMengUse(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().ProjectHaoMengUse(q);
    internal void PublishHaoMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHaoMeng(f);
    internal bool ResumeHaoMeng(long id) => _resolve().ResumeHaoMeng(id);
    internal bool ReturnHaoMengAttack() => _resolve().ReturnHaoMengAttack();
    internal bool ReturnHaoMengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHaoMengMovement(f);
    internal void ReturnHaoMengNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnHaoMengNative(use);
    internal void SettleHaoMengNative(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleHaoMengNative(f);
    internal bool ValidHaoMeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHaoMeng(f);
    internal void ValidateHaoMengNative(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateHaoMengNative(use);
    internal bool WindowSettlement_IsAnniversaryHaoMengSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsAnniversaryHaoMengSettlementBatch(frame, batch);
    internal bool HasAnniversaryHaoMengCapability { get => _resolve().HasAnniversaryHaoMengCapability; }
}
