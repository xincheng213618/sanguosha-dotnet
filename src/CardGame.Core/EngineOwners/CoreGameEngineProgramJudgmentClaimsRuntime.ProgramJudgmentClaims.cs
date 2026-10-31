#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramJudgmentClaimsRuntime
{
    internal SkillProgramStepOutcome ClaimProgramJudgmentCard(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext;
        var judgment = context?.Judgment ?? throw new InvalidOperationException("A judgment claim requires the finalized judgment context.");
        var pending = _coreJudgmentCursorRuntimeOperations.ActiveJudgment ?? throw new InvalidOperationException("A judgment claim lost its parent judgment.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (context!.Window != SkillProgramTriggerWindow.JudgmentFinalized || context.ParentFrameId != _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault()?.Id || pending.Id != judgment.JudgmentFrameId || pending.TargetSeat != frame.OwnerSeat || judgment.SubjectSeat != frame.OwnerSeat || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)is not { } card || card.Id != judgment.CardId || active.PendingMovementContinuation is not null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("The judgment claim is no longer legal.");
        var from = CardLocation.Judgment(frame.OwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            throw new InvalidOperationException("The final judgment card has already left its judgment zone.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(frame.OwnerSeat, 0, null) });
        var sequenceBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(frame.OwnerSeat), new CardMoveReason($"skill-program.{(frame.SkillId)}.claimJudgmentCard"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJudgmentCardClaimedEvent(pending.Id, frame.SkillId, frame.OwnerSeat, card.Id, card.Kind));
        _coreJudgmentOutcomeClaimsOperations.CaptureProgramJudgmentOutcomeClaim(active, pending, card, sequenceBefore);
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
