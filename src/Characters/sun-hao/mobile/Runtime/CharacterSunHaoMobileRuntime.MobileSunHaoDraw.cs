#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunHaoMobileRuntime
{
    internal const string MobileSunHaoDrawReason = "skill-program.mobile:sun-hao-canshi.draw";
    internal bool HasQualifiedMobileGuiming(CharacterState owner)
    {
        if (!owner.IsAlive || owner.IdentityConcealed || !_characterSunHaoOlOperations.Value.SunHaoActualTurnOpen || owner.Seat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return false;
        var grants = owner.SkillGrants.Grants.Where(g => _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Program?.CardPolicies.Any(p => p.Kind == SkillProgramCardPolicyKind.OtherWuTreatedWoundedForCanshi) == true).ToArray();
        if (grants.Length == 0)
            return false;
        var suppressors = _corePrivateGeneralLibrariesOperations.PrivateGeneralLibrarySuppressionInputs(owner);
        return grants.Any(g => _corePrivateGeneralLibrariesOperations.LibraryBaseGrantLive(owner, g) && _coreCurrentTurnSkillSuppressionsOperations.IsCurrentTurnSkillGrantQualified(owner, g) && _coreLordSkillProjectionsOperations.IsProjectedGrantQualified(owner, g) && _corePrivateGeneralLibrariesOperations.IsGeneralLibraryGrantQualified(owner, g) && _coreHpConditionalSkillGrantsOperations.Value.IsHpConditionalGrantQualified(owner, g) && _characterHeQiOlOperations.IsEquipmentSuitGrantQualified(owner, g) && (suppressors.Count == 0 || suppressors.Contains(g.SkillId) || g.SourceId.StartsWith("equipment:", StringComparison.Ordinal)) && _coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, g.SkillId, g.SkillInstanceId, Role.Lord));
    }

    internal SkillProgramStepOutcome BeginMobileSunHaoDraw(ProgramSkillFrame frame)
    {
        if (frame.MobileSunHaoDraw is not null || frame.NativeDraw is not null || frame.WindowContext is not { } context)
            throw new InvalidOperationException("Mobile Canshi requires one clean original draw instruction.");
        var guiming = HasQualifiedMobileGuiming(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var count = _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && (_coreTreatedWoundedOperations.IsRuleWounded(p) || guiming && p.Seat != frame.OwnerSeat && p.HasHpValue && !p.IdentityConcealed && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(p) == "wu"));
        var receipt = new MobileSunHaoDrawReceipt(CharacterSunHaoOlRuntime.SunHaoSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, context.ParentFrameId, count);
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
        if (frame.MobileSunHaoDraw is not { Outcome: not null })
            return;
        if (_characterSunHaoOlOperations.Value.DrainSunHao(frame))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var receipt = frame.MobileSunHaoDraw!;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<SunHaoCanshiGrantedEvent>().Any(e => e.FrameId == id))
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
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsMobileSunHaoDrawMovement(frame, m));
    }
}
