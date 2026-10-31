#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuPanOlEntries
{
    internal bool AdvanceLiuPanSubtree() => _resolve().AdvanceLiuPanSubtree();
    internal void AnswerPijing(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerPijing(selected);
    internal void ApplyPijingTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyPijingTargets(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPijing(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginPijing(supplied, op);
    internal global::CardGame.Core.PromptChoice ChoosePijing(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChoosePijing(p, f);
    internal void CollectIssuedPijingNextUseCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedPijingNextUseCandidates(action, window, result);
    internal void CompletePijingGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompletePijingGift(f);
    internal bool ContinueIssuedPijing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedPijing(f);
    internal bool ContinuePaidPijing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidPijing(f);
    internal void ContinuePijing(long id) => _resolve().ContinuePijing(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreatePijingIssuedContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreatePijingIssuedContext(frame, candidate, context);
    internal bool DrainPijing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainPijing(f);
    internal void DrawPijingReciprocal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrawPijingReciprocal(f);
    internal void FinishPijing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishPijing(f);
    internal bool HasLiuPanBeforeDamageObserver(long id) => _resolve().HasLiuPanBeforeDamageObserver(id);
    internal bool HasLiuPanDamageObserver(long id) => _resolve().HasLiuPanDamageObserver(id);
    internal bool HasPijingTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasPijingTargetTail(use);
    internal bool HasPijingTargetTail2(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action) => _resolve().HasPijingTargetTail(use, action);
    internal bool HasPijingZeroTargetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasPijingZeroTargetTail(use);
    internal bool IsIssuedPijingCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedPijingCandidate(c, context);
    internal bool IsLiuPanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsLiuPanMovement(f, effect, pending);
    internal bool IsLiuPanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLiuPanRecoveryParent(f, recovery);
    internal bool IsPijingAppliedTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PijingTargetsAppliedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> precedingTargets, scoped out global::System.Collections.Generic.IReadOnlyList<int> resultTargets) => _resolve().IsPijingAppliedTargetFact(use, fact, precedingTargets, out resultTargets);
    internal void IssuePijingGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssuePijingGift(f);
    internal bool LiuPanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiuPanFirstChild(f, child);
    internal bool LiuPanNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().LiuPanNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? LiuPanObserverRoot() => _resolve().LiuPanObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PijingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PijingChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PijingGiftCards(int donor, int recipient) => _resolve().PijingGiftCards(donor, recipient);
    internal bool PijingInitialEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().PijingInitialEligible(c, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PijingToggle> PijingInitialOptions(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PijingToggle> selected) => _resolve().PijingInitialOptions(use, selected);
    internal global::CardGame.Core.PijingNextUseGroup? PijingIssuedGroup(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().PijingIssuedGroup(c, context, out use, out window);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> PijingReciprocalOptions(global::CardGame.Core.CardUseFrame use, int issuer) => _resolve().PijingReciprocalOptions(use, issuer);
    internal void PublishPijing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPijing(f);
    internal bool QualifyPijing(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyPijing(c, t, context);
    internal global::CardGame.Core.CardUseFrame RebasePijingSlashTargets(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().RebasePijingSlashTargets(use, targets);
    internal void RecordPijingRedirect(global::CardGame.Core.ProgramSkillFrame f, long useId, int previousSeat, int nextSeat) => _resolve().RecordPijingRedirect(f, useId, previousSeat, nextSeat);
    internal bool RejectPijingDesignatedTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().RejectPijingDesignatedTarget(candidate, context, trigger);
    internal global::CardGame.Core.CardUseFrame RequirePijingUse(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequirePijingUse(f, out window);
    internal bool ResumePijing(long id) => _resolve().ResumePijing(id);
    internal bool ReturnPijingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPijingMovement(f);
    internal bool TryCompletePijingZeroTargetUse(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryCompletePijingZeroTargetUse(window);
    internal bool TryGetPijingUse(global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetPijingUse(context, out use, out window);
    internal bool HasLiuPanCapability { get => _resolve().HasLiuPanCapability; }
}
