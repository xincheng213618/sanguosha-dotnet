#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuanJiongOlRuntime
{
    internal const string SaoguDiscardReason = "skill-program.duan-jiong.saogu.discard", SaoguDrawReason = "skill-program.duan-jiong.saogu.draw";
    internal bool HasDuanJiongSaoguCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveSaoguConversionSweep);

    internal static CardConversionSource DuanJiongSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_duan_jiong_ol_DuanJiong.DuanJiongSource(f);
    internal static int SaoguSuitBit(Suit suit) => RuntimeSkills.RuntimeRules_duan_jiong_ol_DuanJiong.SaoguSuitBit(suit);
    internal int SaoguDiscardedSuitMask(int actor, ActualDiscardRecoveryPhaseKey phase) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguDiscardSuitEvent>().Where(e => e.OwnerSeat == actor && e.Phase == phase).Aggregate(0, (bits, e) => bits | SaoguSuitBit(e.Suit));
    internal void CaptureDuanJiongSaoguBatch(CardMovementBatchContext batch)
    {
        if (!HasDuanJiongSaoguCapability || batch.DiscardRecoveryPhase is not { } phase || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return;
        foreach (var m in batch.Movements)
        {
            var source = _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) ?? (_coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(m) && CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(m.Reason) ? m.From : (CardLocation? )null);
            if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || source is not { OwnerSeat: { } actor } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguDiscardSuitEvent>().Any(e => e.MovementSequence == m.Sequence))
                continue;
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguDiscardSuitEvent(batch.Id, phase, actor, m.CardId, m.Sequence, suit));
        }
    }

    internal IReadOnlyList<DuanJiongSaoguCard> SaoguDiscardable(int actor, int forbidden, string skill, string instance) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(actor)).Where(c => !c.IsGeneralWeapon && (SaoguSuitBit(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], c)) & forbidden) == 0 && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(actor, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(actor, skill, instance, c)).Select(c => new DuanJiongSaoguCard(c.Id, _cardZonesOperations._cardZones.GetLocation(c.Id), c.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], c))).ToArray());
    SkillProgramStepOutcome IDuanJiongProgramHost.Resolve(ProgramSkillFrame f) => BeginDuanJiongSaogu(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 27060, SkillProgramEffectOp.ResolveSaoguConversionSweep)]
    internal bool AllowsDuanJiongSaogu(CharacterState owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == owner.Seat && _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(i => i.SkillId == "ol:saogu" && (_coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, i.SkillId, i.SkillInstanceId, "yin") || SaoguDiscardable(owner.Seat, SaoguDiscardedSuitMask(owner.Seat, phase), i.SkillId, i.SkillInstanceId).Count >= 2));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27060)]
    internal bool AllowsDuanJiongEnding(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSaoguConversionSweep) || context.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat) && SaoguDiscardable(c.OwnerSeat, 0, c.SkillId, c.SkillInstanceId).Count > 0;
    internal SkillProgramStepOutcome BeginDuanJiongSaogu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var ending = f.TriggerId is not null;
        if (f.DuanJiongSaogu is not null || f.SkillId != "ol:saogu" || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase || phase.ActorSeat != f.OwnerSeat || phase.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || (ending ? f.TriggerId != "delegate-current" || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || phase.Kind != ActualDiscardRecoveryPhaseKind.Ending : f.ActivationId != "sweep" || f.WindowContext is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || phase.Kind != ActualDiscardRecoveryPhaseKind.Play))
            throw new InvalidOperationException("Saogu requires its exact living source and actual own Play or Ending phase.");
        var yang = !_coreProgramPersistentStateOperations.GetProgramBooleanState(f, "yin");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = new(DuanJiongSource(f), f.GameplayHash, f.InstructionIndex, phase, ending, yang, f.OwnerSeat, ending ? DuanJiongSaoguStage.DelegateChoice : yang ? DuanJiongSaoguStage.DiscardChoice : DuanJiongSaoguStage.DrawChildren) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguStartedEvent(f.Id, DuanJiongSource(f), f.GameplayHash, phase, ending, yang));
        if (ending)
        {
            PublishSaogu(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (yang)
            PrepareSaoguDiscard(f);
        else
            PaySaoguDraw(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal Dictionary<string, string> SaoguArgs(string branch) => new()
    {
        ["program-action"] = "duan-jiong-saogu",
        ["branch"] = branch
    };
    internal IReadOnlyList<PromptChoice> SaoguChoices(ProgramSkillFrame f)
    {
        var r = f.DuanJiongSaogu!;
        if (r.Stage == DuanJiongSaoguStage.DelegateChoice)
            return Array.AsReadOnly(SaoguDiscardable(f.OwnerSeat, 0, f.SkillId, f.SkillInstanceId).SelectMany(card => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => new PromptChoice(new($"saogu.{(f.Id)}.delegate.{(card.CardId)}.{(p.Seat)}"), $"弃置【{(_coreCardAttackStateOperations.GetAttackCard(card.CardId).DisplayName)}】，令{(p.Name)}执行{((r.Yang ? "阳" : "阴"))}项。", [card.CardId], [p.Seat], SaoguArgs("delegate")))).ToArray());
        if (r.Stage == DuanJiongSaoguStage.DiscardChoice)
        {
            var legal = SaoguDiscardable(r.ActorSeat, r.ForbiddenSuitMask, f.SkillId, f.SkillInstanceId);
            return Array.AsReadOnly(r.Candidates.Where(c => legal.Contains(c) && !r.Selected.Contains(c)).Select(c => new PromptChoice(new($"saogu.{(f.Id)}.discard.{(r.Selected.Count)}.{(c.CardId)}"), $"弃置【{(_coreCardAttackStateOperations.GetAttackCard(c.CardId).DisplayName)}】（{(r.Selected.Count + 1)}/{(r.RequiredCount)}）", [c.CardId], [], SaoguArgs("discard"))).ToArray());
        }

        if (r.Stage == DuanJiongSaoguStage.UseChoice)
            return Array.AsReadOnly(SaoguNativeActions(f).Select((a, i) =>
            {
                var args = SaoguArgs("use");
                args["index"] = i.ToString(CultureInfo.InvariantCulture);
                return new PromptChoice(new($"saogu.{(f.Id)}.use.{(r.UsedCardIds.Count)}.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats, args);
            }).ToArray());
        return[];
    }

    internal void PublishSaogu(ProgramSkillFrame f)
    {
        var r = f.DuanJiongSaogu!;
        var choices = SaoguChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Saogu cannot publish an empty mandatory native choice.");
        var actor = r.Stage == DuanJiongSaoguStage.DelegateChoice ? f.OwnerSeat : r.ActorSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, r.Stage == DuanJiongSaoguStage.UseChoice ? "必须使用其中可合法使用的杀。" : "扫谷：选择原生付款及执行者。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = r.Stage != DuanJiongSaoguStage.UseChoice,
            Choices = choices,
            TargetSeat = actor,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("duan-jiong-saogu")]
    internal void AnswerSaogu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuanJiongSaogu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == DuanJiongSaoguStage.DelegateChoice ? f.OwnerSeat : r.ActorSeat) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], SaoguChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Saogu requires the exact original chooser, materials and native targets.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == DuanJiongSaoguStage.UseChoice)
        {
            IssueSaoguUse(f, choice);
            return;
        }

        if (r.Stage == DuanJiongSaoguStage.DelegateChoice)
        {
            var fee = SaoguDiscardable(f.OwnerSeat, 0, f.SkillId, f.SkillInstanceId).Single(c => c.CardId == choice.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { ActorSeat = choice.Targets.Single(), DelegateFee = fee, Selected = [fee], Stage = DuanJiongSaoguStage.DelegateChildren } });
            PaySaoguDiscard(f, true);
            ContinueSaogu(f.Id);
            return;
        }

        var selected = r.Selected.Append(r.Candidates.Single(c => c.CardId == choice.Cards.Single())).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { Selected = selected } });
        if (selected.Length < r.RequiredCount)
        {
            PublishSaogu(f);
            return;
        }

        PaySaoguDiscard(f, false);
        ContinueSaogu(f.Id);
    }

    internal void PrepareSaoguDiscard(ProgramSkillFrame f)
    {
        var r = f.DuanJiongSaogu!;
        var mask = SaoguDiscardedSuitMask(r.ActorSeat, r.Phase) | (r.DelegateFee is { } fee ? SaoguSuitBit(fee.Suit) : 0);
        var legal = SaoguDiscardable(r.ActorSeat, mask, f.SkillId, f.SkillInstanceId);
        var count = r.Ending ? Math.Min(2, legal.Count) : 2;
        if (legal.Count < count || count == 0)
        {
            FinishSaogu(f, "no-legal-discard");
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { Stage = DuanJiongSaoguStage.DiscardChoice, ForbiddenSuitMask = mask, RequiredCount = count, Candidates = legal, Selected = [] } });
        PublishSaogu(f);
    }

    internal void PaySaoguDiscard(ProgramSkillFrame f, bool fee)
    {
        var r = f.DuanJiongSaogu!;
        var actor = fee ? f.OwnerSeat : r.ActorSeat;
        var legal = SaoguDiscardable(actor, fee ? 0 : r.ForbiddenSuitMask, f.SkillId, f.SkillInstanceId);
        if (r.Selected.Count != (fee ? 1 : r.RequiredCount) || r.Selected.Any(c => !legal.Contains(c)))
            throw new InvalidOperationException("Saogu cannot replace its exact selected legal HE payment.");
        var grain = r.Selected.Any(c => c.From == CardLocation.Equipment(actor) && c.Kind == CardKind.WoodenOx) ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[actor]).Select(c => c.Id).ToArray() : [];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { Stage = fee ? DuanJiongSaoguStage.DelegateChildren : DuanJiongSaoguStage.DiscardChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, WoodenOxGrainIds = grain, Payments = fee ? r.Payments : r.Selected } });
        if (!fee && !r.Ending)
            _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "yin", true);
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, actor, r.Selected.Select(c => c.CardId).ToArray(), new(SaoguDiscardReason), (_, movements) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var receipt = active.DuanJiongSaogu!;
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { DuanJiongSaogu = receipt with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, PaidBatchId = _activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Peek() } });
            foreach (var m in movements)
            {
                var paid = r.Selected.Single(c => c.CardId == m.CardId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguEntityPaidEvent(f.Id, fee, actor, m.CardId, m.From, m.Sequence, paid.Kind, paid.Suit));
            }

            if (!fee)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguBranchPaidEvent(f.Id, actor, true, !r.Ending, movements.Count, receipt.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { DuanJiongSaogu = current.DuanJiongSaogu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal void PaySaoguDraw(ProgramSkillFrame f)
    {
        var r = f.DuanJiongSaogu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { Stage = DuanJiongSaoguStage.DrawChildren, Paid = true, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Selected = [], WoodenOxGrainIds = [] } });
        if (!r.Ending)
            _coreProgramSkillChangesOperations.SetProgramBooleanState(f, "yin", false);
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.ActorSeat], 1, true, new(SaoguDrawReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = f.DuanJiongSaogu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawCount = drawn.Count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguBranchPaidEvent(f.Id, r.ActorSeat, false, !r.Ending, drawn.Count, f.DuanJiongSaogu!.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueSaogu(f.Id);
    }

    internal bool DrainSaoguChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueSaogu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuanJiongSaogu: { } r } f || f.Id != id)
            return;
        if (r.Stage is DuanJiongSaoguStage.DelegateChoice or DuanJiongSaoguStage.DiscardChoice or DuanJiongSaoguStage.UseChoice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishSaogu(f);
            return;
        }

        if (r.Stage == DuanJiongSaoguStage.NativeUse)
            throw new InvalidOperationException("An issued Saogu Slash must return through its native origin.");
        if (DrainSaoguChildren(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.DuanJiongSaogu!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive)
        {
            FinishSaogu(f, "actor-dead-or-game-ended");
            return;
        }

        if (r.Stage == DuanJiongSaoguStage.DelegateChildren)
        {
            if (r.Yang)
                PrepareSaoguDiscard(f);
            else
                PaySaoguDraw(f);
            return;
        }

        if (r.Stage == DuanJiongSaoguStage.DrawChildren)
        {
            FinishSaogu(f, "yin-draw-finished");
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = r with { Stage = DuanJiongSaoguStage.UseChoice, SelectedUseCardId = null, ReservedUseId = null, NativeUse = null, NativeCompleted = false } });
        if (SaoguNativeActions(f).Count == 0)
            FinishSaogu(f, "all-available-slashes-used");
        else
            PublishSaogu(f);
    }

    internal void FinishSaogu(ProgramSkillFrame f, string reason)
    {
        var r = f.DuanJiongSaogu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuanJiongSaoguCompletedEvent(f.Id, r.ActorSeat, r.Ending, r.Yang, r.UsedCardIds.Count, reason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { DuanJiongSaogu = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27060)]
    internal bool ResumeSaogu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DuanJiongSaogu: not null } f || f.Id != id)
            return false;
        ContinueSaogu(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27060)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27060)]
    internal bool ContinuePaidSaogu(ProgramSkillFrame f) => f.DuanJiongSaogu is { Paid: true } && ValidSaogu(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveSaoguConversionSweep)]
    internal PromptChoice ChooseSaoguAi(PendingDecision p, ProgramSkillFrame f)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(p.PlayerSeat);
        double Hostility(int seat) => _coreAiBrainsAiBrainsAccess._aiBrains[p.PlayerSeat].ScoreProgramTarget(view, seat, new(0, 0, 0, 0, 0, 1, false, false));
        return p.Choices.OrderBy(c => c.Cards.Sum(id => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(id), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])) - c.Targets.Sum(Hostility)).First();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2600)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuanJiongSaoguBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureDuanJiongSaoguBatch(completed);
        return completed;
    }
}
