#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanYinAnniversaryRuntime
{
    internal static CardConversionSource YuanYinSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal static CardMoveReason YuanYinDrawReason => new("skill-program.anniversary:moshou.draw");

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 49300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 49300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 49300)]
    internal bool YuanYinTargetWindows(ProgramTargetCardWindowQuery q) => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(b => b.SkillId == AnniversaryYuanYinComposition.Moshou));
    SkillProgramStepOutcome IAnniversaryYuanYinHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveAnniversaryYuanYinMoshou ? BeginYuanYinMoshou(f) : BeginYuanYinYunjiu(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49300, SkillProgramEffectOp.ResolveAnniversaryYuanYinMoshou, SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu)]
    internal bool CanOfferYuanYin(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects is [{ Op: SkillProgramEffectOp.ResolveAnniversaryYuanYinYunjiu }])
            return CanOfferYuanYinYunjiu(c, w);
        return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue && w is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } card, Facts.CardActionCardIsBlack: true } && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } a } use && a.ActionId == card.CardActionId && use.TargetSeats.Contains(c.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginYuanYinMoshou(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryYuanYin is not null || f.InstructionIndex != 1 || f.SkillId != AnniversaryYuanYinComposition.Moshou || f.TriggerId != "black-target-cycle" || f.WindowContext is not { CardUse: { } card } w || !CanOfferYuanYin(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), w))
            throw new InvalidOperationException("Moshou requires its original real black-card target after the optional activation.");
        var source = YuanYinSource(f);
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryYuanYinMoshouAdvancedEvent>().LastOrDefault(e => e.Source == source && e.Hash == f.GameplayHash);
        var requested = previous is { Next: > 0 } ? previous.Next : Math.Max(1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp);
        var next = requested <= 1 ? 0 : requested - 1;
        var r = new AnniversaryYuanYinReceipt(source, f.GameplayHash, w.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, "moshou", "drawing")
        {
            UseId = card.ParentCardUseFrameId,
            ActionId = card.CardActionId,
            Requested = requested,
            Next = next
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinStartedEvent(f.Id, source, r.Hash, r.ParentId, r.Turn, r.Mode));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinMoshouIssuedEvent(f.Id, source, r.Hash, r.UseId, r.ActionId, requested, next));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, requested, YuanYinDrawReason);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryYuanYinMoshou)]
    internal void ReturnYuanYinMoshouDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        if (f.AnniversaryYuanYin is not { Mode: "moshou", Stage: "drawing", Draw: null } r || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Requested || draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Moshou lost its one original native draw request and typed result.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = r with { Stage = "draw-children", Draw = draw, Before = draw.SequenceBefore, After = draw.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinMoshouAdvancedEvent(f.Id, r.Source, r.Hash, r.Requested, r.Next, draw.ActualDrawIds.Count, draw.SequenceBefore, draw.SequenceAfter));
        ContinueYuanYin(f.Id);
    }

    internal bool DrainYuanYin(ProgramSkillFrame f) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 49300)]
    internal bool ContinueYuanYin(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryYuanYin: { } r } f || f.Id != id)
            return false;
        if (r.Mode == "yunjiu")
        {
            ContinueYuanYinYunjiu(f);
            return true;
        }

        if (r.Stage == "drawing")
            return true;
        if (!ValidYuanYin(f))
            throw new InvalidOperationException("Yuan Yin lost his original paid black-target draw.");
        if (DrainYuanYin(f))
            return true;
        FinishYuanYin(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 49300)]
    internal bool ReturnYuanYinMovement(ProgramSkillFrame f) => f.AnniversaryYuanYin is not null && ContinueYuanYin(f.Id);
    internal void FinishYuanYin(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryYuanYinCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryYuanYin = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
