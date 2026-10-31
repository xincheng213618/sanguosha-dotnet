#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHuanAnniversaryRuntime
{
    internal static CardConversionSource ZhangHuanSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 46300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 46300)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 46300)]
    internal bool ZhangHuanTargetWindows(ProgramTargetCardWindowQuery q) => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseTargetsFinalized).Any(b => b.SkillId == AnniversaryZhangHuanComposition.Yiju));
    SkillProgramStepOutcome IAnniversaryZhangHuanHost.Begin(ProgramSkillFrame f) => BeginZhangHuan(f);
    internal IReadOnlyList<AnniversaryZhangHuanDiscard> ZhangHuanDiscards()
    {
        var invocations = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(i => i.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && i.ActualTurnOwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat).ToDictionary(i => i.InvocationId);
        return Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(m => m.To == CardLocation.DiscardPile && invocations.TryGetValue(m.InvocationId, out var i) && m.CompletionSequence > i.SequenceBefore && m.CompletionSequence <= i.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == m.CompletionSequence && move.CardId == m.CardId && move.To == m.To && move.TurnNumber == i.TurnNumber && (move.From == m.From || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move) == m.From) && (move.Reason == i.Reason || move.Reason.Value == i.Reason.Value + "-finished" || move.Reason.Value == i.Reason.Value + "-discard"))).DistinctBy(m => m.CompletionSequence).OrderBy(m => m.CompletionSequence).Select(m => new AnniversaryZhangHuanDiscard(m.InvocationId, m.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind, m.From, invocations[m.InvocationId].ExecutorSeat, m.CompletionSequence)).ToArray());
    }

    internal IReadOnlyList<AnniversaryZhangHuanDiscard> ZhangHuanAvailable(ProgramSkillFrame f) => f.AnniversaryZhangHuan is not { } r ? [] : Array.AsReadOnly(ZhangHuanDiscards().GroupBy(d => d.CardId).Select(group => group.Last()).Where(d => !r.Used.Contains(d.CardId) && _cardZonesOperations._cardZones.Contains(CardLocation.DiscardPile, d.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == d.CardId)?.Sequence == d.Sequence).ToArray());
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46300, SkillProgramEffectOp.ResolveAnniversaryZhangHuanShuguo)]
    internal bool CanOfferZhangHuan(ProgramTriggerLegalityContext q) => ZhangHuanDiscards().Count > 0;
    internal SkillProgramStepOutcome BeginZhangHuan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryZhangHuan is not null || f.InstructionIndex != 1 || f.SkillId != AnniversaryZhangHuanComposition.Shuguo || !ZhangHuanParent(f))
            throw new InvalidOperationException("Shuguo requires its unpaid original actual-turn ending candidate.");
        var r = new AnniversaryZhangHuanReceipt(ZhangHuanSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, f.WindowContext!.ParentFrameId, AnniversaryZhangHuanStage.Choice)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner, r.ParentId));
        ContinueZhangHuan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsAnniversaryZhangHuanExactCardQuery(CharacterState actor, int? selected)
    {
        if (selected is not > 0 || !actor.IsAlive || !_cardZonesOperations._cardZones.Contains(CardLocation.DiscardPile, selected.Value))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryZhangHuan: { Stage: AnniversaryZhangHuanStage.Choice or AnniversaryZhangHuanStage.IssuingUse } r } f)
            return f.OwnerSeat == actor.Seat && ValidZhangHuan(f) && ZhangHuanAvailable(f).Any(d => d.CardId == selected) && (r.Stage == AnniversaryZhangHuanStage.Choice || r.Native?.Discard.CardId == selected);
        return _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { AnniversaryZhangHuanUse: { } n } use && n.Actor == actor.Seat && n.Discard.CardId == selected && IsZhangHuanNative(use);
    }

    internal Card? AnniversaryZhangHuanQueryOriginalCard(CharacterState actor, int? id) => IsAnniversaryZhangHuanExactCardQuery(actor, id) ? _coreAdvancedLifecycleOperations.GetAdvancedCard(id!.Value) : null;
    internal IReadOnlyList<LegalAction> ZhangHuanActions(ProgramSkillFrame f)
    {
        if (!ValidZhangHuan(f) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || f.AnniversaryZhangHuan!.Stop)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        return Array.AsReadOnly(ZhangHuanAvailable(f).SelectMany(d => _coreNativeLegalActionsOperations.BuildLegalActions(actor, false, d.CardId).Where(a => a.CardId == d.CardId && a.ConversionSource is null && a.AdditionalConversionSources is not { Count: > 0 } && a.MinCardCount <= 1 && a.MaxCardCount <= 1 && a.Kind is not (LegalActionKind.Recast or LegalActionKind.EndPlay or LegalActionKind.UseProgramSkill or LegalActionKind.UseEquipmentEffect or LegalActionKind.RevealGeneral) && ((a.PlayedCardKind ?? d.Kind) == d.Kind || d.Kind == CardKind.Slash && a.PlayedCardKind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor)))).Select(AnniversaryZhangHuanNative.Freeze).ToArray());
    }

    internal IReadOnlyList<PromptChoice> ZhangHuanChoices(ProgramSkillFrame f) => Array.AsReadOnly(ZhangHuanActions(f).Select((a, i) => new PromptChoice(new($"shuguo.{(f.Id)}.{(f.AnniversaryZhangHuan!.Used.Count)}.use.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, new Dictionary<string, string> { ["program-action"] = "anniversary-shuguo", ["index"] = i.ToString(CultureInfo.InvariantCulture) })).Append(new(new($"shuguo.{(f.Id)}.{(f.AnniversaryZhangHuan!.Used.Count)}.stop"), "停止使用，结算未使用牌的摸牌", [], [], new Dictionary<string, string> { ["program-action"] = "anniversary-shuguo", ["index"] = "stop" })).ToArray());
    internal void PublishZhangHuan(ProgramSkillFrame f)
    {
        var choices = ZhangHuanChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "戍国：依次使用本回合真正弃置的牌；使用他人弃置的牌后停止。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, "戍国", "戍国", _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryZhangHuanShuguo)]
    internal PromptChoice ZhangHuanAi(PendingDecision p) => p.Choices.First();
    [ProgramAnswer("anniversary-shuguo")]
    internal void AnswerZhangHuan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangHuan: { Stage: AnniversaryZhangHuanStage.Choice } r } f || !ValidZhangHuan(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ZhangHuanChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shuguo rejects a stale discarded entity, prior sequence or native target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["index"] == "stop")
        {
            DrawZhangHuan(f);
            return;
        }

        var a = ZhangHuanActions(f)[int.Parse(choice.Parameters["index"], CultureInfo.InvariantCulture)];
        var d = ZhangHuanAvailable(f).Single(d => d.CardId == a.CardId);
        var n = new AnniversaryZhangHuanNative(f.Id, f.InstructionIndex, r.Source, r.Hash, r.Turn, f.OwnerSeat, d, a, 0, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r with { Stage = AnniversaryZhangHuanStage.IssuingUse, Native = n, Returned = false, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, NativeBatches = [] } });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], a);
    }

    internal bool DrainZhangHuan(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 46300)]
    internal bool ContinueZhangHuan(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryZhangHuan: { } r } f || f.Id != id)
            return false;
        if (r.Stage is AnniversaryZhangHuanStage.Drawing or AnniversaryZhangHuanStage.IssuingUse or AnniversaryZhangHuanStage.NativeUse)
            return true;
        if (DrainZhangHuan(f))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.AnniversaryZhangHuan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == AnniversaryZhangHuanStage.DrawChildren)
        {
            FinishZhangHuan(f);
            return true;
        }

        if (r.Stage == AnniversaryZhangHuanStage.NativeTail)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r = r with { Stage = AnniversaryZhangHuanStage.Choice, Native = null, Returned = false, NativeBatches = [] } });
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishZhangHuan(f);
            return true;
        }

        if (r.Stop || ZhangHuanActions(f).Count == 0)
            DrawZhangHuan(f);
        else
            PublishZhangHuan(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46300)]
    internal bool ReturnZhangHuanMovement(ProgramSkillFrame f) => f.AnniversaryZhangHuan is not null && ContinueZhangHuan(f.Id);
    internal void DrawZhangHuan(ProgramSkillFrame f)
    {
        var r = f.AnniversaryZhangHuan!;
        if (r.Stage != AnniversaryZhangHuanStage.Choice || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHuanDrawIssuedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Shuguo must settle its unused actual-turn discards once after all real uses and children.");
        var discards = ZhangHuanDiscards().Count;
        var maximum = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => p.Seat == f.OwnerSeat || _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count);
        var count = maximum || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None ? 0 : Math.Min(5, Math.Max(0, discards - r.Used.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r with { Stage = count == 0 ? AnniversaryZhangHuanStage.Complete : AnniversaryZhangHuanStage.Drawing, TotalDiscards = discards, RequestedDraw = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanDrawIssuedEvent(f.Id, discards, r.Used.Count, maximum, count));
        if (count == 0)
            FinishZhangHuan(f);
        else
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("skill.anniversary.shuguo.draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryZhangHuanShuguo)]
    internal void ReturnZhangHuanDraw(ProgramSkillFrame supplied, NativeDrawOutcome d)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.AnniversaryZhangHuan!;
        if (!ValidZhangHuan(f) || r.Stage != AnniversaryZhangHuanStage.Drawing || r.Draw is not null || d.RecipientSeat != f.OwnerSeat || d.RequestedCount != r.RequestedDraw)
            throw new InvalidOperationException("Shuguo lost its original capped native draw obligation.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = r with { Stage = AnniversaryZhangHuanStage.DrawChildren, Draw = d, Before = d.SequenceBefore, After = d.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanDrawReturnedEvent(f.Id, d.RequestedCount, d.ActualDrawIds.Count, d.SequenceBefore, d.SequenceAfter));
        ContinueZhangHuan(f.Id);
    }

    internal void FinishZhangHuan(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryZhangHuanCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryZhangHuan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
