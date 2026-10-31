#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJianBoundaryMobileRuntime
{
    internal const string MobileSunJianPolu = "boundary:polu-mobile";
    SkillProgramStepOutcome IMobileSunJianPoluHost.Draw(ProgramSkillFrame frame) => BeginMobileSunJianPolu(frame);
    internal int MobileSunJianInvocationCount(ProgramSkillFrame frame) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.SkillId == MobileSunJianPolu && e.OwnerSeat == frame.OwnerSeat && e.FrameId <= frame.Id);
    internal SkillProgramStepOutcome BeginMobileSunJianPolu(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != MobileSunJianPolu || frame.InstructionIndex != 2 || frame.MobileSunJianPolu is not null || frame.WindowContext?.Window is not (SkillProgramTriggerWindow.CharacterDied or SkillProgramTriggerWindow.OwnerDied))
            throw new InvalidOperationException("Polu requires its original accepted death trigger and frozen target set.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileSunJianPolu = new(new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, MobileSunJianInvocationCount(frame)) });
        ContinueMobileSunJianPolu(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidMobileSunJianPolu(ProgramSkillFrame frame) => frame.MobileSunJianPolu is { } r && frame.SkillId == MobileSunJianPolu && frame.InstructionIndex == 2 && r.Source == new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId) && r.Hash == frame.GameplayHash && r.Hash == _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash && r.Count > 0 && r.Count == MobileSunJianInvocationCount(frame) && r.RecipientIndex >= 0 && r.RecipientIndex <= frame.SelectedTargetSeats.Count && frame.SelectedTargetSeats.Distinct().Count() == frame.SelectedTargetSeats.Count;
    internal void ContinueMobileSunJianPolu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileSunJianPolu: { } receipt } frame || frame.Id != id)
            return;
        if (!ValidMobileSunJianPolu(frame))
            throw new InvalidOperationException("Polu lost its original invocation count or recipient cursor.");
        if (receipt.AwaitingMovement)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { MobileSunJianPolu = receipt = receipt with { RecipientIndex = receipt.RecipientIndex + 1, AwaitingMovement = false, Draw = null }, PendingMovementContinuation = null });
        }

        if (receipt.RecipientIndex == frame.SelectedTargetSeats.Count)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { MobileSunJianPolu = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return;
        }

        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.SelectedTargetSeats[receipt.RecipientIndex], receipt.Count, CharacterSunJianBoundaryMobileDefinitions.MobileSunJianPoluDraw);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileSunJianPolu)]
    internal void ReturnMobileSunJianPoluDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (!ValidMobileSunJianPolu(frame) || frame.MobileSunJianPolu is not { AwaitingMovement: false } r || r.RecipientIndex >= frame.SelectedTargetSeats.Count || outcome.RecipientSeat != frame.SelectedTargetSeats[r.RecipientIndex] || outcome.RequestedCount != r.Count || outcome.ActualDrawIds.Count > r.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == CharacterSunJianBoundaryMobileDefinitions.MobileSunJianPoluDraw).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Polu cannot accept a foreign or repeated native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { MobileSunJianPolu = r with { AwaitingMovement = true, Draw = outcome }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        ContinueMobileSunJianPolu(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 65000)]
    internal bool ResumeMobileSunJianPolu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileSunJianPolu.AwaitingMovement: true } frame || frame.Id != id)
            return false;
        ContinueMobileSunJianPolu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 65000)]
    internal bool ReturnMobileSunJianPoluMovement(ProgramSkillFrame frame)
    {
        if (frame.MobileSunJianPolu is not { AwaitingMovement: true })
            return false;
        ContinueMobileSunJianPolu(frame.Id);
        return true;
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 65000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 65000)]
    internal bool ProveMobileSunJianPoluRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => frame.MobileSunJianPolu is { AwaitingMovement: true, Draw.Replacement: not null } && ValidMobileSunJianPolu(frame) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 65000)]
    internal bool KeepMobileSunJianPolu(ProgramSkillFrame frame) => frame.MobileSunJianPolu is not null && ValidMobileSunJianPolu(frame);
}
