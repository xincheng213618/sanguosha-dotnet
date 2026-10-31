#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGongsunZanMouMobileEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouGongsunZanMobileDistanceEvent> ActiveMouGszMobileDistance(int owner) => _resolve().ActiveMouGszMobileDistance(owner);
    internal void AddMouGszMobileDistance(global::CardGame.Core.ProgramDistanceQuery q, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryBaseTerm> terms) => _resolve().AddMouGszMobileDistance(q, terms);
    internal bool AdvanceMouGszMobileChild() => _resolve().AdvanceMouGszMobileChild();
    internal global::CardGame.Core.PromptChoice AiMouGszMobile(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouGszMobile(p, f);
    internal void AnswerMouGszMobile(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouGszMobile(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AppendMouGszHuPlayable(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().AppendMouGszHuPlayable(owner, cards);
    internal void AssertMouGszHuPayments() => _resolve().AssertMouGszHuPayments();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouGszMobile(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMouGszMobile(supplied, op);
    internal void BindMouGszHuAction(global::CardGame.Core.IGameEvent payload) => _resolve().BindMouGszHuAction(payload);
    internal void CaptureMouGszHuPaymentBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouGszHuPaymentBatch(batch);
    internal void ChangeMouGszMobileCharge(long frame, int owner, int delta) => _resolve().ChangeMouGszMobileCharge(frame, owner, delta);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouGszHuPaymentBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouGszHuPaymentBatch(batch, completed);
    internal void ContinueMouGszMobile(long id) => _resolve().ContinueMouGszMobile(id);
    internal bool DrainMouGszMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouGszMobile(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FilterMouGszHuActions(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> actions) => _resolve().FilterMouGszHuActions(owner, actions);
    internal void FinishMouGszMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouGszMobile(f);
    internal bool IsIssuedMouGszHuPile(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().IsIssuedMouGszHuPile(p);
    internal bool IsLiveMouGszHuPile(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().IsLiveMouGszHuPile(p);
    internal bool IsMouGszHuActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => _resolve().IsMouGszHuActionCost(action, cost);
    internal bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id) => _resolve().IsMouGszHuCard(owner, id);
    internal bool IsMouGszHuOriginalChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection choice) => _resolve().IsMouGszHuOriginalChoice(request, choice);
    internal bool IsMouGszHuOriginalPayment(global::CardGame.Core.NativePublicPileCardPayment p) => _resolve().IsMouGszHuOriginalPayment(p);
    internal bool KeepMouGszMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMouGszMobile(f);
    internal global::CardGame.Core.CardConversionSource MouGszHuAccess(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().MouGszHuAccess(p);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MouGszHuCards(global::CardGame.Core.CharacterState owner) => _resolve().MouGszHuCards(owner);
    internal bool MouGszHuHandSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones) => _resolve().MouGszHuHandSource(owner, card, zones);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> MouGszHuOriginalChoices(global::CardGame.Core.NativePublicPileCardRequest request) => _resolve().MouGszHuOriginalChoices(request);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.PublicPersistentPileSource> MouGszHuPiles(int owner) => _resolve().MouGszHuPiles(owner);
    internal int MouGszMobileCharge(int owner) => _resolve().MouGszMobileCharge(owner);
    internal bool MouGszMobileChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGszMobileChild(f, child);
    internal global::CardGame.Core.PromptChoice MouGszMobileChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, int amount = 0) => _resolve().MouGszMobileChoice(f, branch, text, amount);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouGszMobileChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGszMobileChoices(f);
    internal global::CardGame.Core.DamageFrame? MouGszMobileDamage(global::CardGame.Core.ProgramSkillWindowContext c, int owner) => _resolve().MouGszMobileDamage(c, owner);
    internal bool MouGszMobileDying() => _resolve().MouGszMobileDying();
    internal bool MouGszMobileLion(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGszMobileLion(f, child);
    internal bool MouGszMobileRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().MouGszMobileRecovery(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouGszMobileRoot() => _resolve().MouGszMobileRoot();
    internal bool MouGszMobileSuffix(int n) => _resolve().MouGszMobileSuffix(n);
    internal bool OfferMouGszMobile(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().OfferMouGszMobile(c);
    internal void PrepareMouGszHuPayment(global::CardGame.Core.ResolutionFrame parent, int actor, int provider, int? requester, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> conversions, long? actionId) => _resolve().PrepareMouGszHuPayment(parent, actor, provider, requester, cards, conversions, actionId);
    internal bool PrepareMouGszHuResponse(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::CardGame.Core.CardConversionSource? conversion, int? actor = default) => _resolve().PrepareMouGszHuResponse(provider, cards, kind, conversion, actor);
    internal void PrepareMouGszHuUse(global::CardGame.Core.CardUseFrame use) => _resolve().PrepareMouGszHuUse(use);
    internal void PublishMouGszMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouGszMobile(f);
    internal bool ResumeMouGszMobile(long id) => _resolve().ResumeMouGszMobile(id);
    internal void ReturnMouGszMobileDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnMouGszMobileDraw(f, o);
    internal bool ReturnMouGszMobileMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouGszMobileMovement(f);
    internal void SealMouGszMobile(long id) => _resolve().SealMouGszMobile(id);
    internal bool ValidMouGszMobile(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouGszMobile(f);
    internal bool ValidateMouGszHuConversion(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversion, bool forResponse, global::CardGame.Core.CardKind? effectiveKind) => _resolve().ValidateMouGszHuConversion(provider, materials, conversion, forResponse, effectiveKind);
    internal bool HasMouGszMobile { get => _resolve().HasMouGszMobile; }
}
