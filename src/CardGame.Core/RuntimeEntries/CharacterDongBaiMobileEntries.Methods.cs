#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongBaiMobileEntries
{
    internal global::CardGame.Core.MobileLianzhuIssuedEvent? ActiveMobileLianzhu(int owner) => _resolve().ActiveMobileLianzhu(owner);
    internal bool AdmitIssuedMobileLianzhu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AdmitIssuedMobileLianzhu(c, context);
    internal bool AdvanceMobileLianzhuSubtree() => _resolve().AdvanceMobileLianzhuSubtree();
    internal void AnswerMobileLianzhuRelay(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileLianzhuRelay(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileLianzhuRelay(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMobileLianzhuRelay(f);
    internal global::CardGame.Core.PromptChoice ChooseMobileLianzhuRelay(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileLianzhuRelay(f);
    internal void CollectMobileLianzhuGains(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().CollectMobileLianzhuGains(batch, candidates);
    internal bool ContinueMobileLianzhu(long id) => _resolve().ContinueMobileLianzhu(id);
    internal void FinishMobileLianzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileLianzhu(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeIssuedMobileLianzhuDamage(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().IncludeIssuedMobileLianzhuDamage(owner, window, candidates);
    internal bool IsMobileLianzhuGain(global::CardGame.Core.MobileLianzhuIssuedEvent issued, global::CardGame.Core.CardMovementRecord m) => _resolve().IsMobileLianzhuGain(issued, m);
    internal bool KeepMobileLianzhuGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileLianzhuGift(f);
    internal bool KeepMobileLianzhuRelay(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepMobileLianzhuRelay(f);
    internal void LockMobileLianzhuEntity(global::CardGame.Core.CardMovedEvent fact) => _resolve().LockMobileLianzhuEntity(fact);
    internal global::CardGame.Core.PromptChoice MobileLianzhuChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().MobileLianzhuChoice(f, branch, text, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileLianzhuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileLianzhuChoices(f);
    internal bool MobileLianzhuDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MobileLianzhuDistance(q);
    internal bool MobileLianzhuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileLianzhuFirstChild(root, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileLianzhuGiftCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileLianzhuGiftCards(f);
    internal bool MobileLianzhuGiftMoved(global::CardGame.Core.MobileLianzhuIssuedEvent e) => _resolve().MobileLianzhuGiftMoved(e);
    internal bool MobileLianzhuGiftPaid(global::CardGame.Core.MobileLianzhuIssuedEvent e) => _resolve().MobileLianzhuGiftPaid(e);
    internal bool MobileLianzhuHolder(int owner, int target) => _resolve().MobileLianzhuHolder(owner, target);
    internal global::CardGame.Core.ProgramSkillFrame? MobileLianzhuObserverRoot() => _resolve().MobileLianzhuObserverRoot();
    internal global::CardGame.Core.SkillGrantPhaseExpiry? MobileLianzhuPhase(int owner) => _resolve().MobileLianzhuPhase(owner);
    internal bool MobileLianzhuQuota(global::CardGame.Core.ProgramTargetQuotaQuery q) => _resolve().MobileLianzhuQuota(q);
    internal bool MobileLianzhuSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().MobileLianzhuSlashDistance(q);
    internal bool MobileLianzhuSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().MobileLianzhuSlashQuota(q);
    internal void MoveMobileLianzhuEntity(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MobileLianzhuStage stage, int card, global::CardGame.Core.CardLocation from, int recipient) => _resolve().MoveMobileLianzhuEntity(f, stage, card, from, recipient);
    internal global::CardGame.Core.SkillProgramStepOutcome PrepareMobileLianzhu(global::CardGame.Core.ProgramSkillFrame f, string? sourceBind) => _resolve().PrepareMobileLianzhu(f, sourceBind);
    internal void PublishMobileLianzhuChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileLianzhuChoice(f);
    internal bool QualifyMobileLianzhuGain(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyMobileLianzhuGain(candidate, context);
    internal bool QualifyMobileLianzhuRecovery(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().QualifyMobileLianzhuRecovery(c, w);
    internal global::CardGame.Core.SkillProgramStepOutcome RecoverMobileLianzhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecoverMobileLianzhu(f);
    internal void RequireMobileLianzhuReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireMobileLianzhuReceipt(f);
    internal void ReturnMobileLianzhuDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileLianzhuDraw(f, outcome);
    internal bool ReturnMobileLianzhuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileLianzhuMovement(f);
    internal bool TryMobileLianzhuGain(int owner, global::CardGame.Core.ProgramSkillWindowContext c, scoped out global::CardGame.Core.MobileLianzhuIssuedEvent issued, scoped out global::CardGame.Core.CardMovementRecord movement) => _resolve().TryMobileLianzhuGain(owner, c, out issued, out movement);
    internal bool ValidMobileLianzhuIssued(global::CardGame.Core.MobileLianzhuIssuedEvent e) => _resolve().ValidMobileLianzhuIssued(e);
}
