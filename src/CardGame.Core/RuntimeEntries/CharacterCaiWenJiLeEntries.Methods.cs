#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiWenJiLeEntries
{
    internal void AssertBeifenUses() => _resolve().AssertBeifenUses();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeifen(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBeifen(supplied);
    internal bool BeifenMotherMatches(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BeifenMotherMatches(c, context);
    internal global::CardGame.Core.DeclaredCardPayment? BeifenOriginalDeclaration(global::CardGame.Core.CardUseFrame use) => _resolve().BeifenOriginalDeclaration(use);
    internal bool BeifenSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().BeifenSlashQuotaBeforeLimit(query);
    internal bool CanOfferBeifen(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferBeifen(gate);
    internal bool CanOfferShuangjia(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShuangjia(gate);
    internal global::CardGame.Core.DeclaredCardPayment CaptureBeifenDeclarationPayment(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => _resolve().CaptureBeifenDeclarationPayment(payment, parent);
    internal void CaptureBeifenLossQualifications(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BeifenLossSource> sources) => _resolve().CaptureBeifenLossQualifications(batch, sources);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.BeifenLossSource> CaptureBeifenLossSources(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> locations) => _resolve().CaptureBeifenLossSources(locations);
    internal global::CardGame.Core.BeifenUseQualification? CaptureBeifenUseQualification(global::CardGame.Core.CharacterState actor) => _resolve().CaptureBeifenUseQualification(actor);
    internal void CaptureBeifenZeroMaterialNativeUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureBeifenZeroMaterialNativeUse(payload);
    internal void CaptureMovementBatchStartBeifenLossSources(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartBeifenLossSources(batch);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureShuangjiaInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureShuangjiaInitialEntities(window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectBeifenLossCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectBeifenLossCandidates(batch);
    internal void CollectWindow_CollectBeifenLossCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectBeifenLossCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureBeifenLossQualifications(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureBeifenLossQualifications(batch, completed);
    internal void ContinueBeifen(long id) => _resolve().ContinueBeifen(id);
    internal bool ContinueIssuedBeifen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedBeifen(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateBeifenMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateBeifenMovementContext(window, candidate);
    internal bool DistanceBorrowedPaidWeaponSlash10HasBeifenPreUsePrivilege(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceBorrowedPaidWeaponSlash10HasBeifenPreUsePrivilege(q);
    internal bool DistanceCardUseExemption50HasBeifenUseDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption50HasBeifenUseDistance(q);
    internal bool ExactBeifenLoss(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardConversionSource source, string hash) => _resolve().ExactBeifenLoss(batch, source, hash);
    internal bool ExcludeOrdinaryMovement_IsBeifenLossTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsBeifenLossTrigger(candidate, trigger);
    internal bool HasBeifenLiveUsePrivilege(global::CardGame.Core.CharacterState actor) => _resolve().HasBeifenLiveUsePrivilege(actor);
    internal bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor) => _resolve().HasBeifenPreUsePrivilege(actor);
    internal bool HasBeifenUseDistance(global::CardGame.Core.CharacterState actor, long? existingUseFrameId = default) => _resolve().HasBeifenUseDistance(actor, existingUseFrameId);
    internal bool HasIssuedBeifenUse(long useId, int actorSeat) => _resolve().HasIssuedBeifenUse(useId, actorSeat);
    internal bool HasShuangjiaEntityMark(int cardId) => _resolve().HasShuangjiaEntityMark(cardId);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeShuangjia(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeShuangjia(supplied);
    internal bool IsBeifenOwnedLoss(global::CardGame.Core.CardMovementRecord m, int owner) => _resolve().IsBeifenOwnedLoss(m, owner);
    internal bool IsIssuedBeifenCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedBeifenCandidate(c, context);
    internal bool IsIssuedShuangjiaCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShuangjiaCandidate(candidate, context);
    internal bool IsShuangjiaHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsShuangjiaHandLimitExempt(owner, cardId);
    internal void IssueBeifenUse(global::CardGame.Core.CardUseFrame use) => _resolve().IssueBeifenUse(use);
    internal bool PermitIssuedShuangjia(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PermitIssuedShuangjia(frame);
    internal bool ResumeBeifen(long id) => _resolve().ResumeBeifen(id);
    internal bool ReturnBeifenMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBeifenMovement(f);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateBeifenMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateBeifenMovementContext(frame, candidate);
    internal bool ValidBeifenDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.BeifenUseQualification q) => _resolve().ValidBeifenDeclaration(payment, q);
    internal bool ValidBeifenQualification(global::CardGame.Core.BeifenUseQualification q) => _resolve().ValidBeifenQualification(q);
    internal bool ValidBeifenReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBeifenReceipt(f);
    internal bool HasLeCaiWenJiBeifenCapability { get => _resolve().HasLeCaiWenJiBeifenCapability; }
    internal bool HasLeCaiWenJiShuangjiaCapability { get => _resolve().HasLeCaiWenJiShuangjiaCapability; }
    internal global::CardGame.Core.ShuangjiaMarkRuntime ShuangjiaMarks { get => _resolve().ShuangjiaMarks; }
}
