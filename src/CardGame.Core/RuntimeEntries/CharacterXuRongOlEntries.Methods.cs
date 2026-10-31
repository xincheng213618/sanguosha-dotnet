#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuRongOlEntries
{
    internal void ArmXuRongCausalClaim(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.XuRongDyingProof proof, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().ArmXuRongCausalClaim(f, proof, ids);
    internal bool CanActivateXuRong(int owner, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanActivateXuRong(owner, activation);
    internal bool CanContinueIssuedXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedXuRong(f);
    internal bool CanOfferXuRongLifecycle(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferXuRongLifecycle(candidate, context);
    internal bool CanTargetXuRong(int owner, int target, global::CardGame.Core.SkillProgramActivation activation) => _resolve().CanTargetXuRong(owner, target, activation);
    internal void CaptureXuRongActualDamageHp(long damageFrameId, global::CardGame.Core.GameEngine.IDamageAttempt attack, int hpBefore, int actualAmount) => _resolve().CaptureXuRongActualDamageHp(damageFrameId, attack, hpBefore, actualAmount);
    internal global::CardGame.Core.SkillProgramStepOutcome ClaimXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ClaimXuRong(f);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateXuRongDyingContext(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateXuRongDyingContext(context);
    internal bool ExactXuRongParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.XuRongPending r) => _resolve().ExactXuRongParent(f, r);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteXuRong(global::CardGame.Core.SkillProgramEffectOp op, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExecuteXuRong(op, f);
    internal bool HasXuRongBeforeDamageObserver(long id) => _resolve().HasXuRongBeforeDamageObserver(id);
    internal bool HasXuRongDamageObserver(long id) => _resolve().HasXuRongDamageObserver(id);
    internal bool HasXuRongDyingQualification(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasXuRongDyingQualification(candidate, context);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().InitializeXuRong(f);
    internal bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId) => _resolve().IsXuRongCausalEntityClaim(nativeOwnerId, cardId);
    internal bool IsXuRongDying() => _resolve().IsXuRongDying();
    internal bool IsXuRongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsXuRongMovement(f, e, p);
    internal bool IsXuRongSlashTargetForbidden(int actor, int target, global::CardGame.Core.CardKind kind) => _resolve().IsXuRongSlashTargetForbidden(actor, target, kind);
    internal void IssueXuRongMove(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, string reason) => _resolve().IssueXuRongMove(f, ids, reason);
    internal bool LiveXionghuoSource(global::CardGame.Core.XionghuoGrant grant) => _resolve().LiveXionghuoSource(grant);
    internal void ObserveXuRongDyingQualification(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveXuRongDyingQualification(payload);
    internal void ObserveXuRongTurnExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveXuRongTurnExpiry(payload);
    internal void RemoveXuRongTargetMarkers(global::CardGame.Core.ProgramSkillFrame f, int subject, int source, int count) => _resolve().RemoveXuRongTargetMarkers(f, subject, source, count);
    internal void RequireIssuedXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIssuedXuRong(f);
    internal bool ResumeXuRong(long id) => _resolve().ResumeXuRong(id);
    internal bool ReturnXuRongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnXuRongMovement(f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleXuRong(global::CardGame.Core.ProgramSkillFrame input) => _resolve().SettleXuRong(input);
    internal void TakeXuRongRandomCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TakeXuRongRandomCards(f);
    internal global::CardGame.Core.SkillProgramStepOutcome TransferXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TransferXuRong(f);
    internal bool TryAdvanceXuRongSubtree() => _resolve().TryAdvanceXuRongSubtree();
    internal bool ValidXuRong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidXuRong(f);
    internal int XionghuoActualDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().XionghuoActualDamageBonus(attack);
    internal bool XuRongDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().XuRongDirectedTargetProhibited(query);
    internal bool XuRongFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().XuRongFirstChild(root, child);
    internal int XuRongHandLimitPenalty(global::CardGame.Core.CharacterState owner) => _resolve().XuRongHandLimitPenalty(owner);
    internal global::CardGame.Core.ProgramSkillFrame? XuRongObserverRoot() => _resolve().XuRongObserverRoot();
    internal int XuRongPool(int owner) => _resolve().XuRongPool(owner);
    internal global::CardGame.Core.CardConversionSource XuRongSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().XuRongSource(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.XionghuoGrant> XuRongSubjectGrants(int owner, int subject) => _resolve().XuRongSubjectGrants(owner, subject);
}
