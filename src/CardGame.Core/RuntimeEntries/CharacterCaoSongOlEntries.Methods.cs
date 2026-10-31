#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoSongOlEntries
{
    internal global::CardGame.Core.CaoSongYizhengGrantedEvent[] ActiveCaoSongYizhengGrants(int beneficiary) => _resolve().ActiveCaoSongYizhengGrants(beneficiary);
    internal void AnswerCaoSongLilu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoSongLilu(choice);
    internal bool ApplyCaoSongDamageSupport() => _resolve().ApplyCaoSongDamageSupport();
    internal bool ApplyCaoSongRecoverySupport() => _resolve().ApplyCaoSongRecoverySupport();
    internal void AssertCaoSongLilu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertCaoSongLilu(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoSongLilu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginCaoSongLilu(supplied);
    internal bool CanContinuePaidCaoSongLilu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidCaoSongLilu(frame);
    internal bool CanOfferCaoSongLilu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCaoSongLilu(candidate, trigger, context);
    internal bool CanPayCaoSongYizheng(global::CardGame.Core.CaoSongYizhengGrantedEvent grant, int beneficiary) => _resolve().CanPayCaoSongYizheng(grant, beneficiary);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoSongLiluChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaoSongLiluChoices(frame);
    internal bool CaoSongLiluFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoSongLiluFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? CaoSongLiluObserverRoot() => _resolve().CaoSongLiluObserverRoot();
    internal global::CardGame.Core.CardConversionSource CaoSongSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CaoSongSource(frame);
    internal global::CardGame.Core.PromptChoice ChooseAiCaoSongLilu(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseAiCaoSongLilu(decision, frame);
    internal void CompleteCaoSongLilu(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().CompleteCaoSongLilu(frame, completed);
    internal void ContinueCaoSongLilu(long id) => _resolve().ContinueCaoSongLilu(id);
    internal bool DrainCaoSongLiluChildren(long id) => _resolve().DrainCaoSongLiluChildren(id);
    internal void ExpireCaoSongYizhengAtNextOwnTurn(global::CardGame.Core.TurnStartedEvent started) => _resolve().ExpireCaoSongYizhengAtNextOwnTurn(started);
    internal bool HasCaoSongLiluDamageObserver(long id) => _resolve().HasCaoSongLiluDamageObserver(id);
    internal bool HasDamageAmountSupport(int sourceSeat, int targetSeat) => _resolve().HasDamageAmountSupport(sourceSeat, targetSeat);
    internal bool HasRecoveryAmountSupport(int targetSeat) => _resolve().HasRecoveryAmountSupport(targetSeat);
    internal bool IsAmountSupportHealthChild(global::CardGame.Core.ResolutionFrame? parent, global::CardGame.Core.HpChangedTriggerWindowFrame hp) => _resolve().IsAmountSupportHealthChild(parent, hp);
    internal bool IsCaoSongLiluDying() => _resolve().IsCaoSongLiluDying();
    internal bool IsCaoSongLiluRecoveryParent(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsCaoSongLiluRecoveryParent(producer, recovery);
    internal void IssueCaoSongYizheng(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueCaoSongYizheng(supplied);
    internal void PayCaoSongYizhengMaximum(long parentId, global::CardGame.Core.CaoSongYizhengGrantedEvent grant, int beneficiary, bool recovery) => _resolve().PayCaoSongYizhengMaximum(parentId, grant, beneficiary, recovery);
    internal void PublishCaoSongLiluChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishCaoSongLiluChoice(frame);
    internal bool ResumeCaoSongLilu(long id) => _resolve().ResumeCaoSongLilu(id);
    internal int SupportedCaoSongRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount) => _resolve().SupportedCaoSongRecoveryAmount(recovery, baseAmount);
    internal int SupportedRecoveryHpBefore(global::CardGame.Core.RecoveryReplacementFrame recovery, int fallback) => _resolve().SupportedRecoveryHpBefore(recovery, fallback);
    internal bool TryAdvanceCaoSongLiluSubtree() => _resolve().TryAdvanceCaoSongLiluSubtree();
    internal void ValidateCaoSongSupportPaid(long parentId, global::CardGame.Core.CaoSongAmountSupportReceipt receipt, bool recovery) => _resolve().ValidateCaoSongSupportPaid(parentId, receipt, recovery);
}
