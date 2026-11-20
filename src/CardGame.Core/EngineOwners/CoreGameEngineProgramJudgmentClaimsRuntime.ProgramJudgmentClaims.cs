#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramJudgmentClaimsRuntime
{
    internal SkillProgramStepOutcome ClaimProgramJudgmentCard(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var context = active.WindowContext;
        var judgment = context?.Judgment ?? throw new InvalidOperationException("A judgment claim requires the finalized judgment context.");
        var pending = _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment ?? throw new InvalidOperationException("A judgment claim lost its parent judgment.");
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault();
        if (context!.Window != SkillProgramTriggerWindow.JudgmentFinalized ||
            window is not { Activated: true } || window.ZhenyiBeforeResult == true ||
            window.Id != context.ParentFrameId || window.ParentFrameId != pending.Id || window.Judgment != judgment ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            window.Candidates[window.CandidateIndex] != new ProgramJudgmentTriggerCandidate(active.OwnerSeat, active.SkillId,
                active.TriggerId!, active.SkillInstanceId, active.GameplayHash) ||
            pending.Id != judgment.JudgmentFrameId || pending.TargetSeat != active.OwnerSeat || judgment.SubjectSeat != active.OwnerSeat ||
            _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending) is not { } card || card.Id != judgment.CardId ||
            active.PendingMovementContinuation is not null || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive)
            throw new InvalidOperationException("The judgment claim is no longer legal.");
        var from = CardLocation.Judgment(active.OwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(active.OwnerSeat, 0, null) });
        var sequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(active.OwnerSeat), new CardMoveReason($"skill-program.{active.SkillId}.claimJudgmentCard"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentCardClaimedEvent(pending.Id, active.SkillId, active.OwnerSeat, card.Id, card.Kind));
        _coreJudgmentOutcomeClaimsOperations.Value.CaptureProgramJudgmentOutcomeClaim(active, pending, card, sequenceBefore);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
