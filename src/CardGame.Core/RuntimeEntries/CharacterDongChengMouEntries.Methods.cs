#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongChengMouEntries
{
    internal bool AdvanceMouDcChildren() => _resolve().AdvanceMouDcChildren();
    internal bool BeforeMouDcDelayedCleanup(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().BeforeMouDcDelayedCleanup(completion);
    internal void BindMouDcUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouDcUse(use);
    internal void ClaimMouDcDelayedCard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ClaimMouDcDelayedCard(f);
    internal bool ContinueMouDcSlash(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ContinueMouDcSlash(attack);
    internal bool FinishMouDcZeroAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouDcZeroAttack(attack, out paused);
    internal bool IsMouDcClaimedEntity(long useId, int card) => _resolve().IsMouDcClaimedEntity(useId, card);
    internal bool IsMouDcZeroUse(long id) => _resolve().IsMouDcZeroUse(id);
    internal void IssueMouDcUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouDcUse(f);
    internal bool MatchesMouDcNativeAction(long id, global::CardGame.Core.CardActionContext a, int card) => _resolve().MatchesMouDcNativeAction(id, a, card);
    internal global::CardGame.Core.MouDcOccurrence? MouDcBefore(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().MouDcBefore(c, w);
    internal bool MouDcCanActivate(global::CardGame.Core.CharacterState owner) => _resolve().MouDcCanActivate(owner);
    internal void MouDcClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDcClaim(f);
    internal void MouDcClaimCandidates(global::CardGame.Core.CardActionContext a, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().MouDcClaimCandidates(a, window, result);
    internal global::CardGame.Core.CardUseFrame? MouDcClaimUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w, bool settled = false) => _resolve().MouDcClaimUse(c, w, settled);
    internal bool MouDcDamageParent(global::CardGame.Core.MouDcOccurrence o) => _resolve().MouDcDamageParent(o);
    internal global::CardGame.Core.JudgmentFrame? MouDcDelayedDamage(global::CardGame.Core.MouDcOccurrence o) => _resolve().MouDcDelayedDamage(o);
    internal bool MouDcDying() => _resolve().MouDcDying();
    internal bool MouDcEdge(int i) => _resolve().MouDcEdge(i);
    internal bool MouDcFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDcFirstChild(f, child);
    internal bool MouDcHasPaidClaim(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MouDcOccurrence o, long beforeId) => _resolve().MouDcHasPaidClaim(f, o, beforeId);
    internal bool MouDcInternalSettlementIsNotCommand(global::CardGame.Core.CharacterState owner) => _resolve().MouDcInternalSettlementIsNotCommand(owner);
    internal bool MouDcIssuedClaim(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().MouDcIssuedClaim(c, w);
    internal bool MouDcMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouDcMovement(f, effect, p);
    internal bool MouDcNativeLightning(global::CardGame.Core.JudgmentFrame j) => _resolve().MouDcNativeLightning(j);
    internal bool MouDcParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDcParent(f);
    internal void MouDcProtect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDcProtect(f);
    internal bool MouDcRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouDcRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouDcRoot() => _resolve().MouDcRoot();
    internal bool MouDcSuffix(int i) => _resolve().MouDcSuffix(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouDcTargets(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardKind kind) => _resolve().MouDcTargets(f, kind);
    internal bool MouDcUseRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame u) => _resolve().MouDcUseRidesOn(f, u);
    internal global::CardGame.Core.MouDcOccurrence? ObserveMouDcCardDamage() => _resolve().ObserveMouDcCardDamage();
    internal global::CardGame.Core.CardActionContext PrepareMouDcAction(global::CardGame.Core.CardActionContext action) => _resolve().PrepareMouDcAction(action);
    internal global::CardGame.Core.Card ReadMouDcUseAppearance(long id, global::CardGame.Core.CardAppearanceReference appearance) => _resolve().ReadMouDcUseAppearance(id, appearance);
    internal bool ResumeMouDcAttack() => _resolve().ResumeMouDcAttack();
    internal bool ReturnMouDcUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouDcUse(u);
    internal void ValidateMouDcUse(global::CardGame.Core.CardUseFrame u) => _resolve().ValidateMouDcUse(u);
    internal bool HasMouDongCheng { get => _resolve().HasMouDongCheng; }
    internal global::CardGame.Core.MouDongChengRuntime MouDongCheng { get => _resolve().MouDongCheng; }
}
