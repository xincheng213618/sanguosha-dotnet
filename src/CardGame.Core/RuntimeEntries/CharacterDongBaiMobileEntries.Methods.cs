#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongBaiMobileEntries
{
    internal MobileLianzhuIssuedEvent? ActiveMobileLianzhu(int owner) => _resolve().ActiveMobileLianzhu(owner);
    internal bool AdmitIssuedMobileLianzhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().AdmitIssuedMobileLianzhu(c, context);
    internal bool AdvanceMobileLianzhuSubtree() => _resolve().AdvanceMobileLianzhuSubtree();
    internal void AnswerMobileLianzhuRelay(PromptChoice choice) => _resolve().AnswerMobileLianzhuRelay(choice);
    internal SkillProgramStepOutcome BeginMobileLianzhuRelay(ProgramSkillFrame f) => _resolve().BeginMobileLianzhuRelay(f);
    internal PromptChoice ChooseMobileLianzhuRelay(ProgramSkillFrame f) => _resolve().ChooseMobileLianzhuRelay(f);
    internal void CollectMobileLianzhuGains(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().CollectMobileLianzhuGains(batch, candidates);
    internal bool ContinueMobileLianzhu(long id) => _resolve().ContinueMobileLianzhu(id);
    internal void FinishMobileLianzhu(ProgramSkillFrame f) => _resolve().FinishMobileLianzhu(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeIssuedMobileLianzhuDamage(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates) => _resolve().IncludeIssuedMobileLianzhuDamage(owner, window, candidates);
    internal bool IsMobileLianzhuGain(MobileLianzhuIssuedEvent issued, CardMovementRecord m) => _resolve().IsMobileLianzhuGain(issued, m);
    internal bool KeepMobileLianzhuGift(ProgramSkillFrame f) => _resolve().KeepMobileLianzhuGift(f);
    internal bool KeepMobileLianzhuRelay(ProgramSkillFrame f) => _resolve().KeepMobileLianzhuRelay(f);
    internal void LockMobileLianzhuEntity(CardMovedEvent fact) => _resolve().LockMobileLianzhuEntity(fact);
    internal PromptChoice MobileLianzhuChoice(ProgramSkillFrame f, string branch, string text, IReadOnlyList<int>? cards = default) => _resolve().MobileLianzhuChoice(f, branch, text, cards);
    internal IReadOnlyList<PromptChoice> MobileLianzhuChoices(ProgramSkillFrame f) => _resolve().MobileLianzhuChoices(f);
    internal bool MobileLianzhuDistance(ProgramCardDistanceQuery q) => _resolve().MobileLianzhuDistance(q);
    internal bool MobileLianzhuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().MobileLianzhuFirstChild(root, child);
    internal IReadOnlyList<PromptChoice> MobileLianzhuGiftCards(ProgramSkillFrame f) => _resolve().MobileLianzhuGiftCards(f);
    internal bool MobileLianzhuGiftMoved(MobileLianzhuIssuedEvent e) => _resolve().MobileLianzhuGiftMoved(e);
    internal bool MobileLianzhuGiftPaid(MobileLianzhuIssuedEvent e) => _resolve().MobileLianzhuGiftPaid(e);
    internal bool MobileLianzhuHolder(int owner, int target) => _resolve().MobileLianzhuHolder(owner, target);
    internal ProgramSkillFrame? MobileLianzhuObserverRoot() => _resolve().MobileLianzhuObserverRoot();
    internal SkillGrantPhaseExpiry? MobileLianzhuPhase(int owner) => _resolve().MobileLianzhuPhase(owner);
    internal bool MobileLianzhuQuota(ProgramTargetQuotaQuery q) => _resolve().MobileLianzhuQuota(q);
    internal bool MobileLianzhuSlashDistance(ProgramSlashDistanceQuery q) => _resolve().MobileLianzhuSlashDistance(q);
    internal bool MobileLianzhuSlashQuota(ProgramSlashQuotaQuery q) => _resolve().MobileLianzhuSlashQuota(q);
    internal void MoveMobileLianzhuEntity(ProgramSkillFrame f, MobileLianzhuStage stage, int card, CardLocation from, int recipient) => _resolve().MoveMobileLianzhuEntity(f, stage, card, from, recipient);
    internal SkillProgramStepOutcome PrepareMobileLianzhu(ProgramSkillFrame f, string? sourceBind) => _resolve().PrepareMobileLianzhu(f, sourceBind);
    internal void PublishMobileLianzhuChoice(ProgramSkillFrame f) => _resolve().PublishMobileLianzhuChoice(f);
    internal bool QualifyMobileLianzhuGain(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().QualifyMobileLianzhuGain(candidate, context);
    internal bool QualifyMobileLianzhuRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().QualifyMobileLianzhuRecovery(c, w);
    internal SkillProgramStepOutcome RecoverMobileLianzhu(ProgramSkillFrame f) => _resolve().RecoverMobileLianzhu(f);
    internal void RequireMobileLianzhuReceipt(ProgramSkillFrame f) => _resolve().RequireMobileLianzhuReceipt(f);
    internal void ReturnMobileLianzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMobileLianzhuDraw(f, outcome);
    internal bool ReturnMobileLianzhuMovement(ProgramSkillFrame f) => _resolve().ReturnMobileLianzhuMovement(f);
    internal bool TryMobileLianzhuGain(int owner, ProgramSkillWindowContext c, scoped out MobileLianzhuIssuedEvent issued, scoped out CardMovementRecord movement) => _resolve().TryMobileLianzhuGain(owner, c, out issued, out movement);
    internal bool ValidMobileLianzhuIssued(MobileLianzhuIssuedEvent e) => _resolve().ValidMobileLianzhuIssued(e);
}
