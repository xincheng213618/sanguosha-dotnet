#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaMiDiOlRuntime
{
    internal const string MaMiDiDiscardReason = "skill-program.ma-mi-di.bingjie.discard";
    internal bool HasMaMiDiCapability => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GrowZhengdingMatchingResponse) == true;

    internal static CardConversionSource MaMiDiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_ma_mi_di_ol_MaMiDi.MaMiDiSource(f);
    SkillProgramStepOutcome IMaMiDiHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMaMiDi(f, op);
    internal bool ExactMaMiDiPlay(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == candidate.OwnerSeat && c.SourceSeat == candidate.OwnerSeat && c.TargetSeat == candidate.OwnerSeat && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } w && w.OwnerSeat == candidate.OwnerSeat && w.ItemIndex >= 0 && w.ItemIndex < w.Items.Count && w.Items[w.ItemIndex].Candidate == candidate;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27070)]
    internal bool CanOfferMaMiDi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects is not [var effect] || !MaMiDiComposition.Handles(effect.Op))
            return true;
        if (effect.Op == SkillProgramEffectOp.PayBingjieTurnMaximumHp)
            return ExactMaMiDiPlay(candidate, context) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].MaxHp > 0;
        if (effect.Op == SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards)
            return IsIssuedBingjieCandidate(candidate, context);
        return TryGetMaMiDiResponse(candidate, context, out _, out _);
    }

    internal SkillProgramStepOutcome BeginMaMiDi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MaMiDi is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            throw new InvalidOperationException("Ma Mi Di requires its unique current native instruction.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (!CanOfferMaMiDi(candidate, _coreProgramLifecycleOperations.GetProgramTrigger(f), c))
            throw new InvalidOperationException("Ma Mi Di lost its exact unpaid candidate or paid original-use entitlement.");
        MaMiDiReceipt r;
        if (op == SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards)
        {
            var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == c.ParentFrameId);
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)!;
            var issued = use.MaMiDiBingjie!;
            var targets = (window.Action.EffectiveKind == CardKind.BorrowedSword ? window.Action.TargetSeats.Where((_, i) => i % 2 == 0) : window.Action.EffectiveDesignatedTargetSeats).Where(s => s != f.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(s)).Distinct().ToArray();
            r = new(MaMiDiSource(f), f.GameplayHash, op, 1, c.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, MaMiDiStage.TargetChoice, use.Id, issued.ActionId)
            {
                Grants = issued.Grants.Where(g => g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId).ToArray(),
                Targets = targets
            };
            // This is the actual designation trigger, after earlier native role/target adjustments.
            // Collection only reserved possible grants; it never entitled the previous actor.
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MaMiDiBingjie = issued with { Issued = true } });
            foreach (var grant in r.Grants)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BingjieUseBoundEvent(use.Id, issued.ActionId, f.OwnerSeat, grant.FrameId, r.Turn, r.TurnOwner));
        }
        else
        {
            MaMiDiResponseLinkedEvent? response = null;
            if (!_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
                throw new InvalidOperationException("An unpaid maximum-HP instruction requires its live original source.");
            if (op == SkillProgramEffectOp.GrowZhengdingMatchingResponse && !TryGetMaMiDiResponse(candidate, c, out _, out response))
                throw new InvalidOperationException("Zhengding lost its frozen real response.");
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var after = op == SkillProgramEffectOp.PayBingjieTurnMaximumHp ? owner.MaxHp - 1 : checked(owner.MaxHp + 1);
            r = new(MaMiDiSource(f), f.GameplayHash, op, 1, c.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, MaMiDiStage.MaximumChildren, ActionId: response?.ResponseActionId, MaximumBefore: owner.MaxHp, MaximumAfter: after, HpBefore: owner.Hp, HpAfter: Math.Min(owner.Hp, after), Response: response);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaMiDi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.WindowId, r.Turn, r.TurnOwner, r.UseId, r.ActionId, r.Grants.Count, r.Targets.Count));
        for (var i = 0; i < r.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiFrozenTargetEvent(f.Id, i, r.Targets[i]));
        for (var i = 0; i < r.Grants.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiFrozenGrantEvent(f.Id, i, r.Grants[i].FrameId));
        if (r.Stage == MaMiDiStage.MaximumChildren)
        {
            if (op == SkillProgramEffectOp.PayBingjieTurnMaximumHp && r.MaximumAfter == 0)
            {
                // Same native zero-maximum-HP death protocol as Yisuan. The old
                // shared clamp remains untouched for every historical operation.
                var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                owner.MaxHp = 0;
                owner.Hp = r.HpAfter;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(owner.Seat, -1, 0, f.SkillId) { HpBeforeClamp = r.HpBefore });
            }
            else
                _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, op == SkillProgramEffectOp.PayBingjieTurnMaximumHp ? -1 : 1);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiMaximumIssuedEvent(f.Id, op, f.OwnerSeat, r.MaximumBefore, r.MaximumAfter, r.HpBefore, r.HpAfter));
            if (op == SkillProgramEffectOp.PayBingjieTurnMaximumHp && r.MaximumAfter == 0)
            {
                _coreNativePlayerDeathOperations.BeginPlayerDeath(f.Id, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], null, null, null, causingProgramSkillFrameId: f.Id);
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        ContinueMaMiDi(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal (BingjieGrantedEvent Grant, int Target) MaMiDiCurrentTarget(MaMiDiReceipt r) => (r.Grants[r.Cursor / r.Targets.Count], r.Targets[r.Cursor % r.Targets.Count]);
    internal IReadOnlyList<PromptChoice> MaMiDiChoices(ProgramSkillFrame f)
    {
        var r = f.MaMiDi!;
        if (r.Stage != MaMiDiStage.TargetChoice || r.Targets.Count == 0 || r.Cursor >= r.Grants.Count * r.Targets.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        var target = MaMiDiCurrentTarget(r).Target;
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, target, target, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "ma-mi-di", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "discard", ["cursor"] = r.Cursor.ToString(CultureInfo.InvariantCulture) } })).ToArray());
    }

    internal void PublishMaMiDi(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices)
    {
        var target = MaMiDiCurrentTarget(f.MaMiDi!).Target;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, target, "秉节：由你弃置一张手牌或装备牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = target,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[target].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("ma-mi-di")]
    internal void AnswerMaMiDi(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MaMiDi: { Stage: MaMiDiStage.TargetChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != MaMiDiCurrentTarget(r).Target || !MaMiDiChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Bingjie requires the actual target's current private physical discard.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var pair = MaMiDiCurrentTarget(r);
        var id = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var grain = from == CardLocation.Equipment(pair.Target) && card.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[pair.Target]).Select(c => c.Id).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaMiDi = r with { Stage = MaMiDiStage.TargetChildren }, PendingMovementContinuation = new(pair.Target, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, pair.Target, card, from, new(MaMiDiDiscardReason));
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == id && m.Reason.Value == MaMiDiDiscardReason);
        var step = new MaMiDiDiscardStep(r.Cursor, pair.Grant.FrameId, pair.Target, id, card.Kind, from, moved.To, before, after)
        {
            WoodenOxGrain = grain
        };
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MaMiDi = f.MaMiDi!with { Steps = r.Steps.Append(step).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiDiscardIssuedEvent(f.Id, step.Index, step.GrantFrameId, step.TargetSeat, step.CardId, step.Kind, step.From, step.To, step.Before, step.After));
        ContinueMaMiDi(f.Id);
    }

    internal bool DrainMaMiDi(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void ContinueMaMiDi(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MaMiDi: { } r } f && f.Id == id)
        {
            if (r.Stage != MaMiDiStage.TargetChoice)
            {
                if (DrainMaMiDi(f))
                    return;
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                r = f.MaMiDi!;
                if (r.Stage == MaMiDiStage.MaximumChildren)
                {
                    if (r.Op == SkillProgramEffectOp.PayBingjieTurnMaximumHp && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BingjieGrantedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.TurnOwner, r.WindowId));
                    FinishMaMiDi(f);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaMiDi = r with { Stage = MaMiDiStage.TargetChoice, Cursor = r.Cursor + 1 }, PendingMovementContinuation = null });
                r = f.MaMiDi!;
            }

            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Cursor >= r.Targets.Count * r.Grants.Count)
            {
                FinishMaMiDi(f);
                return;
            }

            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return;
            var choices = MaMiDiChoices(f);
            if (choices.Count > 0)
            {
                PublishMaMiDi(f, choices);
                return;
            }

            var pair = MaMiDiCurrentTarget(r);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiTargetSkippedEvent(f.Id, r.Cursor, pair.Grant.FrameId, pair.Target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MaMiDi = r with { Cursor = r.Cursor + 1 } });
        }
    }

    internal void FinishMaMiDi(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaMiDiCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MaMiDi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27070)]
    internal bool ResumeMaMiDi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MaMiDi: not null } f || f.Id != id)
            return false;
        ContinueMaMiDi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27070)]
    internal bool ReturnMaMiDiMovement(ProgramSkillFrame f)
    {
        if (f.MaMiDi is null)
            return false;
        ContinueMaMiDi(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27070)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27070)]
    internal bool ContinuePaidMaMiDi(ProgramSkillFrame f)
    {
        if (f.MaMiDi is not null)
        {
            return true;
        }

        // The already-paid turn entitlement is also the authority for entering
        // this new frame after source loss, before its first instruction owns
        // the per-target payment receipt. Unpaid maximum-HP entries never use it.
        return f.InstructionIndex is 0 or 1 && f.TriggerId == "issued-turn-target-discards" && f.WindowContext is { } c && _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is [{ Op: SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards }] && IsIssuedBingjieCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards)]
    internal PromptChoice AiMaMiDi(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => c.Cards is [var id] ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
