#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunXuAnniversaryRuntime
{
    internal static CardConversionSource XunXuSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasAnniversaryXunXu => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXunXuDiyin);
    internal static CardMoveReason XunXuDrawReason => new("skill.anniversary.xun-xu.draw");
    internal static CardMoveReason XunXuGainReason => new("skill.anniversary.diyin.claim");
    internal static CardMoveReason XunXuDiscardReason => new("skill.anniversary.boqia.adjust");

    internal void CaptureAnniversaryXunXuMovement(CardMovementBatchContext batch)
    {
        if (!HasAnniversaryXunXu)
            return;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.DiscardPileReceived).Where(c => c.SkillId == AnniversaryXunXuComposition.Diyin && c.BindingId == "next-discard"))
            {
                var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
                var prior = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuLossEvent>().LastOrDefault(e => e.Source == source)?.Occurrence ?? 0;
                var claimed = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuClaimEvent>().Where(e => e.Source == source).Select(e => e.ThroughSeventh).DefaultIfEmpty().Max();
                if (prior / 7 > claimed && batch.Movements.Any(m => m.To == CardLocation.DiscardPile && m.From != m.To))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuClaimEvent(source, batch.Id, prior / 7, batch.Movements.Min(m => m.Sequence) - 1, batch.Movements.Max(m => m.Sequence)));
                if (batch.Movements.Any(m => m.From.OwnerSeat == owner.Seat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.From != m.To) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuLossEvent>().Any(e => e.Source == source && e.BatchId == batch.Id))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuLossEvent(source, batch.Id, prior + 1, batch.Movements.Max(m => m.Sequence)));
            }
    }

    internal int[] XunXuDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
        return !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuClaimEvent>().Any(e => e.Source == source && e.BatchId == batch.Id) ? [] : batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.To == CardLocation.DiscardPile && x.m.From != x.m.To && _cardZonesOperations._cardZones.GetLocation(x.m.CardId) == CardLocation.DiscardPile).Select(x => x.i).ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.SelectDiscardBatchIndexes, 46100)]
    internal int[]? SelectXunXuDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate c, ProgramInstructionFeatures features) => c.SkillId == AnniversaryXunXuComposition.Diyin && c.BindingId == "next-discard" ? XunXuDiscardIndexes(batch, c) : null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 46100, SkillProgramEffectOp.ResolveAnniversaryXunXuDiyin, SkillProgramEffectOp.ResolveAnniversaryXunXuBoqiaCompletion)]
    internal bool CanOfferXunXu(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId == AnniversaryXunXuComposition.Boqia)
            return context.CardUse is { } u && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuUseEvent>().Any(e => e.ActionId == u.CardActionId && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.Reward) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuRearmedEvent>().Any(e => e.ActionId == u.CardActionId);
        return trigger.Id == "same-hp" ? context.TargetSeat == c.OwnerSeat && context.Amount > 0 && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue : context.MovementBatch is { } b && XunXuDiscardIndexes(b, c).Length > 0;
    }

    SkillProgramStepOutcome IAnniversaryXunXuHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginXunXu(f, op);
    internal SkillProgramStepOutcome BeginXunXu(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryXunXu is not null)
        {
            ContinueXunXu(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.InstructionIndex != 1 || f.WindowContext is not { } w || f.TriggerId is null || f.ActivationId != f.TriggerId || operation != _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op)
            throw new InvalidOperationException("Xun Xu requires the accepted sourced trigger and original instruction.");
        var mode = operation == SkillProgramEffectOp.ResolveAnniversaryXunXuBoqiaCompletion ? "boqia" : w.Window == SkillProgramTriggerWindow.DiscardPileReceived ? "claim" : "hp";
        var receipt = new AnniversaryXunXuReceipt(XunXuSource(f), f.GameplayHash, w.ParentFrameId, mode, mode == "boqia" ? "target" : mode)
        {
            Target = f.OwnerSeat,
            ActionId = w.CardUse?.CardActionId ?? 0
        };
        if (mode == "boqia" && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == receipt.ActionId)is { } native)
        {
            var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => IsXunXuFinishedMaterialBatch(native, b)).OrderBy(b => b.Id).ToArray();
            receipt = receipt with
            {
                NativeUseId = native.Id,
                NativeFinishBatches = batches,
                Stage = batches.Length > 0 ? "native-children" : "target"
            };
            foreach (var batch in batches)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuNativeBatchEvent(f.Id, native.Id, receipt.ActionId, batch.Id, batch.Movements.Min(m => m.Sequence) - 1, batch.Movements.Max(m => m.Sequence)));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = receipt, PendingMovementContinuation = receipt.Stage == "native-children" ? new(f.OwnerSeat, 0, null) : null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuStartedEvent(f.Id, receipt.Source, receipt.Hash, receipt.ParentId, mode, receipt.ActionId));
        ContinueXunXu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> XunXuChoices(ProgramSkillFrame f)
    {
        var r = f.AnniversaryXunXu!;
        PromptChoice C(string key, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"anniversary-xun-xu.{(f.Id)}.{(r.Stage)}.{(key)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "anniversary-xun-xu", ["branch"] = key });
        if (r.Stage == "target")
            return Array.AsReadOnly(_coreTableOrderOperations.CurrentTableOrder().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreMatchParticipantsPlayersAccess._players[s].HasHpValue).Select(s => C("target-" + s, "将" + _coreMatchParticipantsPlayersAccess._players[s].Name + "的手牌调整至体力上限", [], [s])).ToArray());
        if (r.Stage != "discard")
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Target, r.Target, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !r.Cards.Contains(c.Id)).Select(c => C("card-" + c.Cards.Single(), c.Label, c.Cards, [r.Target])).ToArray());
    }

    internal void PublishXunXu(ProgramSkillFrame f)
    {
        var r = f.AnniversaryXunXu!;
        var choices = XunXuChoices(f);
        var chooser = r.Stage == "discard" ? r.Target : f.OwnerSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || choices.Count == 0)
        {
            FinishXunXu(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, chooser, choices, r.Stage == "target" ? "博洽：令一名角色将手牌调整至其体力上限。" : "博洽：由手牌拥有者弃置超出的手牌。", r.Target);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("anniversary-xun-xu")]
    internal void AnswerXunXu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXunXu: { } r } f || !XunXuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Xun Xu rejects a stale participant or private card choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == "target")
        {
            var target = choice.Targets.Single();
            var maximum = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[target].MaxHp);
            var difference = maximum - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count;
            if (difference > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = r with { Target = target, Count = difference, Stage = "drawing" } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, target, difference, XunXuDrawReason);
                return;
            }

            var legal = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, target, target, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard).Count;
            var count = Math.Min(-difference, legal);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = r with { Target = target, Count = count, Stage = count > 0 ? "discard" : "finish" } });
            if (count > 0)
                PublishXunXu(f);
            else
                FinishXunXu(f);
            return;
        }

        var ids = r.Cards.Append(choice.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = r with { Cards = ids } });
        if (ids.Length < r.Count)
        {
            PublishXunXu(f);
            return;
        }

        if (ids.Length != r.Count || ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.Target) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(r.Target, _coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Hand(r.Target), OwnedCardMoveIntent.Discard)))
            throw new InvalidOperationException("Boqia must really pay the receiver's complete currently legal excess Hand cards.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = f.AnniversaryXunXu!with { Stage = "children", Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, ids, XunXuDiscardReason, (_, _) => RecordXunXuMovement(f.Id, "discard", r.Target, ids.Length, before));
        ContinueXunXu(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 46100)]
    internal bool ContinueXunXu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXunXu: not null } top || top.Id != id)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryXunXu: { } r } f && f.Id == id)
        {
            if (r.Stage == "native-children")
            {
                if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
                    return true;
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXunXu = r with { Stage = "target" }, PendingMovementContinuation = null });
                continue;
            }

            if (r.Stage is "target" or "discard")
            {
                PublishXunXu(f);
                return true;
            }

            if (r.Stage == "drawing")
                return true;
            if (DrainXunXu(f))
                return true;
            if (f.PendingMovementContinuation is not null)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
                r = f.AnniversaryXunXu!;
            }

            if (r.Stage == "hp")
            {
                if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue)
                {
                    FinishXunXu(f);
                    return true;
                }

                foreach (var seat in _coreTableOrderOperations.CurrentTableOrder().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreMatchParticipantsPlayersAccess._players[s].HasHpValue))
                    if (_characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoQuery(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), seat, "diyin-hp"))
                        return true;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var hp = _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, f.OwnerSeat, "diyin-hp");
                var recipients = _coreTableOrderOperations.CurrentTableOrder().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && _coreMatchParticipantsPlayersAccess._players[s].HasHpValue && _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoQueryValue(f, s, "diyin-hp") == hp).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXunXu = r with { Recipients = recipients, Stage = "next-draw" } });
                continue;
            }

            if (r.Stage == "claim")
            {
                if (f.WindowContext?.MovementBatch is not { } batch)
                    throw new InvalidOperationException("Diyin lost its original next-discard batch.");
                var ids = XunXuDiscardIndexes(batch, new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0)).Select(i => batch.Movements[i].CardId).Distinct().ToArray();
                if (ids.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishXunXu(f);
                    return true;
                }

                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = r with { Stage = "children", Cards = ids, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), XunXuGainReason, afterRequestSettled: settlement =>
                {
                    _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainRequestSettlement(settlement, id, ids, CardLocation.Hand(f.OwnerSeat), before);
                    var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(active with { AnniversaryXunXu = active.AnniversaryXunXu!with { Gain = settlement } });
                    RecordXunXuMovement(id, "claim", f.OwnerSeat, ids.Length, before);
                }, producerFrameId: id);
                continue;
            }

            if (r.Mode == "hp" && r.Stage is "next-draw" or "children")
            {
                var cursor = r.Cursor;
                while (cursor < r.Recipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.Recipients[cursor]].IsAlive)
                    cursor++;
                if (cursor >= r.Recipients.Count)
                {
                    FinishXunXu(f);
                    return true;
                }

                var recipient = r.Recipients[cursor];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = r with { Cursor = cursor + 1, Target = recipient, Count = 1, Stage = "drawing" } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, 1, XunXuDrawReason);
                return true;
            }

            FinishXunXu(f);
            return true;
        }

        return true;
    }

    internal void RecordXunXuMovement(long id, string mode, int target, int count, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXunXu = f.AnniversaryXunXu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuMovementEvent(id, mode, target, count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    internal bool DrainXunXu(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveAnniversaryXunXuDiyin, SkillProgramEffectOp.ResolveAnniversaryXunXuBoqiaCompletion)]
    internal void ReturnXunXuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.AnniversaryXunXu is not { Stage: "drawing" } r || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.Count || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.AttemptFrameId is { } attempt && r.Draws.Any(d => d.AttemptFrameId == attempt))
            throw new InvalidOperationException("Xun Xu lost the original once-issued native draw result.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { AnniversaryXunXu = r with { Stage = "children", Draws = r.Draws.Append(outcome).ToArray(), Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuDrawEvent(f.Id, r.Draws.Count, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ContinueXunXu(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 46100)]
    internal bool ReturnXunXuMovement(ProgramSkillFrame f) => f.AnniversaryXunXu is not null && ContinueXunXu(f.Id);
    internal void FinishXunXu(ProgramSkillFrame f)
    {
        var r = f.AnniversaryXunXu!;
        if (r.Mode == "boqia" && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnniversaryXunXuRearmedEvent>().Any(e => e.ActionId == r.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuRearmedEvent(f.Id, f.OwnerSeat, r.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.Target));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXunXu = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryXunXuDiyin)]
    internal PromptChoice ChooseXunXuDiyin(PendingDecision p, ProgramSkillFrame f) => p.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveAnniversaryXunXuBoqiaCompletion)]
    internal PromptChoice ChooseXunXuBoqia(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Targets.Contains(f.OwnerSeat)) ?? p.Choices[0];
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46100)]
    internal bool RetainXunXu(ProgramSkillFrame f) => f.AnniversaryXunXu is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5500)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureAnniversaryXunXuMovement(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureAnniversaryXunXuMovement(completed);
        return completed;
    }
}
