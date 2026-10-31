#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouMobileRuntime
{
    internal const string MouDuojingTakeReason = "skill-program.mou-mobile:duojing.obtain";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 29202, SkillProgramEffectOp.ResolveMouLuMengDuojing)]
    internal bool CanOfferMouDuojing(ProgramTriggerLegalityContext c) => c.SkillId == MouDuojing && MouLuMengComposition.IsDuojing(c.Program) && c.Context is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } && identity.ActorSeat == c.OwnerSeat && c.Owner.IsAlive && c.Owner.Armor > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && action.ActionId == identity.CardActionId && action.ActorSeat == c.OwnerSeat && use.SourceSeat == c.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.TargetSeats.Any(seat => seat != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive) && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not null && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingStartedEvent>().Any(e => e.CardUseId == use.Id && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == MouDuojing);
    internal CardUseFrame? MouDuojingUse(ProgramSkillFrame f)
    {
        if (f.SkillId != MouDuojing || f.TriggerId != "after-slash-targets" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } context || context.SourceSeat != f.OwnerSeat || identity.ActorSeat != f.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || !MouLuMengComposition.IsDuojing(p) || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } action } use || action.ActionId != identity.CardActionId || action.ActorSeat != f.OwnerSeat || use.SourceSeat != f.OwnerSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.EffectiveKind != use.CardKind || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != use.Id || window.Continuation != ProgramCardContinuation.Slash || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || window.Action.ActionId != action.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            return null;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return i > 1 && _resolutionStackOperations._resolutionStack[i - 1].Id == window.Id && _resolutionStackOperations._resolutionStack[i - 2].Id == use.Id ? use : null;
    }

    internal bool ValidMouDuojing(ProgramSkillFrame f)
    {
        if (f.MouLuMengDuojing is not { } r || MouDuojingUse(f)is not { } use || r.Source != MouLuMengSource(f) || r.GameplayHash != f.GameplayHash || r.WindowId != f.WindowContext!.ParentFrameId || r.CardUseId != use.Id || r.ActionId != use.Action!.ActionId || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || r.Targets.Count == 0 || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == f.OwnerSeat || !use.TargetSeats.Contains(s)) || r.TargetIndex < 0 || r.TargetIndex > r.Targets.Count || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingStartedEvent>().Count(e => e == new MouLuMengDuojingStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.CardUseId, r.ActionId, r.Phase, r.Targets.Count)) != 1)
            return false;
        if (r.Stage == MouLuMengDuojingStage.Complete)
            return r.TargetIndex == r.Targets.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingCompletedEvent>().Count(e => e.FrameId == f.Id) == 1;
        if (r.TargetIndex >= r.Targets.Count)
            return false;
        var target = r.Targets[r.TargetIndex];
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingOfferedEvent>().Count(e => e.FrameId == f.Id && e.TargetIndex == r.TargetIndex && e.TargetSeat == target) != 1)
            return false;
        if (r.Stage == MouLuMengDuojingStage.Choosing)
            return r.TakenCardId is null && r.MovementSequence is null && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingArmorPaidEvent>().Any(e => e.FrameId == f.Id && e.TargetIndex == r.TargetIndex);
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingArmorPaidEvent>().Where(e => e.FrameId == f.Id && e.TargetIndex == r.TargetIndex).ToArray()is not [var paid] || paid.TargetSeat != target || paid.CardUseId != use.Id || paid.ActionId != r.ActionId || paid.ArmorBefore < 1 || paid.ArmorAfter != paid.ArmorBefore - 1 || !use.IgnoresArmor || use.CardAttack?.IgnoresArmor != true || use.PreparedTargetAttacks?.Any(a => !a.IgnoresArmor) == true)
            return false;
        if (r.Stage == MouLuMengDuojingStage.Taking)
            return r.TakenCardId is null && r.MovementSequence is null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingTakenEvent>().Count(e => e == new MouLuMengDuojingTakenEvent(f.Id, r.TargetIndex, target, r.TakenCardId, r.MovementSequence)) == 1 && (r.TakenCardId is null && r.MovementSequence is null || r.TakenCardId is { } card && r.MovementSequence is { } sequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == sequence && m.CardId == card && m.From == CardLocation.Hand(target) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouDuojingTakeReason) == 1);
    }

    internal SkillProgramStepOutcome BeginMouDuojing(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (MouDuojingUse(f)is not { } use || f.MouLuMengDuojing is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingStartedEvent>().Any(e => e.CardUseId == use.Id && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == MouDuojing))
            throw new InvalidOperationException("Duojing needs one exact finalized actual Slash Use from its actor.");
        var targets = Array.AsReadOnly(use.TargetSeats.Where(s => s != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Distinct().ToArray());
        if (targets.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var r = new MouLuMengDuojingReceipt(MouLuMengSource(f), f.GameplayHash, f.WindowContext!.ParentFrameId, use.Id, use.Action!.ActionId, MouLuMengPhase(), targets);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuMengDuojing = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, use.Id, r.ActionId, r.Phase, targets.Count));
        OfferMouDuojingTarget(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void OfferMouDuojingTarget(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouLuMengDuojing!;
        if (r.Stage != MouLuMengDuojingStage.Choosing)
            throw new InvalidOperationException("Duojing cannot offer a paid target again.");
        while (r.TargetIndex < r.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[r.Targets[r.TargetIndex]].IsAlive)
        {
            r = r with
            {
                TargetIndex = r.TargetIndex + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuMengDuojing = r });
        }

        if (r.TargetIndex >= r.Targets.Count || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Armor <= 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            CompleteMouDuojing(f);
            return;
        }

        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingOfferedEvent>().Any(e => e.FrameId == id && e.TargetIndex == r.TargetIndex))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingOfferedEvent(id, r.TargetIndex, r.Targets[r.TargetIndex]));
        PublishMouDuojingChoice(f, MouDuojingChoices(f), "夺荆：是否失去1点护甲，令此杀无视防具并获得目标一张手牌？");
    }

    internal IReadOnlyList<PromptChoice> MouDuojingChoices(ProgramSkillFrame f)
    {
        var r = f.MouLuMengDuojing!;
        var target = r.Targets[r.TargetIndex];
        return Array.AsReadOnly(new[] { "pay", "skip" }.Select(option => new PromptChoice(new($"mou-duojing.{(f.Id)}.{(r.TargetIndex)}.{(option)}"), option == "pay" ? "失去1点护甲发动夺荆" : "不发动夺荆", [], [target], new Dictionary<string, string> { ["program-action"] = "mou-lu-meng-duojing", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["target-index"] = r.TargetIndex.ToString(CultureInfo.InvariantCulture), ["option-id"] = option })).ToArray());
    }

    internal IReadOnlyList<PromptChoice> MouDuojingTakeChoices(ProgramSkillFrame f)
    {
        var r = f.MouLuMengDuojing!;
        var target = r.Targets[r.TargetIndex];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer, destination: SkillProgramCardDestination.OwnerHand).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-lu-meng-duojing-take" } }).ToArray());
    }

    internal void PublishMouDuojingChoice(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices, string message)
    {
        var r = f.MouLuMengDuojing!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, message, choices.SelectMany(c => c.Cards).Distinct().ToArray(), [r.Targets[r.TargetIndex]], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.Targets[r.TargetIndex],
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("mou-lu-meng-duojing")]
    internal void AnswerMouDuojing(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (!ValidMouDuojing(f) || f.MouLuMengDuojing is not { Stage: MouLuMengDuojingStage.Choosing } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouDuojingChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Duojing can only pay its canonical current finalized target once.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = r.Targets[r.TargetIndex];
        if (selected.Parameters.GetValueOrDefault("option-id") == "skip" || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Armor < 1)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingSkippedEvent(f.Id, r.TargetIndex, target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuMengDuojing = r with { TargetIndex = r.TargetIndex + 1 } });
            OfferMouDuojingTarget(f.Id);
            return;
        }

        var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Armor;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuMengDuojing = r with { Stage = MouLuMengDuojingStage.Taking } });
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Armor = before - 1;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(r.CardUseId, use => use with { IgnoresArmor = true, CardAttack = use.CardAttack!with { IgnoresArmor = true }, PreparedTargetAttacks = use.PreparedTargetAttacks is null ? null : Array.AsReadOnly(use.PreparedTargetAttacks.Select(a => a with { IgnoresArmor = true }).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingArmorPaidEvent(f.Id, r.TargetIndex, target, r.CardUseId, r.ActionId, before, before - 1));
        var choices = MouDuojingTakeChoices(f);
        if (choices.Count == 0)
        {
            FinishMouDuojingTake(f, null, null);
            ContinueMouDuojing(f.Id);
            return;
        }

        PublishMouDuojingChoice(f, choices, "夺荆：选择目标的一张手牌获得。");
    }

    [ProgramAnswer("mou-lu-meng-duojing-take")]
    internal void AnswerMouDuojingTake(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        if (!ValidMouDuojing(f) || f.MouLuMengDuojing is not { Stage: MouLuMengDuojingStage.Taking } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MouDuojingTakeChoices(f).Where(c => c.Id == selected.Id).ToArray()) || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), NumberStyles.None, CultureInfo.InvariantCulture, out var slot))
            throw new InvalidOperationException("Duojing must select one exact published opaque native Hand slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var target = r.Targets[r.TargetIndex];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || slot < 0 || slot >= hand.Count)
        {
            FinishMouDuojingTake(f, null, null);
            ContinueMouDuojing(f.Id);
            return;
        }

        var card = hand[slot];
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(target), CardLocation.Hand(f.OwnerSeat), new(MouDuojingTakeReason), movement => FinishMouDuojingTake(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), card.Id, movement.Sequence), disclosure: CardMovementDisclosure.FaceDown);
        ContinueMouDuojing(f.Id);
    }

    internal void FinishMouDuojingTake(ProgramSkillFrame f, int? card, int? sequence)
    {
        var r = f.MouLuMengDuojing!;
        if (r.Stage != MouLuMengDuojingStage.Taking || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingTakenEvent>().Any(e => e.FrameId == f.Id && e.TargetIndex == r.TargetIndex))
            throw new InvalidOperationException("Duojing cannot take or finish the same target's paid reward twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuMengDuojing = r with { Stage = MouLuMengDuojingStage.MovementChildren, TakenCardId = card, MovementSequence = sequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingTakenEvent(f.Id, r.TargetIndex, r.Targets[r.TargetIndex], card, sequence));
    }

    internal bool DrainMouDuojing(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    internal void ContinueMouDuojing(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidMouDuojing(f) || f.MouLuMengDuojing is not { Stage: MouLuMengDuojingStage.MovementChildren } r)
            throw new InvalidOperationException("Duojing can resume only its paid current target's exact native children.");
        if (DrainMouDuojing(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouLuMengDuojing!;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingPhaseCreditEvent>().Any(e => e.FrameId == id && e.TargetIndex == r.TargetIndex))
            throw new InvalidOperationException("Duojing cannot grant the original phase's Slash credit twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingPhaseCreditEvent(id, r.TargetIndex, f.OwnerSeat, r.CardUseId, r.Phase));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuMengDuojing = r with { TargetIndex = r.TargetIndex + 1, Stage = MouLuMengDuojingStage.Choosing, TakenCardId = null, MovementSequence = null } });
        OfferMouDuojingTarget(id);
    }

    internal void CompleteMouDuojing(ProgramSkillFrame f)
    {
        var r = f.MouLuMengDuojing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuMengDuojing = r with { TargetIndex = r.Targets.Count, Stage = MouLuMengDuojingStage.Complete, TakenCardId = null, MovementSequence = null } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengDuojingCompletedEvent(f.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 29202)]
    internal bool ResumeMouDuojing(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.MouLuMengDuojing is not { Stage: MouLuMengDuojingStage.MovementChildren })
            return false;
        ContinueMouDuojing(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29202)]
    internal bool ReturnMouDuojingMovement(ProgramSkillFrame f)
    {
        if (f.MouLuMengDuojing is not { Stage: MouLuMengDuojingStage.MovementChildren })
            return false;
        ContinueMouDuojing(f.Id);
        return true;
    }

    internal int MouDuojingPhaseSlashBonus(int owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { } phase ? 0 : _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingPhaseCreditEvent>().Count(e => e.OwnerSeat == owner && e.Phase == phase && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == owner && s.Source.SkillId == MouDuojing && s.CardUseId == e.CardUseId && s.Phase == phase) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengDuojingArmorPaidEvent>().Any(p => p.FrameId == e.FrameId && p.TargetIndex == e.TargetIndex && p.CardUseId == e.CardUseId));
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouLuMengDuojing)]
    internal PromptChoice SelectAiMouDuojing(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouLuMengDuojing?.Stage == MouLuMengDuojingStage.Taking)
            return p.Choices[0];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = f.MouLuMengDuojing!.Targets[f.MouLuMengDuojing.TargetIndex];
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var enemy = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(view, target, new(0, 0, 0, 0, 0, 1, false, false)) > 0;
        return p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == (enemy && owner.Armor > 1 ? "pay" : "skip"));
    }
}
