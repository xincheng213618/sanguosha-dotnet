#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengJiChengCuiOlEntries
{
    internal PromptChoice AiXumingUse(PendingDecision p, ProgramSkillFrame f) => _resolve().AiXumingUse(p, f);
    internal void AnswerTousuiRequest(PromptChoice selected) => _resolve().AnswerTousuiRequest(selected);
    internal void AnswerXumingUse(PromptChoice selected) => _resolve().AnswerXumingUse(selected);
    internal void BeginTousuiRequest(TousuiRequestReceipt r) => _resolve().BeginTousuiRequest(r);
    internal SkillProgramStepOutcome BeginXumingEnding(ProgramSkillFrame supplied) => _resolve().BeginXumingEnding(supplied);
    internal bool CanOfferXumingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferXumingEnding(candidate, context);
    internal bool CanXumingDamage(int owner, ProgramSkillWindowContext c) => _resolve().CanXumingDamage(owner, c);
    internal void CaptureTousuiUse(IGameEvent fact) => _resolve().CaptureTousuiUse(fact);
    internal bool ContinueIssuedXuming(ProgramSkillFrame f) => _resolve().ContinueIssuedXuming(f);
    internal void ContinueXumingEnding(long id) => _resolve().ContinueXumingEnding(id);
    internal TousuiRequestReceipt? CurrentTousuiRequest() => _resolve().CurrentTousuiRequest();
    internal bool HasAcceptedTousuiKeAction(ProgramSkillFrame f, LegalAction action) => _resolve().HasAcceptedTousuiKeAction(f, action);
    internal bool HasTousuiProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolve().HasTousuiProgramSlash(actor, targets);
    internal bool HasTousuiRequestPrompt() => _resolve().HasTousuiRequestPrompt();
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeXumingAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary) => _resolve().IncludeXumingAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedXumingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedXumingCandidate(c, context);
    internal bool IsIssuedXumingUse(CardUseFrame use) => _resolve().IsIssuedXumingUse(use);
    internal bool IsTousuiSource(CardConversionSource? source) => _resolve().IsTousuiSource(source);
    internal bool IsTousuiVariableSource(CharacterState actor, SkillProgramViewAs rule, Card card, CardLocation from) => _resolve().IsTousuiVariableSource(actor, rule, card, from);
    internal bool IsXumingClaimedProcessingMaterial(long originalUseId, int cardId) => _resolve().IsXumingClaimedProcessingMaterial(originalUseId, cardId);
    internal void OpenTousuiProgramRequest(PromptChoice selected) => _resolve().OpenTousuiProgramRequest(selected);
    internal IReadOnlyList<XumingPromiseIssuedEvent> PendingXumingPromises(int owner) => _resolve().PendingXumingPromises(owner);
    internal void PrepareTousuiWuJingNative(ProgramSkillFrame f, ProgramMultiCardViewAsSelection selection) => _resolve().PrepareTousuiWuJingNative(f, selection);
    internal void PublishTousuiRequest(TousuiRequestReceipt r) => _resolve().PublishTousuiRequest(r);
    internal SkillProgramStepOutcome ResolveTousuiRequirement(ProgramSkillFrame f) => _resolve().ResolveTousuiRequirement(f);
    internal SkillProgramStepOutcome ResolveXumingDamage(ProgramSkillFrame supplied) => _resolve().ResolveXumingDamage(supplied);
    internal bool ResumeXumingEnding(long id) => _resolve().ResumeXumingEnding(id);
    internal void ReturnXumingNativeUse(long useId) => _resolve().ReturnXumingNativeUse(useId);
    internal void SettleXumingPromise(ProgramSkillFrame f, bool used, long? useId = default) => _resolve().SettleXumingPromise(f, used, useId);
    internal void SignTousuiBottomPayment(CardMovementBatchContext batch) => _resolve().SignTousuiBottomPayment(batch);
    internal void StoreTousuiRequest(TousuiRequestReceipt? r, long id) => _resolve().StoreTousuiRequest(r, id);
    internal IReadOnlyList<PromptChoice> TousuiBorrowedSlashChoices(NativeOwners.BorrowedSwordHandle pending) => _resolve().TousuiBorrowedSlashChoices(pending);
    internal IReadOnlyList<Card> TousuiPhysicalCards(CharacterState actor) => _resolve().TousuiPhysicalCards(actor);
    internal bool TousuiProgramActor(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => _resolve().TousuiProgramActor(f, op, actor);
    internal IReadOnlyList<PromptChoice> TousuiProgramSlashChoices(ProgramSkillFrame f, int actorSeat, IReadOnlyList<int> targets, string? resultBind = default) => _resolve().TousuiProgramSlashChoices(f, actorSeat, targets, resultBind);
    internal int[] TousuiProgramTargets(ProgramSkillFrame f, int actor, SkillProgramEffectOp op) => _resolve().TousuiProgramTargets(f, actor, op);
    internal IReadOnlyList<PromptChoice> TousuiQinglongSlashChoices(NativeOwners.QinglongCrescentBladeHandle pending) => _resolve().TousuiQinglongSlashChoices(pending);
    internal IReadOnlyList<Card> TousuiRequestCards(CardConversionSource source) => _resolve().TousuiRequestCards(source);
    internal IReadOnlyList<PromptChoice> TousuiRequestPickChoices(TousuiRequestReceipt r) => _resolve().TousuiRequestPickChoices(r);
    internal int TousuiRequiredDodge(long useId, int ordinary) => _resolve().TousuiRequiredDodge(useId, ordinary);
    internal bool TousuiSelectedRequestLegal(TousuiRequestReceipt r) => _resolve().TousuiSelectedRequestLegal(r);
    internal IEnumerable<CardConversionSource> TousuiSources(CharacterState actor) => _resolve().TousuiSources(actor);
    internal bool TousuiUseProof(CardUseFrame use) => _resolve().TousuiUseProof(use);
    internal bool TryBeginTousuiBorrowedSlash(NativeOwners.BorrowedSwordHandle pending, PromptChoice selected) => _resolve().TryBeginTousuiBorrowedSlash(pending, selected);
    internal bool TryBeginTousuiQinglongSlash(NativeOwners.QinglongCrescentBladeHandle pending, PromptChoice selected) => _resolve().TryBeginTousuiQinglongSlash(pending, selected);
    internal bool TryChooseTousuiAi(PendingDecision p, scoped out PromptChoice choice) => _resolve().TryChooseTousuiAi(p, out choice);
    internal bool ValidTousuiRequest(TousuiRequestReceipt r) => _resolve().ValidTousuiRequest(r);
    internal void ValidateXumingNativeUse(CardUseFrame use) => _resolve().ValidateXumingNativeUse(use);
    internal IReadOnlyList<PromptChoice> XumingChoices(ProgramSkillFrame f) => _resolve().XumingChoices(f);
    internal IReadOnlyList<Card> XumingCurrentMaterials(XumingPromiseIssuedEvent p) => _resolve().XumingCurrentMaterials(p);
    internal bool XumingDamageEligible(int owner, ProgramSkillWindowContext c) => _resolve().XumingDamageEligible(owner, c);
    internal IReadOnlyList<XumingUsePlan> XumingPlans(ProgramSkillFrame f) => _resolve().XumingPlans(f);
    internal bool HasChengJiChengCui { get => _resolve().HasChengJiChengCui; }
}
