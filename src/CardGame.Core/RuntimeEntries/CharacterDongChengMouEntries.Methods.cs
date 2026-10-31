#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongChengMouEntries
{
    internal bool AdvanceMouDcChildren() => _resolve().AdvanceMouDcChildren();
    internal bool BeforeMouDcDelayedCleanup(GameEngine.AttackCompletionReceipt completion) => _resolve().BeforeMouDcDelayedCleanup(completion);
    internal void BindMouDcUse(CardUseFrame use) => _resolve().BindMouDcUse(use);
    internal void ClaimMouDcDelayedCard(ProgramSkillFrame f) => _resolve().ClaimMouDcDelayedCard(f);
    internal bool ContinueMouDcSlash(NativeOwners.CardAttackHandle attack) => _resolve().ContinueMouDcSlash(attack);
    internal bool FinishMouDcZeroAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouDcZeroAttack(attack, out paused);
    internal bool IsMouDcClaimedEntity(long useId, int card) => _resolve().IsMouDcClaimedEntity(useId, card);
    internal bool IsMouDcZeroUse(long id) => _resolve().IsMouDcZeroUse(id);
    internal void IssueMouDcUse(ProgramSkillFrame f) => _resolve().IssueMouDcUse(f);
    internal bool MatchesMouDcNativeAction(long id, CardActionContext a, int card) => _resolve().MatchesMouDcNativeAction(id, a, card);
    internal MouDcOccurrence? MouDcBefore(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MouDcBefore(c, w);
    internal bool MouDcCanActivate(CharacterState owner) => _resolve().MouDcCanActivate(owner);
    internal void MouDcClaim(ProgramSkillFrame f) => _resolve().MouDcClaim(f);
    internal void MouDcClaimCandidates(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().MouDcClaimCandidates(a, window, result);
    internal CardUseFrame? MouDcClaimUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool settled = false) => _resolve().MouDcClaimUse(c, w, settled);
    internal bool MouDcDamageParent(MouDcOccurrence o) => _resolve().MouDcDamageParent(o);
    internal JudgmentFrame? MouDcDelayedDamage(MouDcOccurrence o) => _resolve().MouDcDelayedDamage(o);
    internal bool MouDcDying() => _resolve().MouDcDying();
    internal bool MouDcEdge(int i) => _resolve().MouDcEdge(i);
    internal bool MouDcFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDcFirstChild(f, child);
    internal bool MouDcHasPaidClaim(ProgramSkillFrame f, MouDcOccurrence o, long beforeId) => _resolve().MouDcHasPaidClaim(f, o, beforeId);
    internal bool MouDcInternalSettlementIsNotCommand(CharacterState owner) => _resolve().MouDcInternalSettlementIsNotCommand(owner);
    internal bool MouDcIssuedClaim(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MouDcIssuedClaim(c, w);
    internal bool MouDcMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouDcMovement(f, effect, p);
    internal bool MouDcNativeLightning(JudgmentFrame j) => _resolve().MouDcNativeLightning(j);
    internal bool MouDcParent(ProgramSkillFrame f) => _resolve().MouDcParent(f);
    internal void MouDcProtect(ProgramSkillFrame f) => _resolve().MouDcProtect(f);
    internal bool MouDcRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouDcRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouDcRoot() => _resolve().MouDcRoot();
    internal bool MouDcSuffix(int i) => _resolve().MouDcSuffix(i);
    internal IReadOnlyList<int> MouDcTargets(ProgramSkillFrame f, CardKind kind) => _resolve().MouDcTargets(f, kind);
    internal bool MouDcUseRidesOn(ProgramSkillFrame f, CardUseFrame u) => _resolve().MouDcUseRidesOn(f, u);
    internal MouDcOccurrence? ObserveMouDcCardDamage() => _resolve().ObserveMouDcCardDamage();
    internal CardActionContext PrepareMouDcAction(CardActionContext action) => _resolve().PrepareMouDcAction(action);
    internal Card ReadMouDcUseAppearance(long id, CardAppearanceReference appearance) => _resolve().ReadMouDcUseAppearance(id, appearance);
    internal bool ResumeMouDcAttack() => _resolve().ResumeMouDcAttack();
    internal bool ReturnMouDcUse(CardUseFrame u) => _resolve().ReturnMouDcUse(u);
    internal void ValidateMouDcUse(CardUseFrame u) => _resolve().ValidateMouDcUse(u);
    internal bool HasMouDongCheng { get => _resolve().HasMouDongCheng; }
    internal MouDongChengRuntime MouDongCheng { get => _resolve().MouDongCheng; }
}
