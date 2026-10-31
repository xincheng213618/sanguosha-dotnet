#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongHappyGodRuntime
{
    internal const string HappyPtAction = "happy-pang-tong";
    SkillProgramStepOutcome IHappyPangTongHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginHappyPangTong(f, op);
    internal CardConversionSource HappyPtSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasHappyPangTong => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.IssueHappyPangTongPlan);

    internal HappyPangTongPlanIssuedEvent HappyPtPlan(long id) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongPlanIssuedEvent>().Single(e => e.PlanId == id);
    internal IEnumerable<HappyPangTongPlanIssuedEvent> HappyPtOutstanding(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongPlanIssuedEvent>().Where(p => p.Source.OwnerSeat == owner && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongPlanSettledEvent>().Any(s => s.PlanId == p.PlanId));
    internal long[] HappyPtDue(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && context.CardUse is { } card && card.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(card.EffectiveKind) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongSlashRecordedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.ActorSeat == card.ActorSeat && e.UseId == card.ParentCardUseFrameId && e.ActionId == card.CardActionId && e.Ordinal == 1))
            return HappyPtOutstanding(owner).Where(p => p.Kind == HappyPangTongPlanKind.Middle && p.TargetSeat == card.ActorSeat && p.PlanId < card.ParentCardUseFrameId && p.IssuedActionBoundary < card.CardActionId).Select(p => p.PlanId).Order().ToArray();
        if (context.Window == SkillProgramTriggerWindow.AfterTurnEnded && context.TargetSeat is { } actor && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is { } parent && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.OwnerSeat == actor)
            return HappyPtOutstanding(owner).Where(p => p.TargetSeat == actor && p.PlanId < parent.Id).Select(p => p.PlanId).Order().ToArray();
        return[];
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42180)]
    internal bool CanOfferHappyPangTong(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId is not (HappyPangTongComposition.Lunce or HappyPangTongComposition.Lanhai))
            return true;
        if (g.WindowContext is not { } context)
            return false;
        return g.Trigger.Id switch
        {
            "round-plan" => IsHappyPtActualRoundStart(g.Candidate, context),
            "first-slash-plan" or "ended-plan" => HappyPtDue(g.OwnerSeat, context).Length > 0,
            "settled-plan" or "renew-plan" => CanRunHappyPangTongSettled(g.Candidate, context),
            _ => true
        };
    }

    internal bool IsHappyPtActualRoundStart(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.RoundStarting || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(facts, e => e is RoundStartedEvent r && r.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        if (start < 0 || facts[start] is not RoundStartedEvent round || round.ActorSeat != context.SourceSeat)
            return false;
        var since = facts.Skip(start + 1).ToArray();
        if (since.OfType<ProgramBindingResolvedEvent>().Any(e => e.Window == SkillProgramTriggerWindow.RoundStarting && e.OwnerSeat == candidate.OwnerSeat && e.SkillId == candidate.SkillId && e.BindingId == candidate.BindingId && e.SkillInstanceId == candidate.SkillInstanceId))
            return false;
        var turns = since.OfType<TurnStartedEvent>().ToArray();
        return window.BeforeTurnPreludeProof is not null ? turns.Length == 0 : turns is [var first] && first.ActorSeat == round.ActorSeat && first.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 42180)]
    internal void ObserveHappyPangTongTurnFacts(IGameEvent payload)
    {
        if (!HasHappyPangTong)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCurrentTurnUseKindsOperations.HasOngoingActualUseTurn && declared.SourceSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(declared.CardKind) && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } action } use && action.ActorSeat == declared.SourceSeat && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongSlashRecordedEvent>().Any(e => e.UseId == use.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongSlashRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.Id, action.ActionId, 1 + _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongSlashRecordedEvent>().Count(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)));
        if (payload is DamageAppliedEvent { Amount: > 0, SourceLess: false } damage && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && _resolutionStackOperations._resolutionStack.LastOrDefault()is DamageFrame d && d.SourceSeat == damage.SourceSeat && d.TargetSeat == damage.TargetSeat && d.Amount == damage.Amount && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && d.ParentFrameId == attack.ResolutionId && attack.DamageWasApplied && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongDamageRecordedEvent>().Any(e => e.DamageId == d.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongDamageRecordedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, d.Id, damage.SourceSeat, damage.Amount));
    }

    internal SkillProgramStepOutcome BeginHappyPangTong(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.HappyPangTong is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || !HappyPtOriginalCandidate(f))
            throw new InvalidOperationException("Lunce requires its exact original real window and unstarted frame.");
        var stage = operation == SkillProgramEffectOp.IssueHappyPangTongPlan ? HappyPangTongStage.Grant : HappyPangTongStage.NextPlan;
        var receipt = new HappyPangTongReceipt(HappyPtSource(f), f.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, stage);
        if (operation is SkillProgramEffectOp.ResolveHappyPangTongMiddle or SkillProgramEffectOp.ResolveHappyPangTongEnding)
        {
            receipt = receipt with
            {
                Plans = HappyPtDue(f.OwnerSeat, context)
            };
            if (receipt.Plans.Count == 0)
                return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongStartedEvent(f.Id, receipt.Source, f.GameplayHash, receipt.ParentId, operation, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        for (var ordinal = 0; ordinal < receipt.Plans.Count; ordinal++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongDueIssuedEvent(f.Id, ordinal, receipt.Plans[ordinal], receipt.ParentId));
        if (operation == SkillProgramEffectOp.IssueHappyPangTongPlan)
            PublishHappyPt(f);
        else if (operation == SkillProgramEffectOp.ResolveHappyPangTongLanhai)
            BeginHappyPtReward(f);
        else
            ContinueHappyPt(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice HappyPtChoice(ProgramSkillFrame f, string mode, string text, IReadOnlyList<int>? targets = null, string? value = null) => new(new($"happy-plan.{(f.Id)}.{(f.HappyPangTong!.CurrentPlanId)}.{(f.HappyPangTong.Stage)}.{(mode)}.{(value)}.{(string.Join('-', targets ?? []))}"), text, [], targets ?? [], new Dictionary<string, string> { { "program-action", HappyPtAction }, { "frame-id", f.Id.ToString(CultureInfo.InvariantCulture) }, { "mode", mode }, { "value", value ?? "" } });
    internal IReadOnlyList<PromptChoice> HappyPtChoices(ProgramSkillFrame f)
    {
        var r = f.HappyPangTong!;
        var choices = new List<PromptChoice>();
        if (r.Stage == HappyPangTongStage.Grant)
            foreach (var kind in Enum.GetValues<HappyPangTongPlanKind>())
                foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                    choices.Add(HappyPtChoice(f, "grant", $"对{(p.Name)}定{((kind == HappyPangTongPlanKind.Upper ? "上" : kind == HappyPangTongPlanKind.Middle ? "中" : "下"))}策", [p.Seat], kind.ToString()));
        if (r.Stage == HappyPangTongStage.LowerChoice)
        {
            choices.Add(HappyPtChoice(f, "lower", "摸两张牌，并令定策角色回复1点体力"));
            choices.Add(HappyPtChoice(f, "decline", "不发动下策"));
        }

        if (r.Stage == HappyPangTongStage.UpperTarget)
        {
            foreach (var seat in _coreAssistedPhysicalCardsOperations.AssistedPhysicalSlashTargets(f.OwnerSeat))
                choices.Add(HappyPtChoice(f, "slash-target", $"选择对{(_coreMatchParticipantsPlayersAccess._players[seat].Name)}使用【杀】", [seat], seat.ToString(CultureInfo.InvariantCulture)));
            choices.Add(HappyPtChoice(f, "decline", "不使用【杀】"));
        }

        if (r.Stage == HappyPangTongStage.UpperCard)
        {
            var view = f with
            {
                AssistedSlashRequest = new(f.OwnerSeat, r.SlashTarget, ActorChoosesTarget: true)
            };
            foreach (var c in _coreNearestLegalSlashRequestsOperations.NearestPhysicalSlashChoices(view))
            {
                var parameters = new Dictionary<string, string>(c.Parameters)
                {
                    {
                        "mode",
                        c.Parameters["request-option"]
                    }
                };
                parameters["program-action"] = HappyPtAction;
                choices.Add(c with { Id = new($"happy-upper.{(f.Id)}.{(r.CurrentPlanId)}.{(c.Id.Value)}"), Parameters = parameters });
            }
        }

        if (r.Stage == HappyPangTongStage.MiddleCard)
        {
            var plan = HappyPtPlan(r.CurrentPlanId);
            var cardUse = f.WindowContext!.CardUse!;
            var seats = new[]
            {
                plan.TargetSeat
            }.Concat(cardUse.DesignatedTargetSeats ?? _coreCardUseLifecycleOperations.LifecycleCardUse(cardUse.ParentCardUseFrameId)?.TargetSeats ?? []).Distinct();
            foreach (var seat in seats.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive))
                foreach (var c in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, seat, seat == f.OwnerSeat ? [CardZoneKind.Equipment] : [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Obtain))
                {
                    var parameters = new Dictionary<string, string>(c.Parameters)
                    {
                        {
                            "mode",
                            "take"
                        },
                        {
                            "card-owner",
                            seat.ToString(CultureInfo.InvariantCulture)
                        }
                    };
                    parameters["program-action"] = HappyPtAction;
                    choices.Add(c with { Id = new($"happy-middle.{(f.Id)}.{(r.CurrentPlanId)}.{(seat)}.{(c.Id.Value)}"), Targets = Array.AsReadOnly(new[] { seat }), Parameters = parameters });
                }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishHappyPt(ProgramSkillFrame f)
    {
        var choices = HappyPtChoices(f);
        if (choices.Count == 0)
        {
            if (f.HappyPangTong!.Stage == HappyPangTongStage.Grant)
                FinishHappyPt(f);
            else
                SettleHappyPt(f, false);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, "【论策】请选择。", f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(HappyPtAction)]
    internal void AnswerHappyPt(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyPangTong: { } r } f || !ValidHappyPt(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], HappyPtChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lunce lost its exact current native menu or original plan.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var mode = choice.Parameters["mode"];
        if (r.Stage == HappyPangTongStage.Grant)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongPlanIssuedEvent(f.Id, r.Source, r.GameplayHash, Enum.Parse<HappyPangTongPlanKind>(choice.Parameters["value"]), choice.Targets.Single(), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence));
            FinishHappyPt(f);
            return;
        }

        if (mode == "decline")
        {
            SettleHappyPt(f, false);
            return;
        }

        if (r.Stage == HappyPangTongStage.UpperTarget)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = HappyPangTongStage.UpperCard, SlashTarget = choice.Targets.Single() } });
            PublishHappyPt(f);
            return;
        }

        if (r.Stage == HappyPangTongStage.UpperCard)
        {
            if (mode == "faction")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = HappyPangTongStage.UpperUse, FactionPending = true } });
                _coreAssistedFactionSlashOperations.BeginAssistedProgramFactionSlashRequest(f, f.OwnerSeat, r.SlashTarget, HappyPtUpperBind(f));
                return;
            }

            var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var(cards, kind, conversion) = _coreAssistedSlashPaymentOperations.ReadAssistedSlashPayment(actor, choice);
            if (!_coreNearestLegalSlashRequestsOperations.NearestSlashPaymentHasRange(actor, r.SlashTarget, cards, conversion, kind) || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, _coreMatchParticipantsPlayersAccess._players[r.SlashTarget], cards[0], conversion, kind, noEffectiveRank: cards.Count > 1, specificEffectiveRank: choice.Parameters.GetValueOrDefault("equipment") == CardKind.ZhangbaSerpentSpear.ToString() ? _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(actor, cards) : null, physicalCardIds: cards.Select(c => c.Id).ToArray()))
                throw new InvalidOperationException("Upper plan Slash lost its real payment or legal target.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyPangTong = r with { Stage = HappyPangTongStage.UpperUse } });
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[r.SlashTarget], cards[0], kind, actor.Seat, physicalCards: cards, countsTowardSlashLimit: false, usesZhuqueFan: kind == CardKind.FireSlash && (cards[0].Kind == CardKind.Slash || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(actor, cards[0])) && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor), conversionSource: conversion, programSkillCardUseFrameId: f.Id);
            return;
        }

        if (r.Stage == HappyPangTongStage.LowerChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = HappyPangTongStage.LowerDrawing, LowerAccepted = true, DrawCount = 2 } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 2, new("skill-program.happy:lunce.lower-draw"));
            return;
        }

        if (r.Stage != HappyPangTongStage.MiddleCard)
            throw new InvalidOperationException("Lunce cannot pay a resolved plan twice.");
        var source = int.Parse(choice.Parameters["card-owner"], CultureInfo.InvariantCulture);
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var from = new CardLocation(zone, source);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
        f = HappyPtChildren(f, HappyPangTongStage.GainChildren);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = f.HappyPangTong!with { GainCardId = card.Id, GainFrom = from } });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new("skill-program.happy:lunce.middle-gain"));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyPangTong = f.HappyPangTong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueHappyPt(f.Id);
    }

    internal ProgramSkillFrame HappyPtChildren(ProgramSkillFrame f, HappyPangTongStage stage)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = f.HappyPangTong!with { Stage = stage, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        return f;
    }

    internal bool DrainHappyPt(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    internal void ContinueHappyPt(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyPangTong: { } r } f && f.Id == id)
        {
            if (!ValidHappyPt(f))
                throw new InvalidOperationException("Lunce continuation lost its original plan, native candidate or already-paid return.");
            if (r.Stage is HappyPangTongStage.UpperUse)
            {
                if (r.FactionPending && r.NativeUseId == 0 && f.ChoiceBindings.SingleOrDefault(b => b.Name == HappyPtUpperBind(f))is { OptionId: "declined" })
                {
                    SettleHappyPt(f, false);
                    return;
                }

                return;
            }

            if (r.Stage is HappyPangTongStage.Settling or HappyPangTongStage.LowerDrawing or HappyPangTongStage.RewardDrawing)
                return;
            if (DrainHappyPt(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.HappyPangTong!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == HappyPangTongStage.NextPlan)
            {
                if (r.Cursor >= r.Plans.Count || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    FinishHappyPt(f);
                    return;
                }

                var plan = HappyPtPlan(r.Plans[r.Cursor]);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r = r with { CurrentPlanId = plan.PlanId, SlashTarget = -1, NativeUseId = 0, NativeActionId = 0, FactionPending = false, GainCardId = 0, GainFrom = null, DrawOutcome = null, LowerAccepted = false } });
                if (r.Operation == SkillProgramEffectOp.ResolveHappyPangTongMiddle)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = HappyPangTongStage.MiddleCard } });
                    PublishHappyPt(f);
                    return;
                }

                var damaged = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongDamageRecordedEvent>().Any(e => e.ActualTurn == r.ActualTurn && e.SourceSeat == plan.TargetSeat);
                if (plan.Kind == HappyPangTongPlanKind.Middle || plan.Kind == HappyPangTongPlanKind.Upper && !damaged || plan.Kind == HappyPangTongPlanKind.Lower && damaged)
                {
                    SettleHappyPt(f, false);
                    return;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = plan.Kind == HappyPangTongPlanKind.Upper ? HappyPangTongStage.UpperTarget : HappyPangTongStage.LowerChoice } });
                PublishHappyPt(f);
                return;
            }

            if (r.Stage == HappyPangTongStage.LowerDrawChildren)
            {
                f = HappyPtChildren(f, HappyPangTongStage.LowerRecoveryChildren);
                var target = HappyPtPlan(r.CurrentPlanId).TargetSeat;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongRecoveryIssuedEvent(f.Id, r.CurrentPlanId, target, 1));
                if (_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
                    _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target, 1, null, null);
                continue;
            }

            if (r.Stage is HappyPangTongStage.GainChildren or HappyPangTongStage.UpperReturned or HappyPangTongStage.LowerRecoveryChildren)
            {
                SettleHappyPt(f, true);
                return;
            }

            if (r.Stage == HappyPangTongStage.GrowthMaximumChildren)
            {
                f = HappyPtChildren(f, HappyPangTongStage.GrowthHpChildren);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongRecoveryIssuedEvent(f.Id, r.CurrentPlanId, f.OwnerSeat, 1));
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
                continue;
            }

            if (r.Stage is HappyPangTongStage.RewardChildren or HappyPangTongStage.GrowthHpChildren)
            {
                FinishHappyPt(f);
                return;
            }

            PublishHappyPt(f);
            return;
        }
    }

    internal void SettleHappyPt(ProgramSkillFrame supplied, bool success)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.HappyPangTong!;
        var plan = HappyPtPlan(r.CurrentPlanId);
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongPlanSettledEvent>().Any(e => e.PlanId == plan.PlanId))
            throw new InvalidOperationException("An independently issued plan may settle exactly once.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = HappyPangTongStage.Settling } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongPlanSettledEvent(f.Id, plan.PlanId, f.OwnerSeat, plan.TargetSeat, plan.Kind, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, success));
        if (!BeginHappyPtSettled(f, success))
            ReturnHappyPtSettled(f.Id, plan.PlanId);
    }

    internal void BeginHappyPtReward(ProgramSkillFrame f)
    {
        var w = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(p => p.Id == f.WindowContext!.ParentFrameId);
        var ret = w.HappyPlanReturn!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = f.HappyPangTong!with { CurrentPlanId = ret.PlanId } });
        if (ret.Success)
        {
            var count = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongPlanSettledEvent>().Count(e => e.OwnerSeat == f.OwnerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Success);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = f.HappyPangTong!with { Stage = HappyPangTongStage.RewardDrawing, DrawCount = count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongBenefitIssuedEvent(f.Id, ret.PlanId, "draw", count, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, count, new("skill-program.happy:lanhai.success-draw"));
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var before = owner.MaxHp;
        f = HappyPtChildren(f, HappyPangTongStage.GrowthMaximumChildren);
        var actualGrowth = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongBenefitIssuedEvent>().Count(e => e.Branch == "grow" && e.MaximumAfter > e.MaximumBefore && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyPangTongStartedEvent>().Any(s => s.FrameId == e.FrameId && s.Source.OwnerSeat == f.OwnerSeat)) < 3;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = f.HappyPangTong!with { MaximumBefore = before, MaximumAfter = before + (actualGrowth ? 1 : 0) } });
        if (actualGrowth)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().ChangeMaximumHp(f, 1);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappyPangTong = f.HappyPangTong!with { MaximumAfter = owner.MaxHp } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongBenefitIssuedEvent(f.Id, ret.PlanId, "grow", 1, before, owner.MaxHp));
        ContinueHappyPt(f.Id);
    }

    internal void FinishHappyPt(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 42180)]
    internal bool ResumeHappyPt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyPangTong: not null } f || f.Id != id)
            return false;
        ContinueHappyPt(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42180)]
    internal bool ReturnHappyPtMovement(ProgramSkillFrame f) => f.HappyPangTong is not null && ResumeHappyPt(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyPangTongEnding)]
    internal void ReturnHappyPtLowerDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnHappyPtDraw(f, outcome, HappyPangTongStage.LowerDrawing, HappyPangTongStage.LowerDrawChildren);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyPangTongLanhai)]
    internal void ReturnHappyPtRewardDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnHappyPtDraw(f, outcome, HappyPangTongStage.RewardDrawing, HappyPangTongStage.RewardChildren);
    internal void ReturnHappyPtDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome, HappyPangTongStage from, HappyPangTongStage to)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.HappyPangTong!;
        if (!ValidHappyPt(f) || r.Stage != from || f.NativeDraw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != r.DrawCount || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Lunce/Lanhai lost its exact once-paid native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappyPangTong = r with { Stage = to, DrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyPangTongDrawReturnedEvent(f.Id, r.CurrentPlanId, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueHappyPt(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.IssueHappyPangTongPlan, SkillProgramEffectOp.ResolveHappyPangTongMiddle, SkillProgramEffectOp.ResolveHappyPangTongEnding)]
    internal PromptChoice HappyPtAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode") == "grant" && c.Parameters.GetValueOrDefault("value") == HappyPangTongPlanKind.Lower.ToString() && c.Targets.Contains(f.OwnerSeat)) ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode")is "lower" or "take" or "use") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("mode") == "decline") ?? p.Choices[0];
}
