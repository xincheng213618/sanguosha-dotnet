#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengJiChengCuiOlEntries
{
    internal global::CardGame.Core.PromptChoice AiXumingUse(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiXumingUse(p, f);
    internal void AnswerTousuiRequest(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerTousuiRequest(selected);
    internal void AnswerXumingUse(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerXumingUse(selected);
    internal void BeginTousuiRequest(global::CardGame.Core.TousuiRequestReceipt r) => _resolve().BeginTousuiRequest(r);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXumingEnding(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginXumingEnding(supplied);
    internal bool CanOfferXumingEnding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferXumingEnding(candidate, context);
    internal bool CanXumingDamage(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanXumingDamage(owner, c);
    internal void CaptureTousuiUse(global::CardGame.Core.IGameEvent fact) => _resolve().CaptureTousuiUse(fact);
    internal bool ContinueIssuedXuming(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedXuming(f);
    internal void ContinueXumingEnding(long id) => _resolve().ContinueXumingEnding(id);
    internal global::CardGame.Core.TousuiRequestReceipt? CurrentTousuiRequest() => _resolve().CurrentTousuiRequest();
    internal bool HasAcceptedTousuiKeAction(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => _resolve().HasAcceptedTousuiKeAction(f, action);
    internal bool HasTousuiProgramSlash(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<int> targets) => _resolve().HasTousuiProgramSlash(actor, targets);
    internal bool HasTousuiRequestPrompt() => _resolve().HasTousuiRequestPrompt();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> IncludeXumingAfterTurnEndedItems(global::CardGame.Core.DeferredTurnEndFrame parent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnEndingBoundaryItem> ordinary) => _resolve().IncludeXumingAfterTurnEndedItems(parent, ordinary);
    internal bool IsIssuedXumingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedXumingCandidate(c, context);
    internal bool IsIssuedXumingUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedXumingUse(use);
    internal bool IsTousuiSource(global::CardGame.Core.CardConversionSource? source) => _resolve().IsTousuiSource(source);
    internal bool IsTousuiVariableSource(global::CardGame.Core.CharacterState actor, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().IsTousuiVariableSource(actor, rule, card, from);
    internal bool IsXumingClaimedProcessingMaterial(long originalUseId, int cardId) => _resolve().IsXumingClaimedProcessingMaterial(originalUseId, cardId);
    internal void OpenTousuiProgramRequest(global::CardGame.Core.PromptChoice selected) => _resolve().OpenTousuiProgramRequest(selected);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.XumingPromiseIssuedEvent> PendingXumingPromises(int owner) => _resolve().PendingXumingPromises(owner);
    internal void PrepareTousuiWuJingNative(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().PrepareTousuiWuJingNative(f, selection);
    internal void PublishTousuiRequest(global::CardGame.Core.TousuiRequestReceipt r) => _resolve().PublishTousuiRequest(r);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveTousuiRequirement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ResolveTousuiRequirement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveXumingDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveXumingDamage(supplied);
    internal bool ResumeXumingEnding(long id) => _resolve().ResumeXumingEnding(id);
    internal void ReturnXumingNativeUse(long useId) => _resolve().ReturnXumingNativeUse(useId);
    internal void SettleXumingPromise(global::CardGame.Core.ProgramSkillFrame f, bool used, long? useId = default) => _resolve().SettleXumingPromise(f, used, useId);
    internal void SignTousuiBottomPayment(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().SignTousuiBottomPayment(batch);
    internal void StoreTousuiRequest(global::CardGame.Core.TousuiRequestReceipt? r, long id) => _resolve().StoreTousuiRequest(r, id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiBorrowedSlashChoices(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _resolve().TousuiBorrowedSlashChoices(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> TousuiPhysicalCards(global::CardGame.Core.CharacterState actor) => _resolve().TousuiPhysicalCards(actor);
    internal bool TousuiProgramActor(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op, int actor) => _resolve().TousuiProgramActor(f, op, actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiProgramSlashChoices(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> targets, string? resultBind = default) => _resolve().TousuiProgramSlashChoices(f, actorSeat, targets, resultBind);
    internal int[] TousuiProgramTargets(global::CardGame.Core.ProgramSkillFrame f, int actor, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().TousuiProgramTargets(f, actor, op);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiQinglongSlashChoices(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending) => _resolve().TousuiQinglongSlashChoices(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> TousuiRequestCards(global::CardGame.Core.CardConversionSource source) => _resolve().TousuiRequestCards(source);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TousuiRequestPickChoices(global::CardGame.Core.TousuiRequestReceipt r) => _resolve().TousuiRequestPickChoices(r);
    internal int TousuiRequiredDodge(long useId, int ordinary) => _resolve().TousuiRequiredDodge(useId, ordinary);
    internal bool TousuiSelectedRequestLegal(global::CardGame.Core.TousuiRequestReceipt r) => _resolve().TousuiSelectedRequestLegal(r);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardConversionSource> TousuiSources(global::CardGame.Core.CharacterState actor) => _resolve().TousuiSources(actor);
    internal bool TousuiUseProof(global::CardGame.Core.CardUseFrame use) => _resolve().TousuiUseProof(use);
    internal bool TryBeginTousuiBorrowedSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.PromptChoice selected) => _resolve().TryBeginTousuiBorrowedSlash(pending, selected);
    internal bool TryBeginTousuiQinglongSlash(global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle pending, global::CardGame.Core.PromptChoice selected) => _resolve().TryBeginTousuiQinglongSlash(pending, selected);
    internal bool TryChooseTousuiAi(global::CardGame.Core.PendingDecision p, scoped out global::CardGame.Core.PromptChoice choice) => _resolve().TryChooseTousuiAi(p, out choice);
    internal bool ValidTousuiRequest(global::CardGame.Core.TousuiRequestReceipt r) => _resolve().ValidTousuiRequest(r);
    internal void ValidateXumingNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateXumingNativeUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XumingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XumingChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> XumingCurrentMaterials(global::CardGame.Core.XumingPromiseIssuedEvent p) => _resolve().XumingCurrentMaterials(p);
    internal bool XumingDamageEligible(int owner, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().XumingDamageEligible(owner, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.XumingUsePlan> XumingPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XumingPlans(f);
    internal bool HasChengJiChengCui { get => _resolve().HasChengJiChengCui; }
}
