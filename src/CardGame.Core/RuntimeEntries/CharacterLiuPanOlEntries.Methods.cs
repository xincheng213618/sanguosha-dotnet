#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuPanOlEntries
{
    internal bool AdvanceLiuPanSubtree() => _resolve().AdvanceLiuPanSubtree();
    internal void AnswerPijing(PromptChoice selected) => _resolve().AnswerPijing(selected);
    internal void ApplyPijingTargets(ProgramSkillFrame f) => _resolve().ApplyPijingTargets(f);
    internal SkillProgramStepOutcome BeginPijing(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginPijing(supplied, op);
    internal PromptChoice ChoosePijing(PendingDecision p, ProgramSkillFrame f) => _resolve().ChoosePijing(p, f);
    internal void CollectIssuedPijingNextUseCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedPijingNextUseCandidates(action, window, result);
    internal void CompletePijingGift(ProgramSkillFrame f) => _resolve().CompletePijingGift(f);
    internal bool ContinueIssuedPijing(ProgramSkillFrame f) => _resolve().ContinueIssuedPijing(f);
    internal bool ContinuePaidPijing(ProgramSkillFrame f) => _resolve().ContinuePaidPijing(f);
    internal void ContinuePijing(long id) => _resolve().ContinuePijing(id);
    internal ProgramSkillWindowContext CreatePijingIssuedContext(ProgramCardTriggerWindowFrame frame, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CreatePijingIssuedContext(frame, candidate, context);
    internal bool DrainPijing(ProgramSkillFrame f) => _resolve().DrainPijing(f);
    internal void DrawPijingReciprocal(ProgramSkillFrame f) => _resolve().DrawPijingReciprocal(f);
    internal void FinishPijing(ProgramSkillFrame f) => _resolve().FinishPijing(f);
    internal bool HasPijingTargetTail(CardUseFrame use) => _resolve().HasPijingTargetTail(use);
    internal bool HasPijingTargetTail2(CardUseFrame use, CardActionContext action) => _resolve().HasPijingTargetTail(use, action);
    internal bool HasPijingZeroTargetTail(CardUseFrame use) => _resolve().HasPijingZeroTargetTail(use);
    internal bool IsIssuedPijingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedPijingCandidate(c, context);
    internal bool IsLiuPanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiuPanRecoveryParent(f, recovery);
    internal bool IsPijingAppliedTargetFact(CardUseFrame use, PijingTargetsAppliedEvent fact, IReadOnlyList<int> precedingTargets, scoped out IReadOnlyList<int> resultTargets) => _resolve().IsPijingAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
    internal void IssuePijingGift(ProgramSkillFrame f) => _resolve().IssuePijingGift(f);
    internal bool LiuPanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiuPanFirstChild(f, child);
    internal bool LiuPanNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().LiuPanNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? LiuPanObserverRoot() => _resolve().LiuPanObserverRoot();
    internal IReadOnlyList<PromptChoice> PijingChoices(ProgramSkillFrame f) => _resolve().PijingChoices(f);
    internal IReadOnlyList<Card> PijingGiftCards(int donor, int recipient) => _resolve().PijingGiftCards(donor, recipient);
    internal bool PijingInitialEligible(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().PijingInitialEligible(c, context);
    internal IReadOnlyList<PijingToggle> PijingInitialOptions(CardUseFrame use, IReadOnlyList<PijingToggle> selected) => _resolve().PijingInitialOptions(use, selected);
    internal PijingNextUseGroup? PijingIssuedGroup(ProgramTriggerCandidate c, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().PijingIssuedGroup(c, context, out use, out window);
    internal IReadOnlyList<IReadOnlyList<int>> PijingReciprocalOptions(CardUseFrame use, int issuer) => _resolve().PijingReciprocalOptions(use, issuer);
    internal void PublishPijing(ProgramSkillFrame f) => _resolve().PublishPijing(f);
    internal bool QualifyPijing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyPijing(c, t, context);
    internal CardUseFrame RebasePijingSlashTargets(CardUseFrame use, IReadOnlyList<int> targets) => _resolve().RebasePijingSlashTargets(use, targets);
    internal void RecordPijingRedirect(ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => _resolve().RecordPijingRedirect(f, useId, previousSeat, nextSeat);
    internal bool RejectPijingDesignatedTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, SkillProgramTrigger trigger) => _resolve().RejectPijingDesignatedTarget(candidate, context, trigger);
    internal CardUseFrame RequirePijingUse(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequirePijingUse(f, out window);
    internal bool ResumePijing(long id) => _resolve().ResumePijing(id);
    internal bool ReturnPijingMovement(ProgramSkillFrame f) => _resolve().ReturnPijingMovement(f);
    internal bool TryCompletePijingZeroTargetUse(ProgramCardTriggerWindowFrame window) => _resolve().TryCompletePijingZeroTargetUse(window);
    internal bool TryGetPijingUse(ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetPijingUse(context, out use, out window);
    internal bool HasLiuPanCapability { get => _resolve().HasLiuPanCapability; }
}
