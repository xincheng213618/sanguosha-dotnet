#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDragonBoatGeneralsRuntime
{
    internal bool DragonBoatAllies(int owner, int other) => _coreDragonBoatActivitiesOperations.IsDragonBoatActivity && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(other) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[other].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[other].TeamId == team;
    SkillProgramStepOutcome IDragonBoatGeneralHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation, int targetSeat) => BeginDragonBoatGeneral(frame, operation, targetSeat);
    bool IDragonBoatAllyQuery.IsTeammate(int ownerSeat, int targetSeat) => ownerSeat != targetSeat && DragonBoatAllies(ownerSeat, targetSeat);
    internal void CaptureDragonBoatFinalizedUse(long useId, CardActionContext action)
    {
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber < 1 || action.Type != CardActionType.Use || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } actual } use || actual.ActionId != action.ActionId || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, action) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
            throw new InvalidOperationException("Dragon Boat Lianpian requires its exact finalized native card use.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatActualUseEvent>().Any(e => e.UseId == useId))
            return;
        var targets = _characterSuFeiOlOperations.SuFeiPrimaryTargets(action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatActualUseEvent(useId, action.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.ActorSeat, targets.Length));
        for (var index = 0; index < targets.Length; index++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatActualUseTargetEvent(useId, action.ActionId, index, targets[index]));
    }

    internal IReadOnlyList<int> DragonBoatLianpianTargets(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || identity.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !DragonBoatAllies(ownerSeat, identity.ActorSeat) || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { } action } use || action.ActionId != identity.CardActionId)
            return[];
        var uses = _coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatActualUseEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && e.ActorSeat == action.ActorSeat).ToArray();
        if (uses.Length < 2 || uses[^1].UseId != use.Id || uses[^1].ActionId != action.ActionId)
            return[];
        var previous = _coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatActualUseTargetEvent>().Where(e => e.UseId == uses[^2].UseId && e.ActionId == uses[^2].ActionId).OrderBy(e => e.Index).Select(e => e.TargetSeat).ToArray();
        var current = _coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatActualUseTargetEvent>().Where(e => e.UseId == use.Id && e.ActionId == action.ActionId).OrderBy(e => e.Index).Select(e => e.TargetSeat).ToArray();
        if (previous.Length != uses[^2].TargetCount || current.Length != uses[^1].TargetCount || !current.SequenceEqual(_characterSuFeiOlOperations.SuFeiPrimaryTargets(action)))
            throw new InvalidOperationException("Activity Lianpian lost its actual consecutive target facts.");
        return Array.AsReadOnly(previous.Intersect(current).Where(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive).ToArray());
    }

    internal DragonBoatDianhuArmedEvent? DragonBoatDianhuTarget(int ownerSeat, string instance) => _coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatDianhuArmedEvent>().SingleOrDefault(e => e.OwnerSeat == ownerSeat && e.SkillInstanceId == instance);
    internal bool DragonBoatDianhuQualified(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreDragonBoatActivitiesOperations.IsDragonBoatActivity && context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0, DamageFrameId: { } damageId } && DragonBoatAllies(candidate.OwnerSeat, source) && DragonBoatDianhuTarget(candidate.OwnerSeat, candidate.SkillInstanceId)is { } relation && relation.TargetSeat == target && context.OccurrenceIndex >= 0 && context.OccurrenceIndex < context.Amount && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().LastOrDefault(w => w.Id == context.ParentFrameId)is { } window && window.ParentFrameId == damageId && _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window)is { IsSourceLess: false, DamageWasApplied: true } attack && attack.SourceSeat == source && attack.TargetSeat == target && attack.DamageAmount == context.Amount;
    internal bool DragonBoatTunjiangQualified(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || context.Window != SkillProgramTriggerWindow.TurnEnding || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !DragonBoatAllies(ownerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat))
            return false;
        // TurnStarted is an actual-turn boundary, including granted extra turns.
        var start = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToList().FindLastIndex(e => e is TurnStartedEvent);
        return start >= 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Skip(start + 1).OfType<DamageAppliedEvent>().Any(e => !e.SourceLess && e.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && e.Amount > 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42000)]
    internal bool AllowsDragonBoatGeneralTrigger(ProgramTriggerLegalityContext gate)
    {
        if (!gate.SkillId.StartsWith("dragonboat:", StringComparison.Ordinal))
            return true;
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity)
            return false;
        if (gate.SkillId == "dragonboat:lianpian")
            return DragonBoatLianpianTargets(gate.OwnerSeat, gate.Context).Count > 0;
        if (gate.SkillId == "dragonboat:dianhu" && gate.Context.Window == SkillProgramTriggerWindow.AfterDamageApplied)
            return DragonBoatDianhuQualified(gate.Candidate, gate.Context);
        if (gate.SkillId == "dragonboat:tunjiang")
            return DragonBoatTunjiangQualified(gate.OwnerSeat, gate.Context);
        if (gate.SkillId is "dragonboat:xingzhao" or "dragonboat:xingzhao-benefit")
            return _coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat].TeamId is { } team && _coreDragonBoatActivitiesOperations.DragonBoatTeamScore(team) >= (gate.Context.Window == SkillProgramTriggerWindow.DiscardPhaseStarting ? 6 : 4);
        return true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 42000, SkillProgramEffectOp.DrawAndOfferJianjiExactCard)]
    internal bool AllowsDragonBoatJianji(ProgramActivationLegalityContext gate) => gate.SkillId != "dragonboat:jianji" || _coreDragonBoatActivitiesOperations.IsDragonBoatActivity && _coreMatchParticipantsPlayersAccess._players.Any(p => p.Seat != gate.OwnerSeat && DragonBoatAllies(gate.OwnerSeat, p.Seat));
    internal SkillProgramStepOutcome BeginDragonBoatGeneral(ProgramSkillFrame supplied, SkillProgramEffectOp operation, int targetSeat)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || frame.TriggerId is null || frame.WindowContext is not { } context || frame.DragonBoatGeneral is not null || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            throw new InvalidOperationException("The activity general lost its actual unpaid trigger.");
        if (operation == SkillProgramEffectOp.ArmDragonBoatDianhu)
        {
            if (frame.SkillId != "dragonboat:dianhu" || context.Window != SkillProgramTriggerWindow.GameStarting || frame.InstructionIndex != 2 || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || DragonBoatDianhuTarget(frame.OwnerSeat, frame.SkillInstanceId)is not null)
                throw new InvalidOperationException("Activity Dianhu must record one real game-start target once.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatDianhuArmedEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, targetSeat, context.ParentFrameId));
            return SkillProgramStepOutcome.Continue;
        }

        if (frame.InstructionIndex != 1)
            throw new InvalidOperationException("Activity draw must have one exact producer.");
        var expected = operation switch
        {
            SkillProgramEffectOp.DrawDragonBoatConsecutiveTarget => "dragonboat:lianpian",
            SkillProgramEffectOp.DrawDragonBoatDianhu => "dragonboat:dianhu",
            SkillProgramEffectOp.DrawDragonBoatTunjiang => "dragonboat:tunjiang",
            _ => throw new InvalidOperationException("Unknown activity general operation.")};
        if (frame.SkillId != expected)
            throw new InvalidOperationException("Activity general operation has the wrong source.");
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0);
        if (operation == SkillProgramEffectOp.DrawDragonBoatDianhu && !DragonBoatDianhuQualified(candidate, context) || operation == SkillProgramEffectOp.DrawDragonBoatTunjiang && !DragonBoatTunjiangQualified(frame.OwnerSeat, context))
            throw new InvalidOperationException("The activity draw lost its exact native occurrence.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DragonBoatGeneral = new(frame.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, DragonBoatGeneralStage.Choosing) });
        if (operation == SkillProgramEffectOp.DrawDragonBoatDianhu)
        {
            IssueDragonBoatGeneralDraw(frame, frame.OwnerSeat, 1);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishDragonBoatGeneralChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> DragonBoatGeneralChoices(ProgramSkillFrame frame)
    {
        if (frame.DragonBoatGeneral is not { Stage: DragonBoatGeneralStage.Choosing } || frame.WindowContext is not { } context)
            return[];
        var targets = frame.SkillId == "dragonboat:lianpian" ? DragonBoatLianpianTargets(frame.OwnerSeat, context) : DragonBoatTunjiangQualified(frame.OwnerSeat, context) ? new[]
        {
            frame.OwnerSeat,
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat
        }.Distinct().ToArray() : [];
        return Array.AsReadOnly(targets.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Select(seat => new PromptChoice(new($"dragonboat.{(frame.Id)}.draw-{(seat)}"), $"令{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}摸{((frame.SkillId == "dragonboat:tunjiang" ? 2 : 1))}张牌", [], [seat], new Dictionary<string, string> { ["program-action"] = "dragonboat-general" })).ToArray());
    }

    internal void PublishDragonBoatGeneralChoice(ProgramSkillFrame frame)
    {
        var choices = DragonBoatGeneralChoices(frame);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DragonBoatGeneral = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择本次摸牌的角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.DrawDragonBoatConsecutiveTarget, SkillProgramEffectOp.DrawDragonBoatTunjiang)]
    internal PromptChoice ChooseDragonBoatGeneral(ProgramSkillFrame frame) => DragonBoatGeneralChoices(frame).OrderByDescending(c => DragonBoatAllies(frame.OwnerSeat, c.Targets.Single())).ThenBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).First();
    [ProgramAnswer("dragonboat-general")]
    internal void AnswerDragonBoatGeneral(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DragonBoatGeneral.Stage: DragonBoatGeneralStage.Choosing } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !DragonBoatGeneralChoices(frame).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("The activity draw requires its exact published participant choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        IssueDragonBoatGeneralDraw(frame, choice.Targets.Single(), frame.SkillId == "dragonboat:tunjiang" ? 2 : 1);
    }

    internal static CardMoveReason DragonBoatGeneralDrawReason(string skillId) => RuntimeSkills.RuntimeRules_Core_DragonBoatGenerals.DragonBoatGeneralDrawReason(skillId);
    internal void IssueDragonBoatGeneralDraw(ProgramSkillFrame frame, int recipient, int count)
    {
        if (frame.DragonBoatGeneral is not { Stage: DragonBoatGeneralStage.Choosing } receipt || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            throw new InvalidOperationException("An activity draw cannot repay a completed recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DragonBoatGeneral = receipt with { Stage = DragonBoatGeneralStage.DrawAttempt, RecipientSeat = recipient, RequestedCount = count }, Step = ResolutionFrameStep.ResolvingEffect });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, recipient, count, DragonBoatGeneralDrawReason(frame.SkillId));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawDragonBoatConsecutiveTarget, SkillProgramEffectOp.DrawDragonBoatDianhu, SkillProgramEffectOp.DrawDragonBoatTunjiang)]
    internal void ReturnDragonBoatGeneralDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (frame.NativeDraw is not null || frame.DragonBoatGeneral is not { Stage: DragonBoatGeneralStage.DrawAttempt } receipt || receipt.RecipientSeat != outcome.RecipientSeat || receipt.RequestedCount != outcome.RequestedCount || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.SequenceBefore > outcome.SequenceAfter || outcome.ActualDrawIds.Count > receipt.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason == DragonBoatGeneralDrawReason(frame.SkillId)).OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("The activity draw lost its native request/material return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DragonBoatGeneral = receipt with { Stage = DragonBoatGeneralStage.DrawChildren, Draw = outcome } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatDrawPaidEvent(frame.Id, frame.SkillId, frame.OwnerSeat, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId, outcome.Replacement is not null));
        ResumeDragonBoatGeneral(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 42000)]
    internal bool ResumeDragonBoatGeneral(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { DragonBoatGeneral: { } receipt } frame || frame.Id != id)
            return false;
        if (receipt.Stage == DragonBoatGeneralStage.Choosing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishDragonBoatGeneralChoice(frame);
            return true;
        }

        if (receipt.Stage == DragonBoatGeneralStage.DrawAttempt)
            throw new InvalidOperationException("The activity draw resumed before its native request returned.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { DragonBoatGeneral = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42000)]
    internal bool ReturnDragonBoatGeneralMovement(ProgramSkillFrame frame) => frame.DragonBoatGeneral is not null && ResumeDragonBoatGeneral(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 42000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42000)]
    internal bool ContinuePaidDragonBoatGeneral(ProgramSkillFrame frame)
    {
        if (frame.DragonBoatGeneral is not { Stage: DragonBoatGeneralStage.DrawAttempt or DragonBoatGeneralStage.DrawChildren })
            return false;
        return true;
    }

    internal IEnumerable<CharacterState> WenjiEligibleGiftRecipients(int ownerSeat, string skillId) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != ownerSeat && (skillId != "dragonboat:wenji" || DragonBoatAllies(ownerSeat, p.Seat)) && _characterLiuQiOlOperations.WenjiPayableCards(p.Seat).Count > 0);
    [ProgramAiChoice(SkillProgramEffectOp.DrawAndOfferJianjiExactCard)]
    internal PromptChoice SelectActivityAwareJianjiChoice(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => DragonBoatJianjiAiChoice(decision, frame) ?? ProgramAiChoiceSelector.SelectAiJianjiChoice(decision, frame, effect);
    internal PromptChoice? DragonBoatJianjiAiChoice(PendingDecision decision, ProgramSkillFrame frame)
    {
        if (frame.SkillId != "dragonboat:jianji" || !_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || frame.HuangQuan is not { Stage: "offer" } pending)
            return null;
        var actions = _characterHuangQuanOlOperations.JianjiActions(frame);
        var ranked = decision.Choices.Where(c => c.Parameters.GetValueOrDefault("option") != "decline").Select(choice =>
        {
            var index = int.Parse(choice.Parameters["option"], System.Globalization.CultureInfo.InvariantCulture);
            var action = actions[index];
            var harmful = action.Kind is LegalActionKind.Slash or LegalActionKind.Duel or LegalActionKind.FireAttack or LegalActionKind.Snatch or LegalActionKind.Dismantlement or LegalActionKind.AgainstCurrent;
            var score = harmful ? action.TargetSeats.Any(s => DragonBoatAllies(pending.RecipientSeat, s)) ? -100 : 20 : 10;
            if (harmful && action.TargetSeats.Any(target => _coreProgramEventHistoryOperations.ProgramEventHistory<DragonBoatDianhuArmedEvent>().Any(e => e.OwnerSeat == frame.OwnerSeat && e.TargetSeat == target && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[e.OwnerSeat], "dragonboat:dianhu", e.SkillInstanceId))))
                score += 5;
            if (action.Kind is LegalActionKind.Zong or LegalActionKind.Peach or LegalActionKind.SameBoat && action.TargetSeats.Any(s => !DragonBoatAllies(pending.RecipientSeat, s)))
                score = -100;
            return (Choice: choice, Score: score);
        }).OrderByDescending(item => item.Score).ToArray();
        return ranked.Length != 0 && ranked[0].Score > 0 ? ranked[0].Choice : decision.Choices.Single(c => c.Parameters.GetValueOrDefault("option") == "decline");
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawAndOfferJianjiExactCard)]
    internal void ReturnDragonBoatJianjiDraw(ProgramSkillFrame input, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (frame.SkillId != "dragonboat:jianji" || !_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || frame.NativeDraw is not null || frame.HuangQuan is not { Kind: "jianji", Stage: "drawing" } pending || pending.RecipientSeat != outcome.RecipientSeat || outcome.RequestedCount != 1 || outcome.ActualDrawIds.Count > 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.SequenceBefore > outcome.SequenceAfter || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(outcome.RecipientSeat) && m.Reason.Value == "program.jianji.draw").OrderBy(m => m.Sequence).Select(m => m.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("Activity Jianji lost its exact native draw request and actual card.");
        pending = pending with
        {
            Before = outcome.SequenceBefore,
            After = outcome.SequenceAfter,
            CardIds = outcome.ActualDrawIds,
            Stage = "draw-paid"
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HuangQuan = pending, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuangQuanDrawPaidEvent(frame.Id, pending.Source, frame.GameplayHash, "jianji", pending.RecipientSeat, pending.ActualTurn, pending.TurnOwner, pending.ActualPhase, pending.Before, pending.After, pending.CardIds.Count, null));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 42000)]
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 42000)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 42000)]
    internal void SynchronizeDragonBoatXingzhao()
    {
        if (!_coreDragonBoatActivitiesOperations.IsDragonBoatActivity || _coreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess._synchronizingDragonBoatGrants || _coreMatchParticipantsPlayersAccess._players.Count != 8)
            return;
        _coreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess._synchronizingDragonBoatGrants = true;
        try
        {
            var desired = new Dictionary<(int Seat, string Grant), (int Source, SkillGrant Grant)>();
            foreach (var source in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreNationalSkillsOperations.EnabledContentSkillIds(p).Contains("dragonboat:xingzhao", StringComparer.Ordinal)))
                foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(source).ActiveGrants.Where(g => g.SkillId == "dragonboat:xingzhao"))
                    foreach (var recipient in _coreMatchParticipantsPlayersAccess._players.Where(p => DragonBoatAllies(source.Seat, p.Seat)))
                    {
                        var skills = new List<string>();
                        if (source.Seat != recipient.Seat)
                            skills.Add("dragonboat:xingzhao-benefit");
                        if (source.TeamId is { } team && _coreDragonBoatActivitiesOperations.DragonBoatTeamScore(team) >= 2)
                            skills.Add("dragonboat:xunxun");
                        foreach (var skill in skills)
                        {
                            var id = $"dragonboat:xingzhao:{(source.Seat)}:{(instance.SkillInstanceId)}:{(recipient.Seat)}:{(skill)}";
                            var existing = recipient.SkillGrants.Grants.FirstOrDefault(g => g.GrantId == id);
                            var shared = _coreNationalSkillsOperations.GetSkillBindingShard(recipient).ActiveGrants.FirstOrDefault(g => g.SkillId == skill)?.SkillInstanceId ?? desired.Where(p => p.Key.Seat == recipient.Seat && p.Value.Grant.SkillId == skill).Select(p => p.Value.Grant.SkillInstanceId).FirstOrDefault() ?? id;
                            desired[(recipient.Seat, id)] = (source.Seat, existing ?? new(id, skill, shared, "dragonboat:xingzhao-source:" + source.Seat));
                        }
                    }

            foreach (var recipient in _coreMatchParticipantsPlayersAccess._players)
            {
                foreach (var stale in recipient.SkillGrants.Grants.Where(g => g.SourceId.StartsWith("dragonboat:xingzhao-source:", StringComparison.Ordinal) && !desired.ContainsKey((recipient.Seat, g.GrantId))).ToArray())
                {
                    recipient.SkillGrants.RemoveGrant(stale.GrantId);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatXingzhaoGrantEvent(-1, recipient.Seat, stale.GrantId, stale.SkillId, stale.SkillInstanceId, false));
                }

                foreach (var pair in desired.Where(p => p.Key.Seat == recipient.Seat && !recipient.SkillGrants.Grants.Any(g => g.GrantId == p.Key.Grant)))
                {
                    recipient.SkillGrants.Grant(pair.Value.Grant);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DragonBoatXingzhaoGrantEvent(pair.Value.Source, recipient.Seat, pair.Value.Grant.GrantId, pair.Value.Grant.SkillId, pair.Value.Grant.SkillInstanceId, true));
                }
            }
        }
        finally
        {
            _coreGameEngineDragonBoatGeneralsSynchronizingDragonBoatGrantsAccess._synchronizingDragonBoatGrants = false;
        }
    }
}
