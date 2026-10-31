#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouMobileEntries
{
    internal bool AdvanceMouKurouObserverChildren() => _resolve().AdvanceMouKurouObserverChildren();
    internal void AnswerMouKurouGift(PromptChoice selected) => _resolve().AnswerMouKurouGift(selected);
    internal void AssertMouZhaJiangAdmissions() => _resolve().AssertMouZhaJiangAdmissions();
    internal bool AwaitMouKurouChildren(long id) => _resolve().AwaitMouKurouChildren(id);
    internal SkillProgramStepOutcome BeginMouKurouGift(ProgramSkillFrame input) => _resolve().BeginMouKurouGift(input);
    internal bool CanOfferMouKurouGift(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouKurouGift(gate);
    internal void CaptureMouKurouNativeLoss(HpChangeContext change) => _resolve().CaptureMouKurouNativeLoss(change);
    internal MouZhaJiangAdmission CaptureMouZhaJiangAdmission(CharacterState actor) => _resolve().CaptureMouZhaJiangAdmission(actor);
    internal DeclaredCardPayment CaptureMouZhaJiangDeclaration(DeclaredCardPayment payment, CardDeclarationParentReturn parent) => _resolve().CaptureMouZhaJiangDeclaration(payment, parent);
    internal void CompleteMouKurou(ProgramSkillFrame frame) => _resolve().CompleteMouKurou(frame);
    internal void ContinueMouKurou(long id) => _resolve().ContinueMouKurou(id);
    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouKurouGiftForAi(CharacterState owner, ProgramAiPublicContext publicContext) => _resolve().EstimateMouKurouGiftForAi(owner, publicContext);
    internal bool ExactMouKurouArmorFrame(ProgramSkillFrame frame) => _resolve().ExactMouKurouArmorFrame(frame);
    internal bool ExactMouKurouGiftParent(ProgramSkillFrame frame) => _resolve().ExactMouKurouGiftParent(frame);
    internal void GrantMouKurouLossArmor(ProgramSkillFrame input) => _resolve().GrantMouKurouLossArmor(input);
    internal bool HasIssuedMouZhaJiangUse(long useId, int actorSeat) => _resolve().HasIssuedMouZhaJiangUse(useId, actorSeat);
    internal bool HasMouZhaJiangPreUsePrivilege(CharacterState actor) => _resolve().HasMouZhaJiangPreUsePrivilege(actor);
    internal bool HasMouZhaJiangUseDistance(CharacterState actor, long? useId = default) => _resolve().HasMouZhaJiangUseDistance(actor, useId);
    internal bool IsMouKurouDying() => _resolve().IsMouKurouDying();
    internal bool IsMouKurouRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsMouKurouRecoveryParent(frame, recovery);
    internal bool IsMouZhaJiangUnrespondable(long useId) => _resolve().IsMouZhaJiangUnrespondable(useId);
    internal void IssueMouZhaJiangLegacyDuel(ProgramSkillFrame owner, int actorSeat) => _resolve().IssueMouZhaJiangLegacyDuel(owner, actorSeat);
    internal void IssueMouZhaJiangLegacyDuelPipeline(ProgramSkillFrame frame, int sourceSeat, int targetSeat) => _resolve().IssueMouZhaJiangLegacyDuelPipeline(frame, sourceSeat, targetSeat);
    internal void IssueMouZhaJiangUse(CardUseFrame use) => _resolve().IssueMouZhaJiangUse(use);
    internal IReadOnlyList<PromptChoice> MouKurouChoices(ProgramSkillFrame frame) => _resolve().MouKurouChoices(frame);
    internal bool MouKurouFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().MouKurouFirstChild(frame, child);
    internal CardKind MouKurouGiftKind(CharacterState owner, Card card, CardLocation from) => _resolve().MouKurouGiftKind(owner, card, from);
    internal ProgramSkillFrame? MouKurouObserverRoot() => _resolve().MouKurouObserverRoot();
    internal bool MouKurouObserverSuffix(int index) => _resolve().MouKurouObserverSuffix(index);
    internal bool MouKurouRecoveryIsPubliclyCertain(GameSnapshot snapshot, int ownerSeat) => _resolve().MouKurouRecoveryIsPubliclyCertain(snapshot, ownerSeat);
    internal CardConversionSource? MouZhaJiangCounterspellSource(CharacterState actor, IReadOnlyList<int> ids) => _resolve().MouZhaJiangCounterspellSource(actor, ids);
    internal int MouZhaJiangPendingDeclarations(int actor, CurrentTurnCardUseKindsRecordedEvent[] facts) => _resolve().MouZhaJiangPendingDeclarations(actor, facts);
    internal int MouZhaJiangProspectiveOrdinal(int actor) => _resolve().MouZhaJiangProspectiveOrdinal(actor);
    internal ResolutionFrame? MouZhaJiangResponseOwner(int actor, CardKind kind) => _resolve().MouZhaJiangResponseOwner(actor, kind);
    internal bool MouZhaJiangSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().MouZhaJiangSlashQuotaBeforeLimit(query);
    internal void ObserveMouZhaJiangAdmission(IGameEvent payload) => _resolve().ObserveMouZhaJiangAdmission(payload);
    internal bool PermitPaidMouKurou(ProgramSkillFrame frame) => _resolve().PermitPaidMouKurou(frame);
    internal void PrepareMouZhaJiangResponse(CharacterState actor, IReadOnlyList<int> ids, CardKind kind) => _resolve().PrepareMouZhaJiangResponse(actor, ids, kind);
    internal IReadOnlyList<GameEngine.MouKurouAiCandidate> RankMouKurouAiCandidates(CharacterState owner, ProgramAiPublicContext publicContext, IReadOnlyList<PromptChoice>? publishedChoices = default) => _resolve().RankMouKurouAiCandidates(owner, publicContext, publishedChoices);
    internal void RecordMouZhaJiangAwareAlcoholDebit(long useId, CharacterState actor) => _resolve().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
    internal bool ResumeMouKurou(long id) => _resolve().ResumeMouKurou(id);
    internal bool ReturnMouKurouMovement(ProgramSkillFrame frame) => _resolve().ReturnMouKurouMovement(frame);
    internal PromptChoice SelectAiMouKurouGift(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiMouKurouGift(decision, frame);
    internal bool ValidMouKurou(ProgramSkillFrame frame) => _resolve().ValidMouKurou(frame);
    internal bool ValidMouZhaJiangAdmission(MouZhaJiangAdmission q) => _resolve().ValidMouZhaJiangAdmission(q);
    internal bool ValidMouZhaJiangDeclaration(DeclaredCardPayment p, MouZhaJiangAdmission q) => _resolve().ValidMouZhaJiangDeclaration(p, q);
    internal bool HasMouZhaJiangCapability { get => _resolve().HasMouZhaJiangCapability; }
}
