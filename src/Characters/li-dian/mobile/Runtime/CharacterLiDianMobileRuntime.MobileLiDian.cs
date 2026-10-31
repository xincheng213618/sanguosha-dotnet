#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiDianMobileRuntime
{
    internal const string MobileLiDianWangxiSkill = "mobile:wangxi";
    SkillProgramStepOutcome IMobileLiDianWangxiHost.Draw(ProgramSkillFrame f) => BeginMobileLiDianWangxi(f);
    internal bool ExactMobileLiDianDamage(ProgramSkillFrame f, out ProgramSkillWindowContext c)
    {
        c = f.WindowContext!;
        var parentId = c?.ParentFrameId;
        return f.SkillId == MobileLiDianWangxiSkill && f.InstructionIndex == 1 && c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, SourceSeat: { } source, TargetSeat: { } target, DamageFrameId: { } damageId } && source != target && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && c.OwnerSeat == f.OwnerSeat && (f.TriggerId == "taking-each-point" ? target == f.OwnerSeat : f.TriggerId == "dealing-each-point" && source == f.OwnerSeat) && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == parentId)is { } window && window.TriggerWindow == c.Window && window.ParentFrameId == damageId && window.SourceSeat == source && window.TargetSeat == target && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().Any(d => d.Id == damageId && d.SourceSeat == source && d.TargetSeat == target);
    }

    internal SkillProgramStepOutcome BeginMobileLiDianWangxi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileLiDianWangxi is not null || !ExactMobileLiDianDamage(f, out var c))
            throw new InvalidOperationException("Mobile Wangxi requires its original per-point participant damage candidate.");
        var source = c.SourceSeat!.Value;
        var target = c.TargetSeat!.Value;
        if (!_coreMatchParticipantsPlayersAccess._players[source].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var recipients = new[]
        {
            source,
            target
        }.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, seat)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileLiDianWangxi = new(new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, c.ParentFrameId, c.DamageFrameId!.Value, source, target, c.Amount, c.OccurrenceIndex, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, recipients) });
        ContinueMobileLiDianWangxi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidMobileLiDianWangxi(ProgramSkillFrame f) => f.MobileLiDianWangxi is { } r && ExactMobileLiDianDamage(f, out var c) && r.Source == new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) && r.Hash == f.GameplayHash && r.Hash == _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash && r.WindowId == c.ParentFrameId && r.DamageId == c.DamageFrameId && r.SourceSeat == c.SourceSeat && r.TargetSeat == c.TargetSeat && r.Amount == c.Amount && r.OccurrenceIndex == c.OccurrenceIndex && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TurnSeat) && r.TurnNumber >= 0 && r.RecipientIndex is >= 0 and <= 2 && r.Recipients.SequenceEqual(new[] { r.SourceSeat, r.TargetSeat }.OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(r.TurnSeat, seat))) && (r.AwaitingMovement ? r.Draw is { RequestedCount: 1 } draw && r.RecipientIndex < 2 && draw.RecipientSeat == r.Recipients[r.RecipientIndex] && draw.SequenceBefore >= 0 && draw.SequenceAfter >= draw.SequenceBefore && draw.ActualDrawIds.Count <= 1 && (draw.Replacement is null || draw.ActualDrawIds.Count == 0) : r.Draw is null);
    internal void ContinueMobileLiDianWangxi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileLiDianWangxi: { } r } f || f.Id != id)
            return;
        if (!ValidMobileLiDianWangxi(f))
            throw new InvalidOperationException("Mobile Wangxi lost its frozen seating order and original per-point damage receipt.");
        if (r.AwaitingMovement)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { MobileLiDianWangxi = r = r with { RecipientIndex = r.RecipientIndex + 1, AwaitingMovement = false, Draw = null }, PendingMovementContinuation = null });
        }

        while (r.RecipientIndex < 2 && !_coreMatchParticipantsPlayersAccess._players[r.Recipients[r.RecipientIndex]].IsAlive)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLiDianWangxi = r = r with { RecipientIndex = r.RecipientIndex + 1 } });
        if (r.RecipientIndex == 2)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileLiDianWangxi = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return;
        }

        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.Recipients[r.RecipientIndex], 1, CharacterLiDianMobileDefinitions.MobileLiDianWangxiDraw);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawMobileLiDianWangxi)]
    internal void ReturnMobileLiDianWangxiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (!ValidMobileLiDianWangxi(f) || f.MobileLiDianWangxi is not { AwaitingMovement: false, Draw: null } r || r.RecipientIndex >= 2 || outcome.RecipientSeat != r.Recipients[r.RecipientIndex] || outcome.RequestedCount != 1 || outcome.ActualDrawIds.Count > 1 || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == CharacterLiDianMobileDefinitions.MobileLiDianWangxiDraw).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Mobile Wangxi cannot accept another recipient's or a repeated native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileLiDianWangxi = r with { AwaitingMovement = true, Draw = outcome }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        ContinueMobileLiDianWangxi(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 71010)]
    internal bool ResumeMobileLiDianWangxi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileLiDianWangxi.AwaitingMovement: true } f || f.Id != id)
            return false;
        ContinueMobileLiDianWangxi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 71010)]
    internal bool ReturnMobileLiDianWangxiMovement(ProgramSkillFrame f)
    {
        if (f.MobileLiDianWangxi is not { AwaitingMovement: true })
            return false;
        ContinueMobileLiDianWangxi(f.Id);
        return true;
    }
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 71010)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 71010)]
    internal bool ProveMobileLiDianWangxiRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.MobileLiDianWangxi is { AwaitingMovement: true, Draw.Replacement: not null } && ValidMobileLiDianWangxi(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 71010)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 71010)]
    internal bool KeepPaidMobileLiDianWangxi(ProgramSkillFrame f) => f.MobileLiDianWangxi is not null && ValidMobileLiDianWangxi(f);
}
