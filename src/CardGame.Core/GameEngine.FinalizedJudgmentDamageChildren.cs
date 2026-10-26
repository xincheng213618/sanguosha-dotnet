namespace CardGame.Core;

public sealed partial class GameEngine
{
    // A finalized judgment still owns its material while its Leiji-like damage
    // resolves. A damage response may judge independently on that exact child.
    private JudgmentFrame? FinalizedJudgmentDamageParent(long requestingProgramId)
    {
        var index = _resolutionStack.FindIndex(frame => frame.Id == requestingProgramId);
        if (index < 5 || _resolutionStack[index] is not ProgramSkillFrame request ||
            _resolutionStack[index - 1] is not DamageTriggerWindowFrame damageWindow ||
            _resolutionStack[index - 2] is not DamageFrame damage ||
            _resolutionStack[index - 3] is not ProgramSkillFrame producer ||
            _resolutionStack[index - 4] is not ProgramJudgmentTriggerWindowFrame finalized ||
            _resolutionStack[index - 5] is not JudgmentFrame { Succeeded: not null, Step: ResolutionFrameStep.Completed } original ||
            request.ParentFrameId != damageWindow.Id || !DamageProgramMatchesWindow(request, damageWindow) ||
            request.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } response ||
            response.ParentFrameId != damageWindow.Id || response.DamageFrameId != damage.Id ||
            damageWindow.ParentFrameId != damage.Id || damage.ParentFrameId != producer.Id ||
            response.SourceSeat != damage.SourceSeat || response.TargetSeat != damage.TargetSeat || response.Amount != damage.Amount ||
            producer.ParentFrameId != finalized.Id || producer.WindowContext is not
                { Window: SkillProgramTriggerWindow.JudgmentFinalized, Judgment: { } context } origin ||
            origin.ParentFrameId != finalized.Id || finalized.ParentFrameId != original.Id ||
            finalized.ZhenyiBeforeResult == true || !finalized.Activated || finalized.Judgment != context ||
            finalized.CandidateIndex < 0 || finalized.CandidateIndex >= finalized.Candidates.Count ||
            finalized.Candidates[finalized.CandidateIndex] is not { } candidate ||
            candidate.OwnerSeat != producer.OwnerSeat || candidate.SkillId != producer.SkillId ||
            candidate.TriggerId != producer.TriggerId || candidate.SkillInstanceId != producer.SkillInstanceId || candidate.GameplayHash != producer.GameplayHash ||
            producer.AttackAttempt is not { DamageWasApplied: true } attack || attack.ProgramJudgmentWindowId != finalized.Id ||
            attack.SourceSeat != damage.SourceSeat || attack.TargetSeat != damage.TargetSeat || attack.DamageAmount != damage.Amount ||
            attack.Nature != damage.Nature || context.JudgmentFrameId != original.Id || context.CardId != original.CardId ||
            context.SubjectSeat != original.TargetSeat || context.SourceSeat != original.SourceSeat || context.Reason != original.Reason ||
            context.Succeeded != original.Succeeded || GetJudgmentCard(original)?.Id != context.CardId ||
            original.ReplacementCandidateIndex < 0 || original.ReplacementCandidateIndex > original.ReplacementCandidates.Count ||
            original.OutcomeClaim is not null && !HasExactJudgmentOutcomeClaimDisposition(original) ||
            _cardZones.GetLocation(context.CardId) != CardLocation.Judgment(original.TargetSeat) &&
                !HasExactJudgmentOutcomeClaimDisposition(original) && !HasShenZfJudgmentGainDisposition(original) &&
                !(_cardZones.GetLocation(context.CardId) == CardLocation.Hand(original.TargetSeat) &&
                    finalized.Candidates.Take(finalized.CandidateIndex + 1).Any(prior =>
                        _contentRegistry.Skills[prior.SkillId].Program!.Triggers.Single(trigger => trigger.Id == prior.TriggerId)
                            .Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimJudgmentCard))))
            return null;
        return ProgramEventHistory<ProgramJudgmentDamageRequestedEvent>().Count(issued => issued.FrameId == finalized.Id &&
            issued.JudgmentFrameId == original.Id && issued.SkillId == producer.SkillId && issued.TriggerId == producer.TriggerId &&
            issued.SourceSeat == attack.SourceSeat && issued.TargetSeat == attack.TargetSeat && issued.Amount == attack.DamageAmount &&
            issued.Nature == attack.Nature) == 1 ? original : null;
    }

    private bool CanBeginFinalizedDamageChildJudgment(JudgmentFrame original, long parentFrameId,
        int targetSeat, string reason, JudgmentContinuationKind continuation, string? resultBind,
        SkillProgramCardSetVisibility? visibility) =>
        continuation == JudgmentContinuationKind.ProgramSkill && resultBind is not null && visibility is { } publicResult &&
        _resolutionStack.LastOrDefault() is ProgramSkillFrame request && request.Id == parentFrameId &&
        MatchesProgramJudgmentInstruction(request, targetSeat, reason, resultBind, publicResult) &&
        FinalizedJudgmentDamageParent(parentFrameId)?.Id == original.Id;

    private void AssertFinalizedJudgmentDamageChildren()
    {
        var judgments = _resolutionStack.OfType<JudgmentFrame>().Where(frame => frame.CardAttack is null).ToArray();
        for (var index = 1; index < judgments.Length; index++)
        {
            var nested = judgments[index];
            if (nested.Continuation != JudgmentContinuationKind.ProgramSkill ||
                FinalizedJudgmentDamageParent(nested.ParentFrameId)?.Id != judgments[index - 1].Id ||
                _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == nested.ParentFrameId) is not { } request ||
                !IsValidProgramJudgmentContinuation(nested, request))
                throw new InvalidOperationException("A nested judgment lost its exact finalized-judgment damage response parent.");
        }
    }
}
