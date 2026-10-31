#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    internal const string WeifuDiscardReason = RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.WeifuDiscardReason, WeifuDrawReason = "skill-program.qian-zhao.weifu.draw";
    internal const string WeifuJudgmentReason = "weifu", WeifuJudgmentBind = "weifu-final-result";
    internal static CardConversionSource QianZhaoSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_qian_zhao_ol_Weifu.QianZhaoSource(f);
    internal SkillProgramStepOutcome BeginWeifu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "ol:weifu" || f.InstructionIndex != 1 || f.TriggerId is not null || f.WindowContext is not null || f.Weifu is not null || f.SelectedCardIds is not [var id] || f.SelectedTargetSeats.Count != 0 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Weifu requires its exact one-card native Play entry.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || card.IsGeneralWeapon || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard) || from.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, card))
            throw new InvalidOperationException("Weifu cannot replace its exact legal HE discard.");
        var r = new WeifuReceipt(QianZhaoSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, id, from, CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(WeifuDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = f.Weifu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwnerSeat, id, from, r.CostCategory, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainQianZhaoChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26620)]
    internal bool ResumeWeifu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weifu: { } r } f || f.Id != id)
            return false;
        if (r.Stage == WeifuStage.Judging)
            return true;
        if (DrainQianZhaoChildren(id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.Weifu!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishQianZhao(f);
            return true;
        }

        if (r.Stage == WeifuStage.PaymentChildren)
        {
            var judgment = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = r with { Stage = WeifuStage.Judging, JudgmentId = judgment, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = null });
            _ = _coreProgramLifecycleOperations.StartProgramJudgment(f, f.OwnerSeat, WeifuJudgmentReason, WeifuJudgmentBind, SkillProgramCardSetVisibility.Public, f.OwnerSeat);
            return true;
        }

        if (r.Stage == WeifuStage.JudgmentChildren)
        {
            if (r.ResultCategory is null || r.JudgmentId is null)
                throw new InvalidOperationException("Weifu lost its finalized native judgment.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = r with { Issued = true, Stage = WeifuStage.DrawChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            if (r.ResultCategory is SkillProgramCardCategory.Basic or SkillProgramCardCategory.Trick)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuGrantedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwnerSeat, r.ResultCategory.Value, r.JudgmentId.Value));
            var drawn = r.CostCategory == r.ResultCategory ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(WeifuDrawReason)).Count : 0;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = f.Weifu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawCount = drawn } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuDrawPaidEvent(id, drawn));
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        FinishQianZhao(f);
        return true;
    }

    internal bool MatchesWeifuJudgmentInstruction(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => f.Weifu is { Stage: WeifuStage.Judging, JudgmentId: not null } r && f.SkillId == "ol:weifu" && f.TriggerId is null && f.WindowContext is null && r.Source == QianZhaoSource(f) && r.GameplayHash == f.GameplayHash && r.InstructionIndex == f.InstructionIndex && f.InstructionIndex == 1 && target == f.OwnerSeat && reason == WeifuJudgmentReason && bind == WeifuJudgmentBind && visibility == SkillProgramCardSetVisibility.Public && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.ResolveWeifuJudgment;
    internal bool IsWeifuJudgment(JudgmentFrame j, ProgramSkillFrame f) => j.Continuation == JudgmentContinuationKind.ProgramSkill && f.Weifu?.JudgmentId == j.Id && j.ParentFrameId == f.Id && j.SourceSeat == f.OwnerSeat && j.ParentAttackId is null && MatchesWeifuJudgmentInstruction(f, j.TargetSeat, j.Reason, j.ProgramResultBind ?? "", j.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26620)]
    internal void FreezeWeifuJudgmentResult(IGameEvent payload)
    {
        if (payload is not JudgmentResolvedEvent { CardId: { } card, CardKind: { } kind } e || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == e.ParentResolutionId)is not { Weifu: { } r } f)
            return;
        if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is not { } judgment || !IsWeifuJudgment(judgment, f) || judgment.Id != e.ResolutionId || r.ResultKind is not null)
            throw new InvalidOperationException("Weifu cannot freeze a different or duplicate judgment result.");
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Weifu = r with { ResultCardId = card, ResultKind = kind, ResultCategory = category } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WeifuJudgedEvent(f.Id, judgment.Id, card, kind, category));
    }

    internal bool TryReturnWeifuJudgment(JudgmentFrame j)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Weifu: { } r } f || f.Id != j.ParentFrameId)
            return false;
        if (!IsWeifuJudgment(j, f) || r.ResultCardId is not { } id || r.ResultKind is null || !_coreProgramEventHistoryOperations.ProgramEventHistory<WeifuJudgedEvent>().Any(e => e.FrameId == f.Id && e.JudgmentId == j.Id && e.CardId == id && e.Kind == r.ResultKind))
            throw new InvalidOperationException("Weifu lost its exact paid native judgment return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = r with { Stage = WeifuStage.JudgmentChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = f.Weifu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal void FinishQianZhao(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QianZhaoCompletedEvent(f.Id, f.SkillId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Weifu = null, WeifuTargets = null, Kuansai = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26620)]
    internal bool ReturnQianZhaoMovement(ProgramSkillFrame f) => f.Weifu is not null ? ResumeWeifu(f.Id) : f.Kuansai is not null && ResumeKuansai(f.Id);
    SkillProgramStepOutcome IQianZhaoHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveWeifuJudgment => BeginWeifu(f),
        SkillProgramEffectOp.OfferWeifuCategoryTargets => BeginWeifuTargets(f),
        SkillProgramEffectOp.ResolveKuansaiTargetChoice => BeginKuansai(f),
        _ => throw new InvalidOperationException("Unknown Qian Zhao native operation.")};
}
