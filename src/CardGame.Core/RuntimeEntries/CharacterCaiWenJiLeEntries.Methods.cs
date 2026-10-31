#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiWenJiLeEntries
{
    internal void AssertBeifenUses() => _resolve().AssertBeifenUses();
    internal SkillProgramStepOutcome BeginBeifen(ProgramSkillFrame supplied) => _resolve().BeginBeifen(supplied);
    internal bool BeifenMotherMatches(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().BeifenMotherMatches(c, context);
    internal DeclaredCardPayment? BeifenOriginalDeclaration(CardUseFrame use) => _resolve().BeifenOriginalDeclaration(use);
    internal bool BeifenSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => _resolve().BeifenSlashQuotaBeforeLimit(query);
    internal bool CanOfferBeifen(ProgramTriggerLegalityContext gate) => _resolve().CanOfferBeifen(gate);
    internal bool CanOfferShuangjia(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShuangjia(gate);
    internal DeclaredCardPayment CaptureBeifenDeclarationPayment(DeclaredCardPayment payment, CardDeclarationParentReturn parent) => _resolve().CaptureBeifenDeclarationPayment(payment, parent);
    internal void CaptureBeifenLossQualifications(CardMovementBatchContext batch, IReadOnlyList<BeifenLossSource> sources) => _resolve().CaptureBeifenLossQualifications(batch, sources);
    internal IReadOnlyList<BeifenLossSource> CaptureBeifenLossSources(IEnumerable<CardLocation> locations) => _resolve().CaptureBeifenLossSources(locations);
    internal BeifenUseQualification? CaptureBeifenUseQualification(CharacterState actor) => _resolve().CaptureBeifenUseQualification(actor);
    internal void CaptureBeifenZeroMaterialNativeUse(IGameEvent payload) => _resolve().CaptureBeifenZeroMaterialNativeUse(payload);
    internal void CaptureMovementBatchStartBeifenLossSources(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartBeifenLossSources(batch);
    internal ProgramLifecycleTriggerWindowFrame CaptureShuangjiaInitialEntities(ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureShuangjiaInitialEntities(window);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectBeifenLossCandidates(CardMovementBatchContext batch) => _resolve().CollectBeifenLossCandidates(batch);
    internal void CollectWindow_CollectBeifenLossCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBeifenLossCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureBeifenLossQualifications(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBeifenLossQualifications(batch, completed);
    internal void ContinueBeifen(long id) => _resolve().ContinueBeifen(id);
    internal bool ContinueIssuedBeifen(ProgramSkillFrame f) => _resolve().ContinueIssuedBeifen(f);
    internal ProgramSkillWindowContext CreateBeifenMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => _resolve().CreateBeifenMovementContext(window, candidate);
    internal bool DistanceBorrowedPaidWeaponSlash10HasBeifenPreUsePrivilege(ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash10HasBeifenPreUsePrivilege(q);
    internal bool DistanceCardUseExemption50HasBeifenUseDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption50HasBeifenUseDistance(q);
    internal bool ExactBeifenLoss(CardMovementBatchContext batch, CardConversionSource source, string hash) => _resolve().ExactBeifenLoss(batch, source, hash);
    internal bool ExcludeOrdinaryMovement_IsBeifenLossTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsBeifenLossTrigger(candidate, trigger);
    internal bool HasBeifenLiveUsePrivilege(CharacterState actor) => _resolve().HasBeifenLiveUsePrivilege(actor);
    internal bool HasBeifenPreUsePrivilege(CharacterState actor) => _resolve().HasBeifenPreUsePrivilege(actor);
    internal bool HasBeifenUseDistance(CharacterState actor, long? existingUseFrameId = default) => _resolve().HasBeifenUseDistance(actor, existingUseFrameId);
    internal bool HasIssuedBeifenUse(long useId, int actorSeat) => _resolve().HasIssuedBeifenUse(useId, actorSeat);
    internal bool HasShuangjiaEntityMark(int cardId) => _resolve().HasShuangjiaEntityMark(cardId);
    internal SkillProgramStepOutcome InitializeShuangjia(ProgramSkillFrame supplied) => _resolve().InitializeShuangjia(supplied);
    internal bool IsBeifenOwnedLoss(CardMovementRecord m, int owner) => _resolve().IsBeifenOwnedLoss(m, owner);
    internal bool IsIssuedBeifenCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedBeifenCandidate(c, context);
    internal bool IsIssuedShuangjiaCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedShuangjiaCandidate(candidate, context);
    internal bool IsShuangjiaHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsShuangjiaHandLimitExempt(owner, cardId);
    internal void IssueBeifenUse(CardUseFrame use) => _resolve().IssueBeifenUse(use);
    internal bool PermitIssuedShuangjia(ProgramSkillFrame frame) => _resolve().PermitIssuedShuangjia(frame);
    internal bool ResumeBeifen(long id) => _resolve().ResumeBeifen(id);
    internal bool ReturnBeifenMovement(ProgramSkillFrame f) => _resolve().ReturnBeifenMovement(f);
    internal ProgramSkillWindowContext? TryCreateBeifenMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateBeifenMovementContext(frame, candidate);
    internal bool ValidBeifenDeclaration(DeclaredCardPayment payment, BeifenUseQualification q) => _resolve().ValidBeifenDeclaration(payment, q);
    internal bool ValidBeifenQualification(BeifenUseQualification q) => _resolve().ValidBeifenQualification(q);
    internal bool ValidBeifenReceipt(ProgramSkillFrame f) => _resolve().ValidBeifenReceipt(f);
    internal bool HasLeCaiWenJiBeifenCapability { get => _resolve().HasLeCaiWenJiBeifenCapability; }
    internal bool HasLeCaiWenJiShuangjiaCapability { get => _resolve().HasLeCaiWenJiShuangjiaCapability; }
    internal ShuangjiaMarkRuntime ShuangjiaMarks { get => _resolve().ShuangjiaMarks; }
}
