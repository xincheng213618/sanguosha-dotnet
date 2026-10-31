#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoMouOlEntries
{
    internal PendingDecision? AddMouXqJueyanRequestChoices(PendingDecision? p) => _resolve().AddMouXqJueyanRequestChoices(p);
    internal bool BeginMouXqForcedSlash(PromptChoice choice) => _resolve().BeginMouXqForcedSlash(choice);
    internal bool BeginMouXqJueyanResponse(PromptChoice selected) => _resolve().BeginMouXqJueyanResponse(selected);
    internal void BeginMouXqProgramSlash(PromptChoice choice) => _resolve().BeginMouXqProgramSlash(choice);
    internal void CancelMouXqResponseAfterReveal(MouXiaoQiaoJueyanResponseRequest request, CharacterState actor) => _resolve().CancelMouXqResponseAfterReveal(request, actor);
    internal bool ContinueMouXqJueyanSimpleUse(long id, ProgramSimpleCardContinuation c) => _resolve().ContinueMouXqJueyanSimpleUse(id, c);
    internal bool FinishMouXqJueyanAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouXqJueyanAttack(attack, out paused);
    internal bool HasMouXqJueyanDyingResponse(CharacterState actor) => _resolve().HasMouXqJueyanDyingResponse(actor);
    internal bool HasMouXqProgramSlash(CharacterState actor, IEnumerable<int> targets) => _resolve().HasMouXqProgramSlash(actor, targets);
    internal bool HasProspectiveMouXqQinglong(CharacterState actor, CharacterState target, NativeOwners.CardAttackHandle attack) => _resolve().HasProspectiveMouXqQinglong(actor, target, attack);
    internal bool IsIssuedMouXqResponse(ResolutionFrame owner, CardActionContext action) => _resolve().IsIssuedMouXqResponse(owner, action);
    internal bool IsMouXqForcedRequest(MouXiaoQiaoJueyanResponseRequest r, ResolutionFrame parent) => _resolve().IsMouXqForcedRequest(r, parent);
    internal bool IsMouXqJueyanResponseRequest(ProgramSkillFrame f) => _resolve().IsMouXqJueyanResponseRequest(f);
    internal bool IsMouXqJueyanUse(long id) => _resolve().IsMouXqJueyanUse(id);
    internal bool IsMouXqNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => _resolve().IsMouXqNativeSettlementBatch(f, b);
    internal bool IsMouXqProgramRequest(ProgramSkillFrame f, MouXiaoQiaoJueyanResponseRequest r) => _resolve().IsMouXqProgramRequest(f, r);
    internal bool IsPublicHandRevealProducerSuspended(long parentId, string key) => _resolve().IsPublicHandRevealProducerSuspended(parentId, key);
    internal void IssueMouXqCounterspell(CharacterState actor, MouXiaoQiaoJueyanResponseGrant grant) => _resolve().IssueMouXqCounterspell(actor, grant);
    internal void IssueMouXqDodge(CharacterState actor, MouXiaoQiaoJueyanResponseGrant grant) => _resolve().IssueMouXqDodge(actor, grant);
    internal void IssueMouXqNativeUse(ProgramSkillFrame f, MouXiaoQiaoOlUsePlan p) => _resolve().IssueMouXqNativeUse(f, p);
    internal void IssueMouXqRequestedSlash(MouXiaoQiaoJueyanResponseGrant grant) => _resolve().IssueMouXqRequestedSlash(grant);
    internal IReadOnlyList<PromptChoice> MouXqForcedSlashChoices(CharacterState actor, CharacterState target, long parentId, bool qinglong) => _resolve().MouXqForcedSlashChoices(actor, target, parentId, qinglong);
    internal Card MouXqJueyanRepresentation(long id) => _resolve().MouXqJueyanRepresentation(id);
    internal IReadOnlyList<PromptChoice> MouXqJueyanResponseChoices(CharacterState actor, CardKind kind, bool dying = false) => _resolve().MouXqJueyanResponseChoices(actor, kind, dying);
    internal IReadOnlyList<MouXiaoQiaoOlUsePlan> MouXqNativePlans(CharacterState actor, CardKind kind, CardConversionSource source) => _resolve().MouXqNativePlans(actor, kind, source);
    internal IReadOnlyList<PromptChoice> MouXqProgramSlashChoices(ProgramSkillFrame f, int actorSeat, IReadOnlyList<int> targets, string? resultBind = default) => _resolve().MouXqProgramSlashChoices(f, actorSeat, targets, resultBind);
    internal CardActionContext PrepareMouXqJueyanAction(CardActionContext action) => _resolve().PrepareMouXqJueyanAction(action);
    internal void PrepareMouXqProgramSlash(MouXiaoQiaoJueyanResponseRequest r) => _resolve().PrepareMouXqProgramSlash(r);
    internal bool PreserveMouXqRequestedSlash(ProgramSkillFrame f) => _resolve().PreserveMouXqRequestedSlash(f);
    internal IReadOnlyList<Card> PublicRevealInvariantProcessing(IReadOnlyList<Card> cards) => _resolve().PublicRevealInvariantProcessing(cards);
    internal Card ReadMouXqJueyanAppearance(long id, CardAppearanceReference a) => _resolve().ReadMouXqJueyanAppearance(id, a);
    internal bool ResumeMouXqProgramSlash(long id) => _resolve().ResumeMouXqProgramSlash(id);
    internal void ReturnDiouPublicHand(ProgramSkillFrame f) => _resolve().ReturnDiouPublicHand(f);
    internal void ReturnFireAttackRetainedPublicHand(CardUseFrame pending) => _resolve().ReturnFireAttackRetainedPublicHand(pending);
    internal void ReturnHandComparisonSource(ProgramSkillFrame f) => _resolve().ReturnHandComparisonSource(f);
    internal void ReturnHezhongPublicHand(ProgramSkillFrame f) => _resolve().ReturnHezhongPublicHand(f);
    internal void ReturnMouXqJueyanResponse(ProgramSkillFrame f) => _resolve().ReturnMouXqJueyanResponse(f);
    internal void ReturnMouXqProgramSlash(CardUseFrame use) => _resolve().ReturnMouXqProgramSlash(use);
    internal void ReturnPhasePredictionPublicHand(ProgramSkillFrame f) => _resolve().ReturnPhasePredictionPublicHand(f);
    internal void ReturnQingliangPublicHand(ProgramSkillFrame f) => _resolve().ReturnQingliangPublicHand(f);
    internal void ReturnQixiPublicHand(ProgramSkillFrame f) => _resolve().ReturnQixiPublicHand(f);
    internal void ReturnXiashuPublicHand(ProgramSkillFrame f) => _resolve().ReturnXiashuPublicHand(f);
    internal void SignMouXqNativeBatches(ProgramSkillFrame f, CardUseFrame use) => _resolve().SignMouXqNativeBatches(f, use);
    internal void SignMouXqResponse(ResolutionFrame owner, MouXiaoQiaoJueyanResponseGrant grant, CardActionContext action) => _resolve().SignMouXqResponse(owner, grant, action);
    internal bool SkipMouXqJueyanFinishedMovement(long id, Card c) => _resolve().SkipMouXqJueyanFinishedMovement(id, c);
    internal void StartMouXqRequestedReveal(PendingDecision prompt, CardConversionSource source, int cardId, MouXiaoQiaoJueyanResponseRequest request) => _resolve().StartMouXqRequestedReveal(prompt, source, cardId, request);
    internal bool TryBeginPublicHandRevealWindow(long parentId, PublicHandRevealReturnKind returned) => _resolve().TryBeginPublicHandRevealWindow(parentId, returned);
    internal bool TryBeginPublicHandRevealWindow2(long parentId, string key) => _resolve().TryBeginPublicHandRevealWindow(parentId, key);
    internal bool TryMouXqJueyanAiResponse(CardKind kind, bool dying = false) => _resolve().TryMouXqJueyanAiResponse(kind, dying);
    internal bool WindowSettlement_IsMouXqNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMouXqNativeSettlementBatch(frame, batch);
    internal MouXiaoQiaoOlRuntime MouXiaoQiaoOl { get => _resolve().MouXiaoQiaoOl; }
}
