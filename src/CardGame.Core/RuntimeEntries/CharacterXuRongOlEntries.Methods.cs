#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuRongOlEntries
{
    internal void ArmXuRongCausalClaim(ProgramSkillFrame f, XuRongDyingProof proof, IReadOnlyList<int> ids) => _resolve().ArmXuRongCausalClaim(f, proof, ids);
    internal bool CanActivateXuRong(int owner, SkillProgramActivation activation) => _resolve().CanActivateXuRong(owner, activation);
    internal bool CanContinueIssuedXuRong(ProgramSkillFrame f) => _resolve().CanContinueIssuedXuRong(f);
    internal bool CanOfferXuRongLifecycle(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CanOfferXuRongLifecycle(candidate, context);
    internal bool CanTargetXuRong(int owner, int target, SkillProgramActivation activation) => _resolve().CanTargetXuRong(owner, target, activation);
    internal void CaptureXuRongActualDamageHp(long damageFrameId, GameEngine.IDamageAttempt attack, int hpBefore, int actualAmount) => _resolve().CaptureXuRongActualDamageHp(damageFrameId, attack, hpBefore, actualAmount);
    internal SkillProgramStepOutcome ClaimXuRong(ProgramSkillFrame f) => _resolve().ClaimXuRong(f);
    internal ProgramSkillWindowContext CreateXuRongDyingContext(ProgramSkillWindowContext context) => _resolve().CreateXuRongDyingContext(context);
    internal bool ExactXuRongParent(ProgramSkillFrame f, XuRongPending r) => _resolve().ExactXuRongParent(f, r);
    internal SkillProgramStepOutcome ExecuteXuRong(SkillProgramEffectOp op, ProgramSkillFrame f) => _resolve().ExecuteXuRong(op, f);
    internal bool HasXuRongDyingQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasXuRongDyingQualification(candidate, context);
    internal SkillProgramStepOutcome InitializeXuRong(ProgramSkillFrame f) => _resolve().InitializeXuRong(f);
    internal bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId) => _resolve().IsXuRongCausalEntityClaim(nativeOwnerId, cardId);
    internal bool IsXuRongSlashTargetForbidden(int actor, int target, CardKind kind) => _resolve().IsXuRongSlashTargetForbidden(actor, target, kind);
    internal void IssueXuRongMove(ProgramSkillFrame f, IReadOnlyList<int> ids, string reason) => _resolve().IssueXuRongMove(f, ids, reason);
    internal bool LiveXionghuoSource(XionghuoGrant grant) => _resolve().LiveXionghuoSource(grant);
    internal void ObserveXuRongDyingQualification(IGameEvent payload) => _resolve().ObserveXuRongDyingQualification(payload);
    internal void ObserveXuRongTurnExpiry(IGameEvent payload) => _resolve().ObserveXuRongTurnExpiry(payload);
    internal void RemoveXuRongTargetMarkers(ProgramSkillFrame f, int subject, int source, int count) => _resolve().RemoveXuRongTargetMarkers(f, subject, source, count);
    internal bool ResumeXuRong(long id) => _resolve().ResumeXuRong(id);
    internal bool ReturnXuRongMovement(ProgramSkillFrame f) => _resolve().ReturnXuRongMovement(f);
    internal SkillProgramStepOutcome SettleXuRong(ProgramSkillFrame input) => _resolve().SettleXuRong(input);
    internal void TakeXuRongRandomCards(ProgramSkillFrame f) => _resolve().TakeXuRongRandomCards(f);
    internal SkillProgramStepOutcome TransferXuRong(ProgramSkillFrame f) => _resolve().TransferXuRong(f);
    internal bool TryAdvanceXuRongSubtree() => _resolve().TryAdvanceXuRongSubtree();
    internal bool ValidXuRong(ProgramSkillFrame f) => _resolve().ValidXuRong(f);
    internal int XionghuoActualDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().XionghuoActualDamageBonus(attack);
    internal bool XuRongDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().XuRongDirectedTargetProhibited(query);
    internal bool XuRongFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().XuRongFirstChild(root, child);
    internal int XuRongHandLimitPenalty(CharacterState owner) => _resolve().XuRongHandLimitPenalty(owner);
    internal ProgramSkillFrame? XuRongObserverRoot() => _resolve().XuRongObserverRoot();
    internal int XuRongPool(int owner) => _resolve().XuRongPool(owner);
    internal CardConversionSource XuRongSource(ProgramSkillFrame f) => _resolve().XuRongSource(f);
    internal IEnumerable<XionghuoGrant> XuRongSubjectGrants(int owner, int subject) => _resolve().XuRongSubjectGrants(owner, subject);
}
