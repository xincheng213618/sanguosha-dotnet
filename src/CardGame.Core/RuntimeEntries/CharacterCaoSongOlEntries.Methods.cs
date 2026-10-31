#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoSongOlEntries
{
    internal CaoSongYizhengGrantedEvent[] ActiveCaoSongYizhengGrants(int beneficiary) => _resolve().ActiveCaoSongYizhengGrants(beneficiary);
    internal void AnswerCaoSongLilu(PromptChoice choice) => _resolve().AnswerCaoSongLilu(choice);
    internal bool ApplyCaoSongDamageSupport() => _resolve().ApplyCaoSongDamageSupport();
    internal bool ApplyCaoSongRecoverySupport() => _resolve().ApplyCaoSongRecoverySupport();
    internal SkillProgramStepOutcome BeginCaoSongLilu(ProgramSkillFrame supplied) => _resolve().BeginCaoSongLilu(supplied);
    internal bool CanContinuePaidCaoSongLilu(ProgramSkillFrame frame) => _resolve().CanContinuePaidCaoSongLilu(frame);
    internal bool CanOfferCaoSongLilu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferCaoSongLilu(candidate, trigger, context);
    internal bool CanPayCaoSongYizheng(CaoSongYizhengGrantedEvent grant, int beneficiary) => _resolve().CanPayCaoSongYizheng(grant, beneficiary);
    internal IReadOnlyList<PromptChoice> CaoSongLiluChoices(ProgramSkillFrame frame) => _resolve().CaoSongLiluChoices(frame);
    internal bool CaoSongLiluFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().CaoSongLiluFirstChild(root, child);
    internal ProgramSkillFrame? CaoSongLiluObserverRoot() => _resolve().CaoSongLiluObserverRoot();
    internal CardConversionSource CaoSongSource(ProgramSkillFrame frame) => _resolve().CaoSongSource(frame);
    internal PromptChoice ChooseAiCaoSongLilu(PendingDecision decision, ProgramSkillFrame frame) => _resolve().ChooseAiCaoSongLilu(decision, frame);
    internal void CompleteCaoSongLilu(ProgramSkillFrame frame, bool completed) => _resolve().CompleteCaoSongLilu(frame, completed);
    internal void ContinueCaoSongLilu(long id) => _resolve().ContinueCaoSongLilu(id);
    internal bool DrainCaoSongLiluChildren(long id) => _resolve().DrainCaoSongLiluChildren(id);
    internal void ExpireCaoSongYizhengAtNextOwnTurn(TurnStartedEvent started) => _resolve().ExpireCaoSongYizhengAtNextOwnTurn(started);
    internal bool HasDamageAmountSupport(int sourceSeat, int targetSeat) => _resolve().HasDamageAmountSupport(sourceSeat, targetSeat);
    internal bool HasRecoveryAmountSupport(int targetSeat) => _resolve().HasRecoveryAmountSupport(targetSeat);
    internal bool IsAmountSupportHealthChild(ResolutionFrame? parent, HpChangedTriggerWindowFrame hp) => _resolve().IsAmountSupportHealthChild(parent, hp);
    internal bool IsCaoSongLiluRecoveryParent(ProgramSkillFrame producer, RecoveryReplacementFrame recovery) => _resolve().IsCaoSongLiluRecoveryParent(producer, recovery);
    internal void IssueCaoSongYizheng(ProgramSkillFrame supplied) => _resolve().IssueCaoSongYizheng(supplied);
    internal void PayCaoSongYizhengMaximum(long parentId, CaoSongYizhengGrantedEvent grant, int beneficiary, bool recovery) => _resolve().PayCaoSongYizhengMaximum(parentId, grant, beneficiary, recovery);
    internal void PublishCaoSongLiluChoice(ProgramSkillFrame frame) => _resolve().PublishCaoSongLiluChoice(frame);
    internal bool ResumeCaoSongLilu(long id) => _resolve().ResumeCaoSongLilu(id);
    internal int SupportedCaoSongRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount) => _resolve().SupportedCaoSongRecoveryAmount(recovery, baseAmount);
    internal int SupportedRecoveryHpBefore(RecoveryReplacementFrame recovery, int fallback) => _resolve().SupportedRecoveryHpBefore(recovery, fallback);
    internal bool TryAdvanceCaoSongLiluSubtree() => _resolve().TryAdvanceCaoSongLiluSubtree();
    internal void ValidateCaoSongSupportPaid(long parentId, CaoSongAmountSupportReceipt receipt, bool recovery) => _resolve().ValidateCaoSongSupportPaid(parentId, receipt, recovery);
}
