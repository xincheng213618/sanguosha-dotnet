#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGongsunZanMouMobileEntries
{
    internal IEnumerable<MouGongsunZanMobileDistanceEvent> ActiveMouGszMobileDistance(int owner) => _resolve().ActiveMouGszMobileDistance(owner);
    internal void AddMouGszMobileDistance(ProgramDistanceQuery q, List<RuleQueryBaseTerm> terms) => _resolve().AddMouGszMobileDistance(q, terms);
    internal bool AdvanceMouGszMobileChild() => _resolve().AdvanceMouGszMobileChild();
    internal PromptChoice AiMouGszMobile(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouGszMobile(p, f);
    internal void AnswerMouGszMobile(PromptChoice choice) => _resolve().AnswerMouGszMobile(choice);
    internal IReadOnlyList<Card> AppendMouGszHuPlayable(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().AppendMouGszHuPlayable(owner, cards);
    internal void AssertMouGszHuPayments() => _resolve().AssertMouGszHuPayments();
    internal SkillProgramStepOutcome BeginMouGszMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMouGszMobile(supplied, op);
    internal void BindMouGszHuAction(IGameEvent payload) => _resolve().BindMouGszHuAction(payload);
    internal void CaptureMouGszHuPaymentBatch(CardMovementBatchContext batch) => _resolve().CaptureMouGszHuPaymentBatch(batch);
    internal void ChangeMouGszMobileCharge(long frame, int owner, int delta) => _resolve().ChangeMouGszMobileCharge(frame, owner, delta);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouGszHuPaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouGszHuPaymentBatch(batch, completed);
    internal void ContinueMouGszMobile(long id) => _resolve().ContinueMouGszMobile(id);
    internal bool DrainMouGszMobile(ProgramSkillFrame f) => _resolve().DrainMouGszMobile(f);
    internal IReadOnlyList<LegalAction> FilterMouGszHuActions(CharacterState owner, IReadOnlyList<LegalAction> actions) => _resolve().FilterMouGszHuActions(owner, actions);
    internal void FinishMouGszMobile(ProgramSkillFrame f) => _resolve().FinishMouGszMobile(f);
    internal bool IsIssuedMouGszHuPile(PublicPersistentPileSource p) => _resolve().IsIssuedMouGszHuPile(p);
    internal bool IsLiveMouGszHuPile(PublicPersistentPileSource p) => _resolve().IsLiveMouGszHuPile(p);
    internal bool IsMouGszHuActionCost(CardActionContext action, CardActionCost cost) => _resolve().IsMouGszHuActionCost(action, cost);
    internal bool IsMouGszHuCard(CharacterState owner, int id) => _resolve().IsMouGszHuCard(owner, id);
    internal bool IsMouGszHuOriginalChoice(NativePublicPileCardRequest request, NativePublicPileCardSelection choice) => _resolve().IsMouGszHuOriginalChoice(request, choice);
    internal bool IsMouGszHuOriginalPayment(NativePublicPileCardPayment p) => _resolve().IsMouGszHuOriginalPayment(p);
    internal bool KeepMouGszMobile(ProgramSkillFrame f) => _resolve().KeepMouGszMobile(f);
    internal CardConversionSource MouGszHuAccess(PublicPersistentPileSource p) => _resolve().MouGszHuAccess(p);
    internal IReadOnlyList<Card> MouGszHuCards(CharacterState owner) => _resolve().MouGszHuCards(owner);
    internal bool MouGszHuHandSource(CharacterState owner, Card card, IReadOnlyList<CardZoneKind> zones) => _resolve().MouGszHuHandSource(owner, card, zones);
    internal IReadOnlyList<NativePublicPileCardSelection> MouGszHuOriginalChoices(NativePublicPileCardRequest request) => _resolve().MouGszHuOriginalChoices(request);
    internal IEnumerable<PublicPersistentPileSource> MouGszHuPiles(int owner) => _resolve().MouGszHuPiles(owner);
    internal int MouGszMobileCharge(int owner) => _resolve().MouGszMobileCharge(owner);
    internal bool MouGszMobileChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGszMobileChild(f, child);
    internal PromptChoice MouGszMobileChoice(ProgramSkillFrame f, string branch, string text, int amount = 0) => _resolve().MouGszMobileChoice(f, branch, text, amount);
    internal IReadOnlyList<PromptChoice> MouGszMobileChoices(ProgramSkillFrame f) => _resolve().MouGszMobileChoices(f);
    internal DamageFrame? MouGszMobileDamage(ProgramSkillWindowContext c, int owner) => _resolve().MouGszMobileDamage(c, owner);
    internal bool MouGszMobileDying() => _resolve().MouGszMobileDying();
    internal bool MouGszMobileLion(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGszMobileLion(f, child);
    internal bool MouGszMobileRecovery(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().MouGszMobileRecovery(f, child);
    internal ProgramSkillFrame? MouGszMobileRoot() => _resolve().MouGszMobileRoot();
    internal bool MouGszMobileSuffix(int n) => _resolve().MouGszMobileSuffix(n);
    internal bool OfferMouGszMobile(ProgramTriggerLegalityContext c) => _resolve().OfferMouGszMobile(c);
    internal void PrepareMouGszHuPayment(ResolutionFrame parent, int actor, int provider, int? requester, IReadOnlyList<Card> cards, IReadOnlyList<CardConversionSource> conversions, long? actionId) => _resolve().PrepareMouGszHuPayment(parent, actor, provider, requester, cards, conversions, actionId);
    internal bool PrepareMouGszHuResponse(CharacterState provider, IReadOnlyList<Card> cards, CardKind kind, CardConversionSource? conversion, int? actor = default) => _resolve().PrepareMouGszHuResponse(provider, cards, kind, conversion, actor);
    internal void PrepareMouGszHuUse(CardUseFrame use) => _resolve().PrepareMouGszHuUse(use);
    internal void PublishMouGszMobile(ProgramSkillFrame f) => _resolve().PublishMouGszMobile(f);
    internal bool ResumeMouGszMobile(long id) => _resolve().ResumeMouGszMobile(id);
    internal void ReturnMouGszMobileDraw(ProgramSkillFrame f, NativeDrawOutcome o) => _resolve().ReturnMouGszMobileDraw(f, o);
    internal bool ReturnMouGszMobileMovement(ProgramSkillFrame f) => _resolve().ReturnMouGszMobileMovement(f);
    internal void SealMouGszMobile(long id) => _resolve().SealMouGszMobile(id);
    internal bool ValidMouGszMobile(ProgramSkillFrame f) => _resolve().ValidMouGszMobile(f);
    internal bool ValidateMouGszHuConversion(CharacterState provider, IReadOnlyList<Card> materials, CardConversionSource? conversion, bool forResponse, CardKind? effectiveKind) => _resolve().ValidateMouGszHuConversion(provider, materials, conversion, forResponse, effectiveKind);
    internal bool HasMouGszMobile { get => _resolve().HasMouGszMobile; }
}
