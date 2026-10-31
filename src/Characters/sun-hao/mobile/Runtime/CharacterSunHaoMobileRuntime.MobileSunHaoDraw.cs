#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoMobileRuntime
{
    internal const string MobileSunHaoDrawReason = "skill-program.mobile:sun-hao-canshi.draw";
    internal bool HasQualifiedMobileGuiming(CharacterState owner)
    {
        if (!owner.IsAlive || owner.IdentityConcealed || !_characterSunHaoOlOperations.SunHaoActualTurnOpen || owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return false;
        var grants = owner.SkillGrants.Grants.Where(g => _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.CardPolicies.Any(p => p.Kind == SkillProgramCardPolicyKind.OtherWuTreatedWoundedForCanshi) == true).ToArray();
        if (grants.Length == 0)
            return false;
        var suppressors = _corePrivateGeneralLibrariesOperations.PrivateGeneralLibrarySuppressionInputs(owner);
        return grants.Any(g => _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, g) && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, g) && _coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, g) && _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, g) && _coreHpConditionalSkillGrantsOperations.IsHpConditionalGrantQualified(owner, g) && _characterHeQiOlOperations.IsEquipmentSuitGrantQualified(owner, g) && (suppressors.Count == 0 || suppressors.Contains(g.SkillId) || g.SourceId.StartsWith("equipment:", StringComparison.Ordinal)) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, g.SkillId, g.SkillInstanceId, Role.Lord));
    }

    internal SkillProgramStepOutcome BeginMobileSunHaoDraw(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is not null || frame.NativeDraw is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Mobile Canshi requires one clean original draw instruction.");
        var guiming = HasQualifiedMobileGuiming(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var count = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && (_coreTreatedWoundedOperations.IsRuleWounded(p) || guiming && p.Seat != frame.OwnerSeat && p.HasHpValue && !p.IdentityConcealed && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == "wu"));
        var receipt = new MobileSunHaoDrawReceipt(global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime.SunHaoSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, context.ParentFrameId, count);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileSunHaoDrawIssuedEvent(frame.Id, receipt.Source, receipt.Hash, receipt.ActualTurn, receipt.TurnOwner, receipt.WindowId, receipt.X));
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, count, new(MobileSunHaoDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation)]
    internal void ReturnMobileSunHaoDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.MobileSunHaoDraw is not { Outcome: null } receipt || frame.NativeDraw is not null || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != receipt.X || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Mobile Canshi lost its once-issued exact native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = receipt with { Outcome = outcome }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileSunHaoDrawReturnedEvent(frame.Id, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        ContinueMobileSunHaoDraw(frame.Id);
    }

    internal void ContinueMobileSunHaoDraw(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        AssertMobileSunHaoDraw(frame);
        if (frame.MobileSunHaoDraw is not { Outcome: not null })
            return;
        if (_characterSunHaoOlOperations.DrainSunHao(frame))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var receipt = frame.MobileSunHaoDraw!;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<SunHaoCanshiGrantedEvent>().Any(e => e.FrameId == id))
            throw new InvalidOperationException("A mobile Canshi replacement cannot issue the same obligation twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SunHaoCanshiGrantedEvent(frame.Id, receipt.Source, receipt.Hash, receipt.ActualTurn, receipt.TurnOwner, receipt.WindowId, receipt.X));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileSunHaoDraw = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 80400)]
    internal bool ResumeMobileSunHaoDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileSunHaoDraw: not null } frame || frame.Id != id)
            return false;
        ContinueMobileSunHaoDraw(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 80400)]
    internal bool ReturnMobileSunHaoDrawMovement(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is null)
            return false;
        ContinueMobileSunHaoDraw(frame.Id);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 80400)]
    internal void AssertMobileSunHaoDraw(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is not { } receipt)
            return;
        if (frame.SkillId != SunHaoComposition.MobileCanshi || frame.TriggerId != "normal-draw-replace-wounded" || frame.ActivationId != frame.TriggerId || frame.InstructionIndex != 1 || frame.SunHaoCanshi is not null || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || receipt.Source != global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime.SunHaoSource(frame) || receipt.Hash != frame.GameplayHash || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || receipt.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || receipt.X < 0 || receipt.X > _coreMatchParticipantsPlayersAccess._players.Count || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || context.ParentFrameId != receipt.WindowId || context.OwnerSeat != frame.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == receipt.WindowId)is not { NormalDrawReplaced: false, Continuation: ProgramLifecycleContinuation.CompleteDrawPhase } || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileSunHaoDrawIssuedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var issued] || issued != new MobileSunHaoDrawIssuedEvent(frame.Id, receipt.Source, receipt.Hash, receipt.ActualTurn, receipt.TurnOwner, receipt.WindowId, receipt.X))
            throw new InvalidOperationException("Mobile Canshi lost its original qualified source, frozen population, draw phase or issuing frame.");
        if (receipt.Outcome is null)
        {
            if (frame.PendingMovementContinuation is not null || frame.NativeDraw is not { ReturnKind: NativeDrawAttemptReturnKind.ProgramOperation, Operation: SkillProgramEffectOp.GrantSunHaoCanshiDrawObligation } draw || draw.InstructionIndex != frame.InstructionIndex || draw.RequestedCount != receipt.X || draw.Reason.Value != MobileSunHaoDrawReason || !draw.Recipients.SequenceEqual([frame.OwnerSeat]) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileSunHaoDrawReturnedEvent>().Any(e => e.FrameId == frame.Id))
                throw new InvalidOperationException("Mobile Canshi lost its unpaid native draw child.");
            return;
        }

        var result = receipt.Outcome;
        if (frame.NativeDraw is not null || frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != frame.OwnerSeat || result.RecipientSeat != frame.OwnerSeat || result.RequestedCount != receipt.X || result.SequenceBefore < 0 || result.SequenceAfter < result.SequenceBefore || result.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || result.ActualDrawIds.Count > receipt.X || result.ActualDrawIds.Distinct().Count() != result.ActualDrawIds.Count || result.Replacement is not null && result.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > result.SequenceBefore && m.Sequence <= result.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == MobileSunHaoDrawReason).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(result.ActualDrawIds) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileSunHaoDrawReturnedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var returned] || returned != new MobileSunHaoDrawReturnedEvent(frame.Id, result.AttemptFrameId, result.SequenceBefore, result.SequenceAfter, result.ActualDrawIds.Count, result.ReplacementGainedIds.Count) || result.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frame.Id && e.RecipientSeat == frame.OwnerSeat && e.RequestedCount == receipt.X && e.Replaced == (result.Replacement is not null)) != 1)
            throw new InvalidOperationException("Mobile Canshi lost its once-returned physical draw outcome and child cursor.");
        if (result.AttemptFrameId is { } request)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptStartedEvent>().Where(e => e.Context.AttemptFrameId == request).ToArray()is not [var start] || start.Context.ParentFrameId != frame.Id || start.Context.RecipientSeat != frame.OwnerSeat || start.Context.RequestedCount != receipt.X || start.Context.Reason.Value != MobileSunHaoDrawReason || start.Context.ActualTurnNumber != receipt.ActualTurn || start.Context.ActualTurnOwnerSeat != receipt.TurnOwner || start.Context.SequenceBefore > result.SequenceBefore || start.Return != new NativeDrawAttemptReturn(NativeDrawAttemptReturnKind.ProgramOperation, frame.InstructionIndex, 0))
                throw new InvalidOperationException("Mobile Canshi retained an outcome from a foreign native draw request.");
        }
        else if (result.Replacement is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptStartedEvent>().Any(e => e.Context.ParentFrameId == frame.Id))
            throw new InvalidOperationException("Mobile Canshi cannot invent or omit an accepted native draw request.");
    }

    internal bool IsMobileSunHaoDrawMovement(ProgramSkillFrame frame, CardMovementRecord movement)
    {
        if (frame.MobileSunHaoDraw?.Outcome is not { } result || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement) || movement.To != CardLocation.Hand(frame.OwnerSeat))
            return false;
        return result.ActualDrawIds.Contains(movement.CardId) && movement.Sequence > result.SequenceBefore && movement.Sequence <= result.SequenceAfter && movement.From == CardLocation.DrawPile && movement.Reason.Value == MobileSunHaoDrawReason || result.Replacement is { } replacement && replacement.GainedIds.Contains(movement.CardId) && movement.Sequence > replacement.SequenceBefore && movement.Sequence <= replacement.SequenceAfter && movement.Reason == replacement.Reason;
    }

    internal bool MobileSunHaoDrawFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.MobileSunHaoDraw is not { Outcome: not null })
            return false;
        AssertMobileSunHaoDraw(frame);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsMobileSunHaoDrawMovement(frame, m));
    }
}
